using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class CalendarDayModel : BaseModel
    {
        #region Ctor
        public CalendarDayModel()
        {

        }
        #endregion
        #region Propertices
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime StartDateGeo { get; set; }
        public DateTime EndDateGeo { get; set; }
        public int WorkProgramId { get; set; }

        #endregion
    }
}
