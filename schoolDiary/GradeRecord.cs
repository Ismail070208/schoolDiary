using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schoolDiary
{
    [Serializable]
    public class GradeRecord
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public double Grade { get; set; }
        public string TeacherName { get; set; }
    }
}
