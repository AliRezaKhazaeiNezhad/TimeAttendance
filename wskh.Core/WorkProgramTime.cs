using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core.Enumerator;

namespace TimeAttendance.Core
{
    [Table("WorkProgramTimes")]
    public class WorkProgramTime
    {
        public WorkProgramTime()
        {

        }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ساعت شروع با فرمت HH:MM
        /// </summary>
        [MaxLength(5)]
        public string StartTime { get; set; }

        /// <summary>
        /// ساعت پایان با فرمت HH:MM
        /// </summary>
        [MaxLength(5)]
        public string EndTime { get; set; }




        /// <summary>
        /// بازه بین شروع تا پایان برحسب HH:MM
        /// </summary>
        [MaxLength(5)]
        public string Duration { get; set; }

        public TimeType TimeType { get; set; }

        #region Relations
        public int WorkProgramDayId { get; set; }
        public virtual WorkProgramDay WorkProgramDay { get; set; }
        #endregion
    }
}
