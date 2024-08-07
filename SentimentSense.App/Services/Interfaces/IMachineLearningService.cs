﻿using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;

namespace SentimentSense.App.Services.Interfaces;

public interface IMachineLearningService
{
    TrainTestData LoadData();
    ITransformer BuildAndTrainModel(IDataView splitTrainSet);
    CalibratedBinaryClassificationMetrics EvaluateModel(ITransformer model, IDataView data);
}