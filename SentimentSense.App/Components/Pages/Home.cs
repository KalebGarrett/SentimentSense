using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.ML;
using SentimentSense.App.Data.Datasets;
using SentimentSense.App.Services;

namespace SentimentSense.App.Components.Pages;

public partial class Home
{
    // Remove and register as a service
    private static MLContext MlContext { get; set; } = new MLContext(); 
    private SentimentService SentimentService { get; set; } = new SentimentService(MlContext);
    private string SentimentText { get; set; }
    private string Sentiment { get; set; }
    private string Probablity { get; set; }
    private string Accuracy { get; set; }
    private string AreaUnderRocCurve { get; set; }
    private string F1Score { get; set; }
    

    private void AnalyzeSentiment()
    {
        var trainingData = SentimentService.LoadData().TrainSet;
        var model =  SentimentService.BuildAndTrainModel(trainingData);
        
        var resultPrediction = SentimentService.UseModelWithSingleEntity(model, SentimentText);
        Sentiment = resultPrediction.Prediction ? "Positive" : "Negative";
        Probablity = resultPrediction.Probability.ToString(CultureInfo.InvariantCulture);
        
        var testData = SentimentService.LoadData().TestSet;
        var metrics = SentimentService.EvaluateModel(model, testData);
        Accuracy = $"Accuracy: {metrics.Accuracy:P2}";
        AreaUnderRocCurve = $"AreaUnderRocCurve: {metrics.AreaUnderRocCurve:P2}";
        F1Score = $"F1Score: {metrics.F1Score:P2}";
    }
}