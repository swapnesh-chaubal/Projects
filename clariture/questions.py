import json
import sys
#import sys.path
#sys.path.append('/usr/bin/')
#import sqllite

class Question():
    def __init__(self, question):
        self.question = question
        self.answers = list()

    def append(self, answer):
        self.answers.append(answer)

    def remove_answer(self, answer):
        if answer in self.answers:
            self.answers.remove(answer)
    
class Answer():
    def __init__(self, answer):
        self.answer = answer
        self.count = 0

    def increment(self):
        self.count += 1

class QuestionAnswer():
    def __init__(self):
        self.questions = dict()
        q1 = Question("What is the capital of the US?")
        ans1 = list()
        ans1.append(Answer("Washington DC"))
        ans1.append(Answer("NYC"))
        ans1.append(Answer("SF"))
        self.questions[q1] = ans1

        q2 = Question("What's your favorite car?")
        ans2 = list()
        ans2.append(Answer("Tesla"))
        ans2.append(Answer("Mercedes"))
        ans2.append(Answer("BMW"))
        self.questions[q2] = ans2


        q3 = Question("What is the capital of CA?")
        ans3 = list()
        ans3.append(Answer("SF"))
        ans3.append(Answer("Sacramento"))
        ans3.append(Answer("LA"))
        self.questions[q3] = ans3

        q4 = Question("What are types of pizzas?")
        ans4 = list()
        ans4.append(Answer("Chicago Style"))
        ans4.append(Answer("New York"))
        ans4.append(Answer("Italian"))
        self.questions[q4] = ans4

        q4 = Question("What are types of cars?")
        ans4 = list()
        ans4.append(Answer("BMW"))
        ans4.append(Answer("Cycle"))
        ans4.append(Answer("Tesla"))
        self.questions[q4] = ans4


    def set_answer(self, question, ans):
        question = question.strip()
        if question.strip() in self.questions.keys():
            ans = self.questions[question]
            ans.increment()
   
    def get_random_question(self):
        import random
        num = random.randrange(len(self.questions))
        return self.get_json_QA(self.questions[num])    
       
    def get_json_pair(self, key, value):
        data = dict()
        data[key] = value
        return json.dumps(data)

    def get_stats(self):
        question_data = dict()
        for question in self.questions:
            question_data["question"] = question
            ans = list()
            for answer in self.questions[question]:
                data = dict()
                data["answer"] = answer.answer
                data["count"] = answer.count
                ans.append(json.dumps(data))

            question_data["answers"] = ans

        return json.dumps(question_data)
 
    def get_json_QA(self, question):
        if question not in self.questions:
            return None

        question = self.questions[question]
        return json.dumps(question)
