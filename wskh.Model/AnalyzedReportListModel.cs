
using System.Collections.Generic;
using TimeAttendance.Core;

namespace TimeAttendance.Model
{
    public class AnalyzedReportListModel
    {
        public AnalyzedReportListModel()
        {
            Reports = new List<AnalyzedReport>();
        }

        public string UserGroup { get; set; }
        public string UserInformattion { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<AnalyzedReport> Reports { get; set; }
    }
}
