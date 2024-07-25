using Microsoft.ML;
using static Microsoft.ML.DataOperationsCatalog;

namespace SentimentSense.App.Services.Interfaces;

public interface IMachineLearningService
{
    TrainTestData LoadData(MLContext ctx);
    ITransformer BuildAndTrainModel(MLContext mlContext, IDataView splitTrainSet);
}