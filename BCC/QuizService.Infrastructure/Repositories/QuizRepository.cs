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
    public class QuizRepository : IQuizRepository
    {
        private readonly IDbConnection _connection;
        public QuizRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public Quiz Get(int id)
        {
            const string quizSql = "SELECT * FROM Quiz WHERE Id = @Id;";
            var quiz = _connection.QueryFirstOrDefault<Quiz>(quizSql, new { Id = id });
            return quiz;
        }

    }
}
