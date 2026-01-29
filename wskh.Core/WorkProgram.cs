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
    [Table("WorkPrograms")]
    public class WorkProgram : BaseEntity
    {
        #region Ctor
        public WorkProgram()
        {
            WorkProgramDays = new List<WorkProgramDay>();
            ReportDays = new List<ReportDay>();
        }
        #endregion
        #region Propertices
     
        public DateTime? CreateDateTime { get; set; }

        #region مشترک برای شناوری و هفتگی
        [MaxLength(50)]
        public string Title { get; set; }
        public WorkProgramType Type { get; set; }
        #endregion
        #region شناوری
        ///درصورتیکه برنامه شناوری بود این دو فیلد پر میشوند و در غیر اینصورت لیست های مربوط به این جدول پر خواهند شد
        /// <summary>
        /// شناوری - موظفی در روز بر حسب HH:MM
        /// درصورتیکه مقدار 00:00 باشد، یعنی شناور بدون موظفی
        /// </summary>
        [MaxLength(5)]
        public string WorkTimeInDay { get; set; }
        /// <summary>
        /// شناوری - تردد در دو تاریخ متفاوت
        /// </summary>
        public bool MoreThan24Houres { get; set; }
        #endregion

        #region کل زمان های برنامه
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
        #endregion
        #endregion
        #region Relations
        public virtual List<WorkProgramDay> WorkProgramDays { get; set; }
        public virtual List<ReportDay> ReportDays { get; set; }
        #endregion
    }
}
