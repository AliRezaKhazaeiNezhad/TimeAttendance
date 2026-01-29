using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    /// <summary>
    /// این کلاس وظیفه ثبت تاریخچه ارتباطات با سخت افزار و نتیجه آن را برعهده دارد
    /// </summary>
    public class PatchHistory
    {
        public PatchHistory()
        {

        }
        [Key]
        public int Id { get; set; }
        [MaxLength(75)]
        public string PtachName { get; set; }
        [MaxLength(15)]
        public string PatchCode { get; set; }

        public int LastLogCount { get; set; }
        public int LogCount { get; set; }

        [MaxLength(75)]
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }


        public int FingerDeviceId { get; set; }
        public virtual FingerDevice FingerDevice { get; set; }

    }
}
