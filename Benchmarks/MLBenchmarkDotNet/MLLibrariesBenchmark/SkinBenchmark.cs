using BenchmarkDotNet.Attributes;
using System.Linq;
using MLLibrariesBenchmark.AccordBenchmark;
using MLLibrariesBenchmark.NumlBenchmark;

namespace MLLibrariesBenchmark
{
    public class SkinBenchmark
    {
        private AccordClassificator _accordClassificator;
        private NumlClassificator<Skin, string> _numlClassificator;

        [Setup]
        public void Init()
        {
            var testingData = new ClassificationData();
            var trainingData = new ClassificationData();
            testingData.OpenAndParseFile("skinTesting", true);
            testingData.ProcessDataset("skin");
            trainingData.OpenAndParseFile("skinTraining", true);
            trainingData.ProcessDataset("skin");

            _accordClassificator = new AccordClassificator(trainingData, testingData);
            _numlClassificator = new NumlClassificator<Skin, string>(
                Skin.LoadData("skinTraining").ToList(),
                Skin.LoadData("skinTesting").ToList(),
                model => model.Label
            );
        }

        [Benchmark]
        public double Numl_NaiveBayes()
        {
            return _numlClassificator.NaiveBayesTest();
        }

        //[Benchmark]
        // Removed, becouse this **** works 3,5 hours
        public double Numl_KNN()
        {
            return _numlClassificator.KNNTest();
        }

        [Benchmark]
        public double Numl_DecisionTree()
        {
            return _numlClassificator.DecisionTreeTest();
        }

        [Benchmark]
        public double Accord_NaiveBayes()
        {
            return _accordClassificator.NaiveBayesTest();
        }

        [Benchmark]
        public double Accord_KNN()
        {
            return _accordClassificator.KNNTest();
        }

        [Benchmark]
        public double Accord_DecisionTree()
        {
            return _accordClassificator.DecisionTreeTest();
        }
    }
}
