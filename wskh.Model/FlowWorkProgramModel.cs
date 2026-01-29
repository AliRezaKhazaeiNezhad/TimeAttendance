using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class FlowWorkProgramModel : BaseModel
    {
        public FlowWorkProgramModel()
        {
            WorkTimeInDayHour = "0";
            WorkTimeInDayMinute = "0";
        }

        [MaxLength(50)]
        [Display(Name = "عنوان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }

        /// <summary>
        /// شناوری - موظفی در روز بر حسب HH:MM
        /// </summary>
        [MaxLength(2)]
        [Display(Name = "شناوری در روز"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string WorkTimeInDayHour { get; set; }
        [MaxLength(2)]
        [Display(Name = "شناوری در روز"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string WorkTimeInDayMinute { get; set; }
        /// شناوری - تردد در دو تاریخ متفاوت
        /// </summary>
        [Display(Name = "تردد در دو تاریخ؟")]
        public bool MoreThan24Houres { get; set; }

    }
}
