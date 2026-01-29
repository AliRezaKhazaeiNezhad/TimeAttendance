using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Model
{
    public class UserGroupModel : BaseModel
    {
        public UserGroupModel()
        {

        }

        [MaxLength(75)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }
    }
}
