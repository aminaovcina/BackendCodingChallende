using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.IServices
{
    public interface IQuizService
    {
        dynamic Get(int id);
    }
}
