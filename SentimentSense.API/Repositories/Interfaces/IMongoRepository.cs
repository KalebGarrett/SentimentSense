using System.Linq.Expressions;
using SentimentSense.Models;

namespace SentimentSense.API.Repositories.Interfaces;

public interface IMongoRepository
{
    public Task<IEnumerable<BaseModel>> FindAll();
    public Task<BaseModel> FindById(string id);
    public Task<BaseModel> InsertOne(BaseModel data);
    public Task<BaseModel> InsertMany(ICollection<BaseModel> documents);
    public Task<BaseModel> ReplaceOne(string id, BaseModel data);
    public Task<BaseModel> DeleteById(string id, bool hardDelete = false);
    public Task<BaseModel> DeleteMany(Expression<Func<BaseModel, bool>> filterExpression);
}