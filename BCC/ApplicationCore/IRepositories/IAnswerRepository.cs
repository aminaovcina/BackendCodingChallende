﻿using QuizService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.IRepositories
{
    public interface IAnswerRepository
    {
        IEnumerable<Answer> GetByQuizId(int id);
    }
}
