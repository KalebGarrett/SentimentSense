using System.Linq.Expressions;
using SentimentSense.Models;

namespace SentimentSense.API.Repositories.Interfaces;

public interface IMongoRepository<T> where T : class
{
    public Task<IEnumerable<T>> FindAll();
    public Task<T> FindById(string id);
    public Task<T> InsertOne(T data);
    public Task<T> InsertMany(ICollection<T> documents);
    public Task<T> ReplaceOne(string id, T data);
    public Task DeleteById(string id);
    public Task DeleteMany(Expression<Func<T, bool>> filterExpression);
}