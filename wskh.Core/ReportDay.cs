using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Core.Enumerator;

namespace TimeAttendance.Core
{
    [Table("ReportDays")]
    public class ReportDay : BaseEntity
    {
        #region Ctor
        public ReportDay()
        {
            Logs = new List<Log>();
        }
        #endregion


        #region Fileds

        #region اطلاعات سال، ماه، روز به فرمت فارسی و شمسی
        /// <summary>
        /// سال به فرمت شمسی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// ماه به فرمت شمسی
        /// </summary>
        public int PersianMonth { get; set; }

        /// <summary>
        /// روز به فرمت شمسی
        /// </summary>
        public int PersianDay { get; set; }

        /// <summary>
        /// تاریخ شمسی
        /// </summary>
        [MaxLength(12)]
        public string PersianDate { get; set; }

        /// <summary>
        /// روز به فارسی
        /// </summary>
        [MaxLength(30)]
        public string PersianDayName { get; set; }
        #endregion

        #region اطلاعات کلی گزارش
        /// <summary>
        /// تاریخ
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// روز هفته
        /// </summary>
        public DayInWeek DayInWeek { get; set; }


        /// <summary>
        /// وضعیت گزارش
        /// </summary>
        public ReportState State { get; set; }

        /// <summary>
        /// نوع روز
        /// </summary>
        public WorkType WorkType { get; set; }

        /// <summary>
        /// نوع برنامه
        /// </summary>
        public WorkProgramType WorkProgramType { get; set; }

        #endregion





        #region اطلاعات تردد

        /// <summary>
        /// زمان حضور براسا برنامه کاری
        /// </summary>
        [MaxLength(10)]
        public string WorkTime { get; set; }

        /// <summary>
        /// کل زمان حضور
        /// </summary>
        [MaxLength(10)]
        public string TotalWorkTime { get; set; }

        /// <summary>
        /// کل زمان مرخصی روزانه
        /// </summary>
        [MaxLength(10)]
        public string TotalDailyAbsenceMinute { get; set; }

        /// <summary>
        /// کل زمان ماموریت روزانه
        /// </summary>
        [MaxLength(10)]
        public string TotalDailyMissionMinute { get; set; }

        /// <summary>
        /// کل زمان مرخصی ساعتی
        /// </summary>
        [MaxLength(10)]
        public string TotalHourlyAbsenceMinute { get; set; }

        /// <summary>
        /// کل زمان ماموریت ساعتی
        /// </summary>
        [MaxLength(10)]
        public string TotalHourlyMissionMinute { get; set; }

        /// <summary>
        /// کل زمان عدم حضور
        /// </summary>
        [MaxLength(10)]
        public string TotalAbsenceTime { get; set; }

        /// <summary>
        /// اضافه کار اول وقت (دقیقه)
        /// </summary>
        [MaxLength(10)]
        public string StartOverTimeMinute { get; set; }

        /// <summary>
        /// اضافه کار آخر وقت (دقیقه)
        /// </summary>
        [MaxLength(10)]
        public string EndOverTimeMinute { get; set; }

        /// <summary>
        /// اضافه کار روز تعطیل
        /// </summary>
        [MaxLength(10)]
        public string ExtraTimeInHolidayMinute { get; set; }

        /// <summary>
        /// تاخیر ورود (دقیقه)
        /// </summary>
        [MaxLength(10)]
        public string DelayEnteranceMinute { get; set; }

        /// <summary>
        /// تعجیل خروج (دقیقه)
        /// </summary>
        [MaxLength(10)]
        public string DelayExitMinute { get; set; }





        /// <summary>
        /// عدم حضور
        /// </summary>
        [MaxLength(5)]
        public string AbsenceTimeMinute { get; set; }


        /// <summary>
        /// تاخیر سرویس
        /// </summary>
        [MaxLength(5)]
        public string ServiceDelayMin { get; set; }


        /// <summary>
        /// نوع تردد
        /// </summary>
        public TradeType TradeType { get; set; }

        /// <summary>
        /// گام برنامه
        /// </summary>
        public ReportDayStep Step { get; set; }


        /// <summary>
        /// شرح
        /// </summary>
        public string Description { get; set; }

        #endregion

        #endregion



        #region Relations

        /// <summary>
        /// Relation to logs
        /// </summary>
        [JsonIgnore]
        public virtual List<Log> Logs { get; set; }



        /// <summary>
        /// کاربر موردنظر
        /// </summary>
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual wskhUser User { get; set; }


        /// <summary>
        /// تقویم کاری
        /// </summary>
        public int? CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }


        /// <summary>
        /// تعطیلی
        /// </summary>
        public int? SpecialDayId { get; set; }
        public virtual SpecialDay SpecialDay { get; set; }


        /// <summary>
        /// برنامه
        /// </summary>
        public int? WorkProgramId { get; set; }
        public virtual WorkProgram WorkProgram { get; set; }


        /// <summary>
        /// روز
        /// </summary>
        public int? WorkProgramDayId { get; set; }
        public virtual WorkProgramDay WorkProgramDay { get; set; }
        #endregion
    }
}
