using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using numl.Supervised.KNN;
using numl.Supervised.NaiveBayes;
using System;
using System.Collections.Generic;

namespace MLLibrariesBenchmark.NumlBenchmark
{
    public class NumlClassificator<TBase, TLabel> where TBase : class
    {
        private readonly List<TBase> _trainingData;
        private readonly List<TBase> _testingData;
        private readonly Descriptor _description = Descriptor.Create<TBase>();
        private readonly Func<TBase, TLabel> _labelFunction;

        public NumlClassificator(List<TBase> trainingData, List<TBase> testingData, Func<TBase, TLabel> labelFunction)
        {
            _trainingData = trainingData;
            _testingData = testingData;
            _labelFunction = labelFunction;
        }

        public double DecisionTreeTest()
        {
            var generator = new DecisionTreeGenerator();
            var model = generator.Generate(_description, _trainingData);
            return Estimate(model);
        }

        public double KNNTest()
        {
            var generator = new KNNGenerator(2);
            var model = generator.Generate(_description, _trainingData);
            return Estimate(model);
        }

        public double NaiveBayesTest()
        {
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(_description, _trainingData);
            return Estimate(model);
        }

        private double Estimate(IModel model)
        {
            double error = 0;
            foreach (var data in _testingData)
            {
                var trueLabel = _labelFunction(data);
                var predictedLabel = _labelFunction(model.Predict(data));
                if (trueLabel.Equals(predictedLabel)) error++;
            }
            return error / _testingData.Count;
        }
    }
}
