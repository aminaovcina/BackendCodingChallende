using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizService.Domain.Models
{
    public class QuizAnswersModel
    {
        public IEnumerable<AnswerItem> Answers { get; set; }
    }
    public class AnswerItem
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
