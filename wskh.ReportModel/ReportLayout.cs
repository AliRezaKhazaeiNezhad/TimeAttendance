using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.ReportModel
{
    public class ReportLayout
    {
        public ReportLayout()
        {
            BasicInfos = new List<BasicInfo>();
        }

        public string ReportTitle { get; set; }
        public string SoftwareCompany { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PrintUser { get; set; }
        public string PrintDate { get; set; }
        public string CompanyCategory { get; set; }
        public string CompanyName { get; set; }
        public byte[] CompanyLogo { get; set; }


        public string TotalOne { get; set; }
        public string TotalTwo { get; set; }
        public string TotalThree { get; set; }
        public string TotalFour { get; set; }



        public List<BasicInfo> BasicInfos { get; set; }
    }
}
