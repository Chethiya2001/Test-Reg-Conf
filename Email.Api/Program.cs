
using AuthManager;
using common.Api.MassTransit;
using common.Api.MongoDB;
using Email.Api.Clients;
using Email.Api.Entities;
using Email.Api.services;
using MassTransit;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

Random jiter = new Random();
builder.Services.AddMongo().AddMongoRepositotry<EmailItem>("Email")
.AddMongoRepositotry<PaymentItem>("PaymentItems")
.AddMassTransitWithRabbitMq();
builder.Services.AddCustomeAuthentication();
NewMethod(builder, jiter);

//email
builder.Services.AddScoped<EmailService>(sp =>
        new EmailService(
            builder.Configuration["Email:SmtpServer"],
            builder.Configuration.GetValue<int>("Email:SmtpPort"),
            builder.Configuration["Email:SmtpUser"],
            builder.Configuration["Email:SmtpPass"]!
        ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static void NewMethod(WebApplicationBuilder builder, Random jiter)
{
    builder.Services.AddHttpClient<PaymentClient>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5229");
    })
    .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
        5,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
        TimeSpan.FromMilliseconds(jiter.Next(0, 1000)),
        onRetry: (outcome, timeSpan, retryAttempt) =>
        {

            Console.WriteLine($"Delaying for {timeSpan.TotalSeconds} seconds, then making retry {retryAttempt}");
        }
    ))
    .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
        3,
        TimeSpan.FromSeconds(15),
        onBreak: (outcome, timeSpan) =>
        {

            Console.WriteLine($"Opening circute for {timeSpan.TotalSeconds} seconds...");
        }
    ,
        onReset: () => Console.WriteLine("Closing the circuit...")
    ))
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
}