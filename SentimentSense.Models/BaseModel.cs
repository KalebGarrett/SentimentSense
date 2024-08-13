using MongoDB.Bson.Serialization.Attributes;

namespace SentimentSense.Models;

public class BaseModel
{
    [BsonId]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; }
}