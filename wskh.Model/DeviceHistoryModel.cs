using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class DeviceHistoryModel : BaseModel
    {
        public DeviceHistoryModel()
        {

        }

        [Display(Name = "تعداد مدیر دستگاه")]
        public int AdminCount { get; set; }
        [Display(Name = "تعداد کاربر دستگاه")]
        public int UserCount { get; set; }
        [Display(Name = "تعداد اثر انگشت")]
        public int FingerPrintCount { get; set; }
        [Display(Name = "تعداد پسورد")]
        public int PasswordCount { get; set; }
        [Display(Name = "تعداد ورود و خروج")]
        public int LogCount { get; set; }

    }
}
