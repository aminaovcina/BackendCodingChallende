using ApplicationCore.IRepositories;
using Dapper;
using QuizService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizService.Infrastructure.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IDbConnection _connection;
        public AnswerRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<Answer> GetByQuizId(int id)
        {
            const string answersSql = "SELECT a.Id, a.Text, a.QuestionId FROM Answer a INNER JOIN Question q ON a.QuestionId = q.Id WHERE q.QuizId = @QuizId;";
            var answers = _connection.Query<Answer>(answersSql, new { QuizId = id });
            return answers;
        }
    }
}
