using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schoolDiary
{
    [Serializable]
    public class AttendanceRecord
    {
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string Note { get; set; }
    }
}
