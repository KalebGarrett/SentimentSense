using Microsoft.ML;
using static Microsoft.ML.DataOperationsCatalog;

namespace SentimentSense.App.Services.Interfaces;

public interface IMachineLearningService
{
    TrainTestData LoadData();
    ITransformer BuildAndTrainModel(IDataView splitTrainSet);
}