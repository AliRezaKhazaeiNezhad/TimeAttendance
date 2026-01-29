using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class RemovedLogReport : BaseModel
    {
        public RemovedLogReport()
        {

        }


        public string LogDate { get; set; }
        public string LogTime { get; set; }
        public string EnrollNumber { get; set; }
        public string UserName { get; set; }
        public string Device { get; set; }
        public string UserRemove { get; set; }
    }
}
