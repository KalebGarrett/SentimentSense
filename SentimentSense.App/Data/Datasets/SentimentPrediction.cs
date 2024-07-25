using Microsoft.ML.Data;

namespace SentimentSense.App.Data.Datasets;

public class SentimentPrediction
{
    [ColumnName("PredictedLabel")] public bool Prediction { get; set; }
    public float Probability { get; set; }
    public float Score { get; set; }
}