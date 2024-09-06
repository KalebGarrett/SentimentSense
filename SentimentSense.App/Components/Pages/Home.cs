using Microsoft.AspNetCore.Components;
using MudBlazor;
using SentimentSense.App.Services;

namespace SentimentSense.App.Components.Pages;

public partial class Home
{
    [Inject] private SentimentService SentimentService { get; set; }
    private string SentimentText { get; set; }
    private string Sentiment { get; set; }
    private double Probability { get; set; }
    private static double Accuracy { get; set; }
    private static double AreaUnderRocCurve { get; set; }
    private static double F1Score { get; set; }
    private int Index { get; set; } = -1;
    private double[] Data { get; set; } 
    private string[] Labels { get; set; } = {"Accuracy", "AreaUnderRocCurve", "F1Score"};
    private Position LegendPosition { get; set; } = Position.Bottom;
    private Color FullColor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Accuracy = 0;
        AreaUnderRocCurve = 0;
        F1Score = 0;
    }
    
    private async Task AnalyzeText()
    {
        var loadModelRemotely = await SentimentService.LoadModelRemotely();

        var resultPrediction = SentimentService.UseModelWithSingleEntity(loadModelRemotely, SentimentText);
        Sentiment = resultPrediction.Prediction ? "Positive" : "Negative";
        Probability = resultPrediction.Probability * 100;
        FullColor = resultPrediction.Prediction ? Color.Tertiary : Color.Secondary;

        var testData = SentimentService.LoadFromTextFile().TestSet;
        var metrics = SentimentService.EvaluateModel(loadModelRemotely, testData);
        Accuracy = metrics.Accuracy * 100;
        AreaUnderRocCurve = metrics.AreaUnderRocCurve * 100;
        F1Score = metrics.F1Score * 100;
        
        Data = new[] {Convert.ToDouble(Accuracy), Convert.ToDouble(AreaUnderRocCurve), Convert.ToDouble(F1Score)};
    }
    
    private void OnSelectedValue(Position value)
    {
        switch(value)
        {
            case Position.Top:
                LegendPosition = Position.Top;
                break;
            case Position.Left:
                LegendPosition = Position.Left;
                break;
            case Position.Right:
                LegendPosition = Position.Right;
                break;
            case Position.Bottom:
                LegendPosition = Position.Bottom;
                break;
            case Position.Start:
                LegendPosition = Position.Start;
                break;
            case Position.End:
                LegendPosition = Position.End;
                break;
        }
    }
}