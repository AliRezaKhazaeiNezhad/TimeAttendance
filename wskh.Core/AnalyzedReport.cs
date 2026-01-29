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
    public class AnalyzedReport
    {
        #region Ctor

        public AnalyzedReport()
        {
            AnalyzedReportLogs = new List<AnalyzedReportLog>();

            Leaves = new List<Leave>();
        }

        #endregion


        #region Propertices

        [Key]
        public int Id { get; set; }

        public bool Remove { get; set; }



        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// تاریخ گزارش
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// تاریخ شمسی گزارش
        /// </summary>
        [MaxLength(10)]
        public string PersianDate { get; set; }


        /// <summary>
        /// نام روز 
        /// </summary>
        [MaxLength(20)]
        public string DayName { get; set; }

        /// <summary>
        /// نام شمسی روز 
        /// </summary>
        [MaxLength(20)]
        public string PersianDayName { get; set; }

        /// <summary>
        /// ایندکس روز
        /// </summary>
        public int DayIndex { get; set; }





        /// <summary>
        /// کل کارکرد یا همان موظفی
        /// </summary>
        [MaxLength(20)]
        public string TotalWorkTime { get; set; }


        /// <summary>
        /// کل کارکرد یا همان زمان حضور
        /// </summary>
        [MaxLength(20)]
        public string RealTotalWorkTime { get; set; }


        /// <summary>
        /// کل زمان عدم حضور
        /// </summary>
        [MaxLength(20)]
        public string RealTotaloffTime { get; set; }


        /// <summary>
        /// تعجیل ورود
        /// </summary>
        [MaxLength(20)]
        public string EarlyEnteranceTime { get; set; }

        /// <summary>
        /// تاخیر خروج
        /// </summary>
        [MaxLength(20)]
        public string DelayExitTime { get; set; }

        /// <summary>
        /// تعطیلی؟
        /// </summary>
        public bool IsHoliday { get; set; }

        /// <summary>
        /// روز خاص؟
        /// </summary>
        public bool IsSpecialDay { get; set; }

        /// <summary>
        /// مرخصی روزانه
        /// </summary>
        [MaxLength(20)]
        public string DailyLeave { get; set; }

        /// <summary>
        /// مرخصی ساعتی
        /// </summary>
        [MaxLength(20)]
        public string HourlyLeave { get; set; }


        /// <summary>
        /// ماموریت روزانه
        /// </summary>
        [MaxLength(20)]
        public string DailyMission { get; set; }

        /// <summary>
        /// ماموریت ساعتی
        /// </summary>
        [MaxLength(20)]
        public string HourlyMission { get; set; }


        /// <summary>
        /// تاخیر ورود
        /// </summary>
        [MaxLength(20)]
        public string DelayEnterTime { get; set; }

        /// <summary>
        /// تعجیل خروج
        /// </summary>
        [MaxLength(20)]
        public string EarlyExit { get; set; }

        /// <summary>
        /// غیبت؟
        /// </summary>
        public bool Absence { get; set; }

        /// <summary>
        /// تاخیر سرویس
        /// </summary>
        [MaxLength(20)]
        public string BusDelay { get; set; }


        /// <summary>
        /// وضعیت
        /// </summary>
        public AnalyzedReportState State { get; set; }

        /// <summary>
        /// نوع
        /// </summary>
        public AnalyzedReportType Type { get; set; }

        /// <summary>
        /// تاریخ گزارش
        /// </summary>
        public DateTime ReportDate { get; set; }



        /// <summary>
        /// اضافه کار اول وقت
        /// </summary>
        [MaxLength(20)]
        public string TotalEarlyEnterance { get; set; }

        /// <summary>
        /// اضافه کار آخر وقت
        /// </summary>
        [MaxLength(20)]
        public string TotalDelayExit { get; set; }

        #endregion


        #region Relations

        /// <summary>
        /// کاربر
        /// </summary>
        public string UserId { get; set; }
        public virtual wskhUser User { get; set; }


        /// <summary>
        /// روزهای تقویم
        /// </summary>
        public int CalendarDayId { get; set; }
        public virtual CalendarDay CalendarDay { get; set; }

        public int? WorkProgramDayId { get; set; }
        public virtual WorkProgramDay WorkProgramDay { get; set; }

        public virtual List<AnalyzedReportLog> AnalyzedReportLogs { get; set; }


        public int? HolidayId { get; set; }
        [ForeignKey("HolidayId")]
        public virtual SpecialDay Holiday { get; set; }

        public int? SpecialDayId { get; set; }
        [ForeignKey("SpecialDayId")]
        public virtual SpecialDay SpecialDay { get; set; }


        public virtual List<Leave> Leaves { get; set; }

        #endregion
    }
}
