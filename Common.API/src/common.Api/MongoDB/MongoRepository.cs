
using System.Linq.Expressions;
using MongoDB.Driver;


namespace common.Api.MongoDB;

public class MongoRepository<P> : IRepository<P> where P : IEntity
{

    private readonly IMongoCollection<P> dbCollection;
    private readonly FilterDefinitionBuilder<P> filterBuilder = Builders<P>.Filter;

    public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
    {

        dbCollection = mongoDatabase.GetCollection<P>(collectionName);
    }

    public async Task<IReadOnlyCollection<P>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }
    public async Task<IReadOnlyCollection<P>> GetAllAsync(Expression<Func<P, bool>> filter)
    {
        return await dbCollection.Find(filter).ToListAsync();
    }
    public async Task<P> GetAsync(Guid id)
    {
        FilterDefinition<P> filter = filterBuilder.Eq(item => item.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task<P> GetAsync(Expression<Func<P, bool>> filter)
    {
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task CreateAsync(P payment)
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));
        await dbCollection.InsertOneAsync(payment);
    }

    public async Task UpdateAsync(P payment)
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));
        FilterDefinition<P> filter = filterBuilder.Eq(exItem => exItem.Id, payment.Id);
        await dbCollection.ReplaceOneAsync(filter, payment);
    }
    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<P> filter = filterBuilder.Eq(item => item.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }





}
