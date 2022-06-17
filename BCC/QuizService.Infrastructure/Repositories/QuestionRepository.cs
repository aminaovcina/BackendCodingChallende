using ApplicationCore.IRepositories;
using QuizService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace QuizService.Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IDbConnection _connection;
        public QuestionRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<Question> GetByQuizId(int id)
        {
            const string questionsSql = "SELECT * FROM Question WHERE QuizId = @QuizId;";
            var questions = _connection.Query<Question>(questionsSql, new { QuizId = id });
            return questions;
        }
    }
}
