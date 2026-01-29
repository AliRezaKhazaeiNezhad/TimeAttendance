using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class InstantReportModel : BaseModel
    {
        public InstantReportModel()
        {

        }

        public string GroupName { get; set; }
        public string NameAndFamily { get; set; }
        public string EnteranceTime { get; set; }
        public string ExitTime { get; set; }
        public string Device { get; set; }
        public string StateString { get; set; }
        public string Button1 { get; set; }
        public string DayName { get; set; }
        public string PersianDaet { get; set; }
        public string Logs { get; set; }
    }
}
