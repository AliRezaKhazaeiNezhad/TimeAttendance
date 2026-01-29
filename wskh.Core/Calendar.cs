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
    [Table("Calendars")]
    public class Calendar : BaseEntity
    {
        #region Ctor
        public Calendar()
        {
            CalendarDays = new List<CalendarDay>();
            UserGroups = new List<UserGroup>();
            UserGroupCalendares = new List<UserGroupCalendare>();
            ReportDays = new List<ReportDay>();
        }
        #endregion


        #region Propertices
        [MaxLength(50)]
        public string Title { get; set; }
        public int Year { get; set; }
        public bool IsBigYear { get; set; }
        #endregion


        #region Relations
        public virtual List<CalendarDay> CalendarDays { get; set; }


        public int? SpecialDayGroupingId { get; set; }
        public virtual SpecialDayGrouping SpecialDayGroupings { get; set; }


        public int? ParentId { get; set; }
        public int? OriginalParentId { get; set; }


        public virtual List<UserGroup> UserGroups { get; set; }

        public virtual List<UserGroupCalendare> UserGroupCalendares { get; set; }


        public virtual List<Request> Requests { get; set; }


        public virtual List<ReportDay> ReportDays { get; set; }




        public int? RequestRuleId { get; set; }
        public virtual RequestRule RequestRule { get; set; }
        #endregion
    }
}
