using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class AddTradeModel
    {
        public AddTradeModel()
        {

        }

        public int? Id { get; set; }
        public int ReportDayId { get; set; }
        public string UserId { get; set; }

        public string EnteranceTime { get; set; }
        public string ExitTime { get; set; }

    }
}
