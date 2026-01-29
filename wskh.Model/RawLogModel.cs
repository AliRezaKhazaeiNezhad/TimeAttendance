using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class RawLogModel
    {
        public RawLogModel()
        {

        }

        public int? Id { get; set; }
        public int? Index { get; set; }
        public string VerifyMode { get; set; }
        public string InOutMode { get; set; }
        public string WorkCode { get; set; }
        public bool Orgin { get; set; }
        public string LogDate { get; set; }
        public string LogTime { get; set; }
        public int EnrollNo { get; set; }
        public string DayOfWeek { get; set; }
        public string DeviceName { get; set; }
        public string EnrollNoFull { get; set; }
    }
}
