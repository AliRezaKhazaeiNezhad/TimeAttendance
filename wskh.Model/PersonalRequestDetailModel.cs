using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class PersonalRequestDetailModel
    {
        public PersonalRequestDetailModel()
        {

        }

        public string Title { get; set; }
        public string RemainLastYear { get; set; }
        public string TotalThisYear { get; set; }
        public string RemainThisYear { get; set; }
        public string LeagalMonthly { get; set; }
    }
}
