using ApplicationCore.IRepositories;
using ApplicationCore.IServices;
using QuizService.Domain.Entities;
using QuizService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class QuizService : IQuizService
    {
        public IQuestionRepository _questionRepository;
        public IAnswerRepository _answerRepository;
        public IQuizRepository _quizRepository;
        public QuizService(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IQuizRepository quizRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _quizRepository = quizRepository;
        }
        public QuizResponseModel Get(int id)
        {
           
            var quiz = _quizRepository.Get(id);
            if (quiz == null)
                return null; 
            var questions = _questionRepository.GetByQuizId(id);
            var answers = _answerRepository.GetByQuizId(id).GroupBy(x => x.QuestionId).ToDictionary(k => k.Key, v => v.Select(answer => new QuizResponseModel.AnswerItem
            { 
                Id = answer.Id,
                Text = answer.Text,
            }));
            return new QuizResponseModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Questions = questions.Select(question => new QuizResponseModel.QuestionItem
                {
                    Id = question.Id,
                    Text = question.Text,
                    Answers = answers.ContainsKey(question.Id) ? answers[question.Id] : new QuizResponseModel.AnswerItem[0],
                    CorrectAnswerId = question.CorrectAnswerId
                }),
                Links = new Dictionary<string, string>
            {
                {"self", $"/api/quizzes/{id}"},
                {"questions", $"/api/quizzes/{id}/questions"}
            }
            };
        }
        public int GetScore(int id, QuizAnswersModel value)
        {
            var quiz = Get(id);
            int score = 0;
            if (quiz == null)
                return -1;

            foreach (var question in quiz.Questions)
            {
                //const string sql = "SELECT CASE WHEN EXISTS ( SELECT * FROM Question WHERE Id = @QuestionId AND QuizId = @QuizId AND CorrectAnswerId = @AnswerId)";
                //if (_connection.Query<Quiz>(sql).Count() == 1) score++; ;
                if (value.Answers.Where(x => x.QuestionId == question.Id && x.AnswerId == question.CorrectAnswerId).Any())
                    score++;
            }
            return score;
        }

    }
}
