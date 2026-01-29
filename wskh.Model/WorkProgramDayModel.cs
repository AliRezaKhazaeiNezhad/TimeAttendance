using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class WorkProgramDayModel : BaseModel
    {
        #region Ctor
        public WorkProgramDayModel()
        {
            WorkProgramId = 0;
            DayIndex = 0;
            WorkType = 0;
            WorkProgramTimeModels = new List<WorkProgramTimeModel>();
        }
        #endregion

        #region Fields
        public int DayIndex { get; set; }
        public int WorkType { get; set; }
        #endregion
        #region Relations
        public int WorkProgramId { get; set; }
        public virtual List<WorkProgramTimeModel> WorkProgramTimeModels { get; set; }
        #endregion
    }
}
