using Microsoft.AspNetCore.Components;
using SentimentSense.App.Services;
using SentimentSense.App.Services.Interfaces;

namespace SentimentSense.App.Components.Pages;

public partial class Home
{
    [Inject] private SentimentService SentimentService { get; set; }
    private string SentimentText { get; set; }
    private string Sentiment { get; set; }
    private string Probability { get; set; }
    private string Accuracy { get; set; }
    private string AreaUnderRocCurve { get; set; }
    private string F1Score { get; set; }


    private void AnalyzeSentiment()
    {
        var trainingData = SentimentService.LoadData().TrainSet;
        var model = SentimentService.BuildAndTrainModel(trainingData);

        var resultPrediction = SentimentService.UseModelWithSingleEntity(model, SentimentText);
        Sentiment = resultPrediction.Prediction ? "Positive" : "Negative";
        Probability = $"{resultPrediction.Probability:P2}";

        var testData = SentimentService.LoadData().TestSet;
        var metrics = SentimentService.EvaluateModel(model, testData);
        Accuracy = $"Accuracy: {metrics.Accuracy:P2}";
        AreaUnderRocCurve = $"AreaUnderRocCurve: {metrics.AreaUnderRocCurve:P2}";
        F1Score = $"F1Score: {metrics.F1Score:P2}";
    }
}