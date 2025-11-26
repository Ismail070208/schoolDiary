using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static schoolDiary.AttendanceForm;
using static schoolDiary.GradesForm;

namespace schoolDiary
{
    [Serializable]
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }

        public List<AttendanceRecord> Attendance { get; set; } = new List<AttendanceRecord>();
        public List<GradeRecord> Grades { get; set; } = new List<GradeRecord>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
