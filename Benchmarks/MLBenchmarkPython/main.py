from models import KNN, NaiveBayes, DecisionTree
from data import Data

training_data = Data('skinTraining')
testing_data = Data('skinTesting')

knn = KNN(training_data, testing_data)
bayes = NaiveBayes(training_data, testing_data)
tree = DecisionTree(training_data, testing_data)

knnResult = knn.evaluate()
bayesResult = bayes.evaluate()
treeResult = tree.evaluate()

print knnResult, treeResult, bayesResult
