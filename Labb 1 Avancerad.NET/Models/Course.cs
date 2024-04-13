using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_1_Avancerad.NET.Models
{
    internal class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Student> Students { get; set; }

    }
}
