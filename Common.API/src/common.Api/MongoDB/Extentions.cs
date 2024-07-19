using common.Api.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;



namespace common.Api.MongoDB;

public static class Extentions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        // Bind ServiceSettings from the configuration


        services.AddSingleton(serviceProvider =>
        {
            var Configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var mongoDbSettings = configuration.GetSection(nameof(MondoDbSettings)).Get<MondoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings!.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings!.ServiceName);
        });
        return services;

    }
    public static IServiceCollection AddMongoRepositotry<P>(this IServiceCollection services, string collectionName) where P : IEntity
    {
        // Add services to the container.
        services.AddSingleton<IRepository<P>>(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return new MongoRepository<P>(database, collectionName);
        });
        return services;

    }
}
