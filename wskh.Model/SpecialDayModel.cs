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
    public class SpecialDayModel : BaseModel
    {
        public SpecialDayModel()
        {
            SpecialDayGroupings = new List<SelectListItem>();
            var service = DependencyResolver.Current.GetService<ISpecialDayGroupingService>();
            var list = service.GetList;
            if (list != null && list.Count() > 0)
            {
                list.ForEach(x => SpecialDayGroupings.Add(new SelectListItem() {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }


            WorkPrograms = new List<SelectListItem>();
            var serviceWP = DependencyResolver.Current.GetService<IWorkProgramService>();
            var listWP = serviceWP.GetList.Where(x => x.Type == wskh.Core.Enumerator.WorkProgramType.Ordinary).ToList();
            if (listWP != null && list.Count() > 0)
            {
                listWP.ForEach(x => WorkPrograms.Add(new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }


            TypeList = new List<SelectListItem>();
            TypeList.Add(new SelectListItem() { Value = "0" , Text = "تعطیلی" });
            TypeList.Add(new SelectListItem() { Value = "1" , Text = "روزخاص" });
            //TypeList.Add(new SelectListItem() { Value = "2" , Text = "ایام خاص" });

            DelayExit = "0";
            DelayEnterance = "0";
        }

        #region Propertices

        [MaxLength(50)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }


        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }


        [Display(Name = "تاریخ پایان")]
        public string EndDate { get; set; }


        [Display(Name = "تاخیر ورود (دقیقه)")]
        public string DelayEnterance { get; set; }


        [Display(Name = "تاخیر خروج (دقیقه)")]
        public string DelayExit { get; set; }



        [Display(Name = "نوع تعطیلی")]
        public int TypeId { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public string TypeString { get; set; }

        #endregion
        #region Relations


        [Display(Name = "گروه تعطیلی و روز خاص")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int SpecialDayGroupingId { get; set; }
        public List<SelectListItem> SpecialDayGroupings { get; set; }
        public string SpecialDayGroupingString { get; set; }




        [Display(Name = "برنامه کاری")]
        public int WorkProgramId { get; set; }
        public List<SelectListItem> WorkPrograms { get; set; }
        public string WorkProgramString { get; set; }
        #endregion
    }
}
