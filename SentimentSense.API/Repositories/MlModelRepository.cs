using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SentimentSense.API.Repositories.Interfaces;
using SentimentSense.Models;

namespace SentimentSense.API.Repositories;

public class MlModelRepository : IMongoRepository<MlModel>
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
        var model = await GetCollection().AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        return model;
    }

    public async Task<MlModel> InsertOne(MlModel data)
    {
        data.Id = Guid.NewGuid().ToString();
        data.CreatedAt = DateTime.UtcNow;
        data.UpdatedAt = DateTime.UtcNow;
        data.DtoVersion = 1;
        await GetCollection().InsertOneAsync(data);
        var mlModelList = await GetCollection().AsQueryable().ToListAsync();
        return mlModelList.FirstOrDefault(x => x.Id == data.Id)!;
    }

    public Task<MlModel> InsertMany(ICollection<MlModel> documents)
    {
        throw new NotImplementedException();
    }

    // Fix
    public async Task<MlModel> ReplaceOne(string id, MlModel data)
    {
        data.UpdatedAt = DateTime.UtcNow;
        data.DtoVersion = data.DtoVersion++;
        await GetCollection().ReplaceOneAsync(id, data);
        return data;
    }

    public async Task DeleteById(string id)
    {
        await GetCollection().DeleteOneAsync(x => x.Id == id);
    }

    public Task DeleteMany(Expression<Func<MlModel, bool>> filterExpression)
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