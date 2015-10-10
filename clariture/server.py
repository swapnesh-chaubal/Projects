import tornado.ioloop
import tornado.web
import random
from questions import QuestionAnswer


PORT = 9000
class QuestionAnswerParser():
    def __init__(self):
        self.question_ans = QuestionAnswer()

    def get_question(self):
        return self.question_ans.get_random_question()

    def set_answer(self, question, answer):
       self.question_ans.set_answer(question, answer)

    def get_stats(self):
        return self.question_ans.get_stats()

parser = QuestionAnswerParser()

class GetQuestionHandler(tornado.web.RequestHandler):
    def get(self):
        question = parser.get_question()
        self.set_status(200)
        self.set_header("Content-type", "application/json")
        if question is not None:
            self.write(question)

class GetStatsHandler(tornado.web.RequestHandler):
    def get(self):
        question = parser.get_stats()
        self.set_status(200)
        self.set_header("Content-type", "application/json")
        if question is not None:
            self.write(question)


class SetAnswerHandler(tornado.web.RequestHandler):
    def post(self):
        data = self.request.body.decode('utf-8')
        self.parse_query(data)

    def parse_query(self, data):
        import json
        data = json.loads(data)
        if "answer" in data and "question" in data:
             parser.set_answer(data["question"], data["answer"])
        self.set_status(200)
 

application = tornado.web.Application([
    (r"/getquestion", GetQuestionHandler),
    (r"/setanswer", SetAnswerHandler),
    (r"/getstats", GetStatsHandler),
])


if __name__ == "__main__":
    print("Running Question Answer server at port " + str(PORT))
    application.listen(8888)
    tornado.ioloop.IOLoop.current().start()

