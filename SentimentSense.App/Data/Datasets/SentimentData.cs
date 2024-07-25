using Microsoft.ML.Data;

namespace SentimentSense.App.Data.Datasets;

public class SentimentData
{
    [LoadColumn(0)] public string SentimentText { get; set; }
    [LoadColumn(1)] public bool Sentiment { get; set; }
}