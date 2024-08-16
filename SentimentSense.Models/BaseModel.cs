using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace SentimentSense.Models;

public class BaseModel
{
    [JsonPropertyName("id")] [BsonId] public string Id { get; set; } 
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; } 
    [JsonPropertyName("updatedAt")] public DateTime UpdatedAt { get; set; }
    [JsonPropertyName("dtoVersion")] public int DtoVersion { get; set; }
}