using System.Linq.Expressions;
using SentimentSense.Models;

namespace SentimentSense.API.Repositories.Interfaces;

public interface IMongoRepository
{
    public Task<IEnumerable<MlModel>> FindAll();
    public Task<MlModel> FindById(string id);
    public Task<MlModel> InsertOne(MlModel data);
    public Task<MlModel> InsertMany(ICollection<MlModel> documents);
    public Task<MlModel> ReplaceOne(string id, MlModel data);
    public Task<MlModel> DeleteById(string id);
    public Task<MlModel> DeleteMany(Expression<Func<MlModel, bool>> filterExpression);
}