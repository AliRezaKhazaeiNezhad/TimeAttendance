using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class LeaveReportModel
    {
        public LeaveReportModel()
        {

        }

        public string UserId { get; set; }
        public int LeaveTypeId { get; set; }
        public int Year { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Date { get; set; }
    }

    public class DailyLeaveReportModel
    {
        public DailyLeaveReportModel()
        {

        }

        public string UserId { get; set; }
        public int LeaveTypeId { get; set; }
        public int Year { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
