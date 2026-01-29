using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class TradeReportModel : BaseModel
    {
        public TradeReportModel()
        {

        }

        public string GroupName { get; set; }
        public string NameAndFamily { get; set; }
        public string DayName { get; set; }
        public string PersianDaet { get; set; }
        public string Logs { get; set; }
    }
}
