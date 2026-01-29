using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class PatchHistoryModel : BaseModel
    {
        public PatchHistoryModel()
        {

        }
        public string PtachName { get; set; }
        public string PatchCode { get; set; }
        public string Description { get; set; }
        public string CreateDateTimePersian { get; set; }
        public string FingerDeviceName { get; set; }
        public string LogCount { get; set; }
    }
}
