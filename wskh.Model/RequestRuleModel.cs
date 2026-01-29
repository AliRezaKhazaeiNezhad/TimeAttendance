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
    public class RequestRuleModel : BaseModel
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public RequestRuleModel()
        {
            MaximumAbsence = "02:45";
            MaximumAbsence = "02:30";
            DayDuration = "07:25";
            DelayActionList = new List<SelectListItem>();
            DelayActionPercent = "1";
            DelyMonthMin = "0";
            DelayDayMin = "0";
            FlowTimeMin = "0";
            RequestRuleDetailModels = new List<RequestRuleDetailModel>();

            var _leaveTypeService = DependencyResolver.Current.GetService<ILeaveTypeService>();

            var leaveTypeList = _leaveTypeService.GetList.OrderBy(x => x.Id).ToList();
            int index = 0;

            foreach (var leaveType in leaveTypeList)
            {
                RequestRuleDetailModel ruleDetailModel = new RequestRuleDetailModel();
                ruleDetailModel.LeaveType = leaveType;
                ruleDetailModel.LeaveTypeId = leaveType.Id;
                ruleDetailModel.LeaveTypeName = leaveType.Title;
                ruleDetailModel.Index = index;
                ruleDetailModel.RestDayCountMonthly = "0";

                if (leaveType.Id > 1)
                {
                    ruleDetailModel.RestDayCountYearly = "0";
                }


                RequestRuleDetailModels.Add(ruleDetailModel);
                index = ++index;
            }


            DelayActionList.Add(new SelectListItem() { Value = "0", Text = "هیچ عملیاتی انجام نشود" });
            DelayActionList.Add(new SelectListItem() { Value = "1", Text = "کسر از استحقاقی از ابتدای ساعت" });
            DelayActionList.Add(new SelectListItem() { Value = "2", Text = "کسر از استحقاقی از ابتدای تاخیر" });
            DelayActionList.Add(new SelectListItem() { Value = "3", Text = "غیبت" });
            DelayActionList.Add(new SelectListItem() { Value = "4", Text = "کسر با ضریب" });

            FlowTimes = new List<SelectListItem>();
            FlowTimes.Add(new SelectListItem() {
                Value = "0",
                Text = "ندارد"
            });
            FlowTimes.Add(new SelectListItem()
            {
                Value = "1",
                Text = "دارد"
            });
        }
        #endregion

        #region Propertices
        /// <summary>
        /// عنوان گروه
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "عنوان قوانین تردد"), Required (ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }


        /// <summary>
        /// سقف مرخصی ساعتی
        /// </summary>
        [MaxLength(5)]
        [Display(Name = "سقف مرخصی ساعتی"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string MaximumAbsence { get; set; }


        /// <summary>
        /// طول یک روز کاری برای محاسبه ساعت مرخصی و ماموریت روزانه
        /// </summary>
        [MaxLength(5)]
        [Display(Name = "طول یک روز کاری برای محاسبه ساعت مرخصی و ماموریت روزانه"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string DayDuration { get; set; }


        /// <summary>
        /// درصورت تاخیر ورود چه اتفاقی رخ دهد؟
        /// </summary>
        [Display(Name = "درصورت تاخیر ورود"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int DelayActionId { get; set; }
        public List<SelectListItem> DelayActionList { get; set; }


        /// <summary>
        /// ضریب کسر
        /// </summary>
        [MaxLength(8)]
        [Display(Name = "ضریب کسر"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string DelayActionPercent { get; set; }


        /// <summary>
        /// تاخیر مجاز در ماه برحسب دقیقه
        /// </summary>
        [MaxLength(3)]
        [Display(Name = "حداکثر تاخیر مجاز در ماه برحسب دقیقه"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string DelyMonthMin { get; set; }


        /// <summary>
        /// تاخیر مجاز در روز برحسب دقیقه
        /// </summary>
        [MaxLength(3)]
        [Display(Name = "حداکثر تاخیر مجاز در روز برحسب دقیقه"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string DelayDayMin { get; set; }



     


        /// <summary>
        /// آیا شناوری در روز دارد
        /// </summary>
        [Display(Name = "آیا شناوری در روز دارد")]
        public int FlowTimeId { get; set; }
        public List<SelectListItem> FlowTimes { get; set; }


        /// <summary>
        /// مدت شناوری برحسب دقیقه
        /// </summary>
        [MaxLength(3)]
        [Display(Name = "مدت شناوری در روز (دقیقه)")]
        public string FlowTimeMin { get; set; }



        #endregion

        #region Relations
        public List<RequestRuleDetailModel> RequestRuleDetailModels { get; set; }
        #endregion
    }


    public class RequestRuleGridModel : BaseModel
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public RequestRuleGridModel()
        {
        }
        #endregion

        /// <summary>
        /// عنوان گروه
        /// </summary>
        public string Title { get; set; }


    }
}
