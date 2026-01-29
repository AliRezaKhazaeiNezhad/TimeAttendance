using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class LeaveTypeModel : BaseModel
    {
        public LeaveTypeModel()
        {

        }

        #region Propertices

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(50)]
        public string Title { get; set; }


        public bool AllowRemove { get; set; }

        #endregion
    }
}
