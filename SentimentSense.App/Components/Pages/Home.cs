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

    private void AnalyzeSentiment()
    {
        var trainingData = SentimentService.LoadData().TrainSet;
        var model =  SentimentService.BuildAndTrainModel(trainingData);
        SentimentService.UseModelWithSingleEntity(model, SentimentText);
    }
}