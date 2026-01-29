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
    public class Leave : BaseEntity
    {
        #region Ctor

        public Leave()
        {

        }

        #endregion


        #region Propertices

        /// <summary>
        /// تاریخ شروع مرخصی/ماموریت
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// تاریخ پایان مرخصی/ماموریت
        /// </summary>
        public DateTime EndDateTime { get; set; }


        /// <summary>
        /// تاریخ تایید درخواست
        /// </summary>
        public DateTime? ApprovedDateTime { get; set; }

        /// <summary>
        /// زمان شروع
        /// </summary>
        [MaxLength(5)]
        public string StartTime { get; set; }

        /// <summary>
        /// زمان پایان
        /// </summary>
        [MaxLength(5)]
        public string EndtTime { get; set; }

        /// <summary>
        /// کل زمان مرخصی/ماموریت برحسب دقیقه
        /// </summary>
        [MaxLength(10)]
        public string TotalMinutes { get; set; }

        /// <summary>
        /// شرح درخواست
        /// </summary>
        [MaxLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// نتیجه درخواست
        /// </summary>
        [MaxLength(300)]
        public string Reply { get; set; }


        /// <summary>
        /// وضعیت درخواست
        /// </summary>
        public LeaveState State { get; set; }


        /// <summary>
        /// نوع درخواست
        /// </summary>
        public LeaveType Type { get; set; }

        #endregion


        #region Relations

        /// <summary>
        /// کاربر درخواست دهنده
        /// </summary>
        public string RequestUserId { get; set; }
        [ForeignKey("RequestUserId")]
        public virtual wskhUser RequestUser { get; set; }


        /// <summary>
        /// کارمند جایگزین
        /// </summary>
        public string SecondUserId { get; set; }
        [ForeignKey("SecondUserId")]
        public virtual wskhUser SecondUser { get; set; }


        /// <summary>
        /// کارمند/مدیر تایید کننده
        /// </summary>
        public string ApproveUserId { get; set; }
        [ForeignKey("ApproveUserId")]
        public virtual wskhUser ApproveUser { get; set; }


        /// <summary>
        /// گزارش آنالیز شده
        /// </summary>
        public int? AnalyzedReportId { get; set; }
        public virtual AnalyzedReport AnalyzedReport { get; set; }

        #endregion
    }
}
