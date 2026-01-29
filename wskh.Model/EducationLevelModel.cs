using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Model
{
    public class EducationLevelModel : BaseModel
    {
        public EducationLevelModel()
        {

        }


        [Display(Name = "عنوان مقطع تحصیلی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(75)]
        public string Title { get; set; }
        public bool Remove { get { return false; } set { } }
    }
}
