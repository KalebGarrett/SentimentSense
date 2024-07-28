using Microsoft.ML;
using Microsoft.ML.Data;
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
        var pipeline = _mlContext.Transforms.Text
            .FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.SentimentText))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        var model = pipeline.Fit(splitTrainSet);
        return model;
    }

    public CalibratedBinaryClassificationMetrics EvaluateModel(ITransformer model, IDataView data)
    {
        var predictions = model.Transform(data);
        var metrics = _mlContext.BinaryClassification.Evaluate(predictions);
        return metrics;
    }

    public SentimentPrediction UseModelWithSingleEntity(ITransformer model, string sentimentText)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        var sentimentData = new SentimentData()
        {
            SentimentText = sentimentText
        };
        
        var resultPrediction = predictionEngine.Predict(sentimentData);
        return resultPrediction;
    }
}