using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core.Enumerator;

namespace TimeAttendance.Model
{
    public class OrdinaryWorkProgramModel
    {
        public OrdinaryWorkProgramModel()
        {
            Type = new WorkProgramType();
            OrdinaryWorkProgramDayModels = new List<OrdinaryWorkProgramDayModel>();
            Id = 0;
        }

        public int Id { get; set; }
        public WorkProgramType Type { get; set; }
        [MaxLength(50)]
        [Display(Name = "عنوان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }

        public int WorkProgramType { get; set; }
        public bool MoreThan24Houres { get; set; }
        [Display(Name = "محاسبه زمان استراحت توسط نرم افزار؟")]
        public bool CalculateAbsence { get; set; }

        public List<OrdinaryWorkProgramDayModel> OrdinaryWorkProgramDayModels { get; set; }
    }
}
