using Microsoft.AspNetCore.Components;
using SentimentSense.App.Services;

namespace SentimentSense.App.Components.Pages;

public partial class Home
{
    [Inject] private SentimentService SentimentService { get; set; }
    private string SentimentText { get; set; }
    private string Sentiment { get; set; }
    private string Probability { get; set; }
    private static string Accuracy { get; set; }
    private static string AreaUnderRocCurve { get; set; }
    private static string F1Score { get; set; }
    private int Index = -1;
    private int DataSize { get; set; } = 3;
    private double[] Data { get; set; } =
        {Convert.ToDouble(Accuracy), Convert.ToDouble(AreaUnderRocCurve), Convert.ToDouble(F1Score)};

    private string[] Labels { get; set; } = {"Accuracy", "AreaUnderRocCurve", "F1Score"};

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
        Console.WriteLine(Accuracy);
    }

    private void RandomizeData()
    {
        var random = new Random();
        var new_data = new double[DataSize];
        for (int i = 0; i < new_data.Length; i++)
            new_data[i] = random.NextDouble() * 100;
        Data = new_data;
        StateHasChanged();
    }
}