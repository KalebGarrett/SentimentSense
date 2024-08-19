﻿using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace SentimentSense.Models;

public class MlModel : BaseModel
{
    [JsonPropertyName("model")] public IFormFile Model { get; set; }
}