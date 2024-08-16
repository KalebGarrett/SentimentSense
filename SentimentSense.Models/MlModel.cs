using System.Text.Json.Serialization;

namespace SentimentSense.Models;

public class MlModel : BaseModel
{
    //public byte[] Model { get; set; }
    [JsonPropertyName("model")] public string Model { get; set; }
}