

using AuthManager;
using common.Api.MassTransit;
using common.Api.MongoDB;
using common.Api.Settings;
using Payment.Api.Entities;



var builder = WebApplication.CreateBuilder(args);



var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddControllers(opt =>
{
    opt.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddMongo().AddMongoRepositotry<PaymentItem>("Payments")
.AddMassTransitWithRabbitMq();
builder.Services.AddCustomeAuthentication();
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
