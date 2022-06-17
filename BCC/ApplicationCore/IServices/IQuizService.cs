using QuizService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.IServices
{
    public interface IQuizService
    {
        QuizResponseModel Get(int id);
        int GetScore(int id, QuizAnswersModel value);
    }
}
