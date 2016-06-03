from sklearn.naive_bayes import GaussianNB
from sklearn.neighbors import KNeighborsClassifier
from sklearn import tree


class KNN:
    def __init__(self, training, testing):
        self.testing_data = testing
        self.training_data = training

    def evaluate(self):
        model = KNeighborsClassifier(n_neighbors=2)
        model.fit(self.training_data.inputs, self.training_data.outputs)
        return model.score(self.testing_data.inputs, self.testing_data.outputs)


class NaiveBayes:
    def __init__(self, training, testing):
        self.testing_data = testing
        self.training_data = training

    def evaluate(self):
        model = GaussianNB()
        model.fit(self.training_data.inputs, self.training_data.outputs)
        return model.score(self.testing_data.inputs, self.testing_data.outputs)


class DecisionTree:
    def __init__(self, training, testing):
        self.testing_data = testing
        self.training_data = training

    def evaluate(self):
        model = tree.DecisionTreeClassifier()
        model.fit(self.training_data.inputs, self.training_data.outputs)
        return model.score(self.testing_data.inputs, self.testing_data.outputs)
