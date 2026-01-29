using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Model;

namespace TimeAttendance.Core
{
    public class CommandModel : BaseModel
    {
        public CommandModel()
        {
            Count = 0;
        }



        public string CommandCategory { get; set; }

        public string State { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public string ErorrMessage { get; set; }

        public string CreateDateTime { get; set; }

        public string StartingDateTime { get; set; }

        public string FinishDateTime { get; set; }

        public string User { get; set; }

        public string Device { get; set; }
    }
}
