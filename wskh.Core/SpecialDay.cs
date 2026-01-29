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
    [Table("SpecialDays")]
    public class SpecialDay : BaseEntity
    {
        #region Ctor
        public SpecialDay()
        {
            SpecialDayGroupings = new List<SpecialDayGrouping>();
            HoliDays = new List<AnalyzedReport>();
            SpecialDays = new List<AnalyzedReport>();
        }
        #endregion
        #region Propertices

        /// <summary>
        /// عنوان تعطیلی، روز خاص یا ایام خاص
        /// </summary>
        [MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// تاریخ یا تاریخ شروع
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// تاخیر ورود
        /// </summary>
        [MaxLength(3)]
        public string DelayEnterance { get; set; }

        /// <summary>
        /// تاخیر خروج
        /// </summary>
        [MaxLength(3)]
        public string DelayExit { get; set; }


        /// <summary>
        /// نوع روز تعطیلی
        /// </summary>
        public SpecialDayType Type { get; set; }

        #endregion
        #region Relations

        /// <summary>
        /// گروه
        /// </summary>
        public int? SpecialDayGroupingId { get; set; }
        public virtual SpecialDayGrouping SpecialDayGrouping { get; set; }



        public virtual List<SpecialDayGrouping> SpecialDayGroupings { get; set; }


        [InverseProperty("Holiday")]
        public virtual List<AnalyzedReport> HoliDays { get; set; }
        [InverseProperty("SpecialDay")]
        public virtual List<AnalyzedReport> SpecialDays { get; set; }

        #endregion
    }
}
