using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Core.Enumerator;

namespace TimeAttendance.Model
{
    public class OrdinaryWorkProgramDayModel
    {
        public OrdinaryWorkProgramDayModel()
        {
           
            OrdinaryWorkProgramTimeModels = new List<OrdinaryWorkProgramTimeModel>();
            DayIndex = 1;
            WorkType = WorkType.WorkDay;
            WorkTypeId = 0;
        }

      
        public int DayIndex { get; set; }
        public WorkType WorkType { get; set; }
        public int WorkTypeId { get; set; }





        public List<OrdinaryWorkProgramTimeModel> OrdinaryWorkProgramTimeModels { get; set; }
    }
}
