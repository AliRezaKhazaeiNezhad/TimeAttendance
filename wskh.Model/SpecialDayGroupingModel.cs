using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Model;
using wskh.Service;

namespace TimeAttendance.Model
{
    public class SpecialDayGroupingModel : BaseModel
    {
        public SpecialDayGroupingModel()
        {
        }

        #region Propertices
        [MaxLength(50)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }
        #endregion
    }
}
