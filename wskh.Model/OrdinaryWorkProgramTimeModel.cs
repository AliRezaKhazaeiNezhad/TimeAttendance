using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core.Enumerator;

namespace TimeAttendance.Model
{
    public class OrdinaryWorkProgramTimeModel
    {
        public OrdinaryWorkProgramTimeModel()
        {
            Id = 0;
        }

        public int Id { get; set; }

        [MaxLength(5)]
        [Display(Name = "شروع فعالیت"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartTime { get; set; }
        [MaxLength(5)]
        [Display(Name = "پایان فعالیت"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string EndTime { get; set; }
        [MaxLength(5)]
        public string Duration { get; set; }
        public TimeType TimeType { get; set; }
    }
}
