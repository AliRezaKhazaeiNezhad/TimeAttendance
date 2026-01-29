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
    [Table("WorkProgramDays")]
    public class WorkProgramDay : BaseEntity
    {
        public WorkProgramDay()
        {
            WorkProgramTimes = new List<WorkProgramTime>();

            ReportDays = new List<ReportDay>();

            AnalyzedReports = new List<AnalyzedReport>();
        }
        #region Fields

        /// <summary>
        /// شماره روز است که از 1 شروع میشود و عدد 1 معادل با شنبه میباشد
        /// </summary>
        public int DayIndex { get; set; }



        /// <summary>
        /// ساعت کاری روز برحسب دقیقه
        /// </summary>
        public int TotalWorkTimeMinute { get; set; }
        /// <summary>
        /// ساعت استراحت روز برحسب دقیقه
        /// </summary>
        public int TotalRestTimeMinute { get; set; }
        /// <summary>
        /// ساعت اضافه کار اول وقت برحسب دقیقه
        /// </summary>
        public int TotalOverTimeStartMinute { get; set; }
        /// <summary>
        /// ساعت اضافه کار آخر وقت برحسب دقیقه
        /// </summary>
        public int TotalOverTimeEndMinute { get; set; }




        /// <summary>
        /// نوع روز
        /// </summary>
        public WorkType WorkType { get; set; }
   
        #endregion


        #region Relations
        public int WorkProgramId { get; set; }
        public virtual WorkProgram WorkProgram { get; set; }
        public virtual List<WorkProgramTime> WorkProgramTimes { get; set; }
        public virtual List<ReportDay> ReportDays { get; set; }
        public virtual List<AnalyzedReport> AnalyzedReports { get; set; }

        #endregion
    }
}
