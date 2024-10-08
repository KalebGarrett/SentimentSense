﻿using System.Net;
using Amazon.S3;
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

    public TrainTestData LoadFromTextFile()
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

    public TrainTestData LoadFromEnumerable()
    {
        throw new NotImplementedException();
    }

    public ITransformer BuildOrLoadModelLocally(IDataView splitTrainSet)
    {
        ITransformer model;

        if (!File.Exists("MlModels/sentimentsensemodel.zip"))
        {
            var pipeline = _mlContext.Transforms.Text
                .FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.SentimentText))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

            model = pipeline.Fit(splitTrainSet);
            _mlContext.Model.Save(model, splitTrainSet.Schema,
                "MlModels/sentimentsensemodel.zip");
            return model;
        }

        model = _mlContext.Model.Load
        (
            "MlModels/sentimentsensemodel.zip",
            out _
        );
        return model;
    }

    public async Task<ITransformer> LoadModelRemotely()
    {
        var accessKey = Environment.GetEnvironmentVariable("AccessKey");
        var secretKey = Environment.GetEnvironmentVariable("SecretKey");
        
        var config = new AmazonS3Config
        {
            ServiceURL = "https://nyc3.digitaloceanspaces.com",
            UseHttp = true
        };

        var client = new AmazonS3Client
        (
            accessKey,
            secretKey,
            config
        );

        var objectResponse = await client.GetObjectAsync("sentimentsense-model", "sentimentsensemodel.zip");
        
        ITransformer model;
        
        if (objectResponse.HttpStatusCode != HttpStatusCode.OK)
        {
            model = _mlContext.Model.Load
            (
                "MlModels/sentimentsensemodel.zip",
                out _
            );

            return model;
        }
        
        var responseStream = objectResponse.ResponseStream;
        model = _mlContext.Model.Load(responseStream, out _);
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