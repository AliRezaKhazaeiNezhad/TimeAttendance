using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class ChangePassWordModel
    {
        public ChangePassWordModel()
        {

        }
        [Display(Name = "گذرواژه جدید")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        [MinLength(6, ErrorMessage = "{0} حداقل 6 کلمه باید باشد.")]
        public string Password { get; set; }
        public string Error { get; set; }
    }
}
