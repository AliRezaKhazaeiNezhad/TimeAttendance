using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class OrganizationInformationModel : BaseModel
    {
        public OrganizationInformationModel()
        {

        }

        [MaxLength(75)]
        [Display(Name = "نام سازمان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }

        [MaxLength(75)]
        [Display(Name = "زمینه فعالیت"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Category { get; set; }

        [MaxLength(150)]
        [Display(Name = "آدرس"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Address { get; set; }

        [MaxLength(15)]
        [Display(Name = "شماره"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Phone { get; set; }

        [MaxLength(300)]
        public string LogoPath { get; set; }
        public string Image { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
    }
}
