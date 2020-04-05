using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zestaw_4.Models;

namespace Zestaw_4.Services
{
    public interface IStudentsDal
    {
        public IEnumerable<Student> GetStudents();
    }
}
