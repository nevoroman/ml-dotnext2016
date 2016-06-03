class Data:
    def __init__(self, path):
        self.inputs = []
        self.outputs = []

        file_lines = open(path).readlines()

        for line in file_lines[1:]:
            line_data = line.split(",")
            input_nums = []
            for inputString in line_data[:-1]:
                input_nums.append(float(inputString))
            self.inputs.append(input_nums)
            self.outputs.append((line_data[-1]))