using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class WorkProgramModel : BaseModel
    {
        public WorkProgramModel()
        {
            WorkProgramDayModels = new List<WorkProgramDayModel>();
            TypeList = new List<SelectListItem>();
            TypeList.Add(new SelectListItem() { Value = "0", Text = "انتخاب نمایید" });
            TypeList.Add(new SelectListItem() { Value = "1", Text = "هفتگی یا چرخشی" });
            TypeList.Add(new SelectListItem() { Value = "2", Text = "شناور" });
            TypeList.Add(new SelectListItem() { Value = "3", Text = "بیمارستانی و نگهبانی" });

            MoreThan24HouresList = new List<SelectListItem>();
            MoreThan24HouresList.Add(new SelectListItem() { Value = "0", Text = "خیر" });
            MoreThan24HouresList.Add(new SelectListItem() { Value = "1", Text = "بلی" });

        }

        [MaxLength(50)]
        [Display(Name = "عنوان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }


        [Display(Name = "نوع برنامه"), Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int TypeId { get; set; }


        [MaxLength(5)]
        [Display(Name = "کارکرد در روز (00:00)")]
        public string WorkTimeInDay { get; set; }
        [Display(Name = "شیفت بیشتر از 24 ساعت")]
        public int MoreThan24HouresId { get; set; }
        public List<SelectListItem> MoreThan24HouresList { get; set; }


        public string TypeString { get; set; }


        public virtual List<WorkProgramDayModel> WorkProgramDayModels { get; set; }
        public List<SelectListItem> TypeList { get; set; }
    }
}
