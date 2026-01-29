using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    [Table("CalendarDays")]
    public class CalendarDay : BaseEntity
    {
        #region Ctor
        public CalendarDay()
        {
            AnalyzedReports = new List<AnalyzedReport>();
        }
        #endregion

        #region Propertices
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }
        #endregion

        #region Relations
        public int CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }

        public int WorkProgramId { get; set; }
        public virtual WorkProgram WorkProgram { get; set; }

        public virtual List<AnalyzedReport> AnalyzedReports { get; set; }

        #endregion
    }
}
