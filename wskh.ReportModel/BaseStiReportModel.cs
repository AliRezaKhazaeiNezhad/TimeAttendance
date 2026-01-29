using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.ReportModel
{
    public class BaseStiReportModel
    {
        public BaseStiReportModel()
        {

        }

        public string CompanyName { get; set; }
        public string CompanyCategory { get; set; }
        public string ReportTitle { get; set; }
        public string SoftwareCompany { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PrintUser { get; set; }
        public string PrintDate { get; set; }
    }
}
