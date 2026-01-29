using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    public class SpecialDayGrouping : BaseEntity
    {
        #region Ctor
        public SpecialDayGrouping()
        {
            SpecialDays = new List<SpecialDay>();
            Calendars = new List<Calendar>();
        }
        #endregion


        #region Propertice
        [MaxLength(30)]
        public string Title { get; set; }
        #endregion


        #region Relations
        public virtual List<SpecialDay> SpecialDays { get; set; }
        public virtual List<Calendar> Calendars { get; set; }
        #endregion
    }
}
