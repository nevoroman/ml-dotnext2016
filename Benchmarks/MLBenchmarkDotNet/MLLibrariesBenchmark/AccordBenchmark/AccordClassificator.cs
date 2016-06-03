using System.Collections.Generic;
using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Univariate;

namespace MLLibrariesBenchmark.AccordBenchmark
{
    public class AccordClassificator
    {
        private readonly ClassificationData _testingData;
        private readonly ClassificationData _trainingData;

        public AccordClassificator(ClassificationData trainingData, ClassificationData testingData)
        {
            _trainingData = trainingData;
            _testingData = testingData;
        }

        public double DecisionTreeTest()
        {
            var attributes = DecisionVariable.FromCodebook(_trainingData.CodeBook, _trainingData.InputColumnNames.ToArray());

            var classificationDecisionTree = new DecisionTree(attributes, _trainingData.OutputPossibleValues);
            new C45Learning(classificationDecisionTree).Run(_trainingData.InputData, _trainingData.OutputData);

            var testingDataCount = _testingData.InputData.Length;
            double error = 0;
            for (var i = 0; i < testingDataCount; i++)
            {
                var input = _testingData.InputData[i];
                var result = classificationDecisionTree.Compute(input);
                if (result != _testingData.OutputData[i]) error++;
            }
            return error / testingDataCount;
        }

        public double NaiveBayesTest()
        {
            // Create a new Naive Bayes classifier.
            var bayesianModel = new NaiveBayes<NormalDistribution>(
                _trainingData.OutputPossibleValues,
                _trainingData.InputAttributeNumber,
                NormalDistribution.Standard);

            bayesianModel.Estimate(
                _trainingData.InputData,
                _trainingData.OutputData,
                true,
                new NormalOptions { Regularization = 1e-5 /* To avoid zero variances. */ });

            return bayesianModel.Error(_testingData.InputData, _testingData.OutputData);
        }

        public double KNNTest()
        {
            var knn = new KNearestNeighbors(k: 2, classes: _trainingData.OutputPossibleValues, inputs: _trainingData.InputData, outputs: _trainingData.OutputData);

            var testingDataCount = _testingData.InputData.Length;
            double error = 0;
            for (var i = 0; i < testingDataCount; i++)
            {
                var input = _testingData.InputData[i];
                var result = knn.Compute(input);
                if (result != _testingData.OutputData[i]) error++;
            }
            return error / testingDataCount;
        }
    }
}
