using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class WorkProgramGridModel : BaseModel
    {
        public WorkProgramGridModel()
        {

        }

        public string Title { get; set; }
        public string MoreThan24Hours { get; set; }
        public string Duration { get; set; }
        public string WorkProgramRule { get; set; }
        public string CreateDateTime { get; set; }

        public string TypeInGrid { get; set; }
    }
}
