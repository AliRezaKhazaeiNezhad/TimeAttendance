using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class WorkProgramTimeModel : BaseModel
    {
        public WorkProgramTimeModel()
        {
            DurationInMinute = 0;
            WorkTimeInMinute = 0;
            WorkType = 0;
        }

        /// <summary>
        /// ساعت شروع با فرمت HH:MM
        /// </summary>
        [MaxLength(5)]
        [Display(Name = "زمان شروع")]
        public string StartTime { get; set; }

        /// <summary>
        /// ساعت پایان با فرمت HH:MM
        /// </summary>
        [MaxLength(5)]
        [Display(Name = "زمان پایان")]
        public string EndTime { get; set; }

        /// <summary>
        /// تردد در دو تاریخ
        /// </summary>
        [Display(Name = "تردد در دو تاریخ")]
        public bool TwoDays { get; set; }

        /// <summary>
        /// درصورتیکه صحیح داشته باشد یعنی پرسنل برای زمان نماز و استراحت ورود و خروج نمیزنند
        /// </summary>
        [Display(Name = "پرسنل برای زمان استراحت ورود و خروج نمیزنند؟")]
        public bool CalculaeRest { get; set; }


        /// <summary>
        /// بازه بین شروع تا پایان برحسب دقیقه
        /// </summary>
        public int DurationInMinute { get; set; }

        /// <summary>
        /// بازه بین شروع تا پایان برحسب HH:MM
        /// </summary>
        [MaxLength(5)]
        public string Duration { get; set; }


        /// <summary>
        /// بازه کارکرد برحسب دقیقه
        /// </summary>
        public int WorkTimeInMinute { get; set; }
        /// <summary>
        /// بازه کارکرد برحسب HH:MM
        /// </summary>
        [MaxLength(5)]
        public string WorkTime { get; set; }


        public int WorkType { get; set; }

        #region Relations
        public int WorkProgramDayModelId { get; set; }
        #endregion
    }
}
