using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.ReportModel
{
    public class BasicInfo
    {
        public BasicInfo()
        {
            Trades = new List<Trade>();
            Contracts = new List<Contract>();
            DeviceAndEnrolls = new List<DeviceAndEnroll>();
        }

        public string GroupName { get; set; }
        public string NameAndFamily { get; set; }
        public string PersonalCode { get; set; }
        public string TotalTime { get; set; }
        public string Index { get; set; }
        public string Branch { get; set; }
        public string EducationLevel { get; set; }
        public string State { get; set; }


        public List<Trade> Trades { get; set; }
        public List<Contract> Contracts { get; set; }
        public List<DeviceAndEnroll> DeviceAndEnrolls { get; set; }
    }
}
