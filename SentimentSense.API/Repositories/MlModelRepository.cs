using System.Linq.Expressions;
using MongoDB.Driver;
using SentimentSense.API.Repositories.Interfaces;
using SentimentSense.Models;

namespace SentimentSense.API.Repositories;

public class MlModelRepository : IMongoRepository
{
    private readonly IMongoClient _mongoClient;

    public MlModelRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<IEnumerable<MlModel>> FindAll()
    {
        var models = await GetCollection().AsQueryable().ToListAsync();
        return models;
    }

    public async Task<MlModel> FindById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<MlModel> InsertOne(MlModel data)
    {
        data.Id = Guid.NewGuid().ToString();
        data.CreatedAt = DateTime.UtcNow;
        data.UpdatedAt = DateTime.UtcNow;
        await GetCollection().InsertOneAsync(data);
        var mlModelList = await GetCollection().AsQueryable().ToListAsync();
        return mlModelList.FirstOrDefault(x => x.Id == data.Id);
    }

    public Task<MlModel> InsertMany(ICollection<MlModel> documents)
    {
        throw new NotImplementedException();
    }

    public Task<MlModel> ReplaceOne(string id, MlModel data)
    {
        throw new NotImplementedException();
    }

    public Task<MlModel> DeleteById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<MlModel> DeleteMany(Expression<Func<MlModel, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }
    
    private IMongoCollection<MlModel> GetCollection()
    {
        var db = _mongoClient.GetDatabase("SentimentSense");
        var collection = db.GetCollection<MlModel>("MlModels");
        return collection;
    }
}