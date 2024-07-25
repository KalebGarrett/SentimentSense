using Microsoft.ML;
using SentimentSense.App.Data.Constants;
using SentimentSense.App.Data.Datasets;
using SentimentSense.App.Services.Interfaces;
using static Microsoft.ML.DataOperationsCatalog;

namespace SentimentSense.App.Services;

public class SentimentService : IMachineLearningService
{
    private readonly MLContext _mlContext;

    public SentimentService(MLContext context)
    {
        _mlContext = context;
    }

    public TrainTestData LoadData()
    {
        var dataView =
            _mlContext.Data.LoadFromTextFile<SentimentData>
            (
                FilePath.SentimentDataCsv,
                hasHeader: true,
                separatorChar: ','
            );

        var splitDataView = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        return splitDataView;
    }

    public ITransformer BuildAndTrainModel(IDataView splitTrainSet)
    {
        var pipeline = _mlContext.Transforms.Concatenate(outputColumnName: "Features",
                inputColumnNames: nameof(SentimentData.SentimentText))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        var model = pipeline.Fit(splitTrainSet);
        return model;
    }

    public void UseModelWithSingleEntity(ITransformer model, string sentimentText)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        var sentimentData = new SentimentData()
        {
            SentimentText = sentimentText
        };
        var resultPrediction = predictionEngine.Predict(sentimentData);

        Console.WriteLine
        (resultPrediction.Prediction
            ? $"Sentiment: {sentimentData.SentimentText} | Prediction: Positive"
            : $"Sentiment: {sentimentData.SentimentText} | Prediction: Negative"
        );
    }
}