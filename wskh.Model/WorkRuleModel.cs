using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using wskh.Model;
using wskh.Service;

namespace TimeAttendance.Core
{
    public class WorkRuleModel : BaseModel
    {
        #region Ctor
        public WorkRuleModel()
        {
            FlowTimeList = new List<SelectListItem>();
            FlowTimeList.Add(new SelectListItem() {
                Value = "0",
                Text = "خیر"
            });
            FlowTimeList.Add(new SelectListItem()
            {
                Value = "1",
                Text = "بلی"
            });

            RequestRules = new List<SelectListItem>();
            var _requestRuleService = DependencyResolver.Current.GetService<IRequestRuleService>();
            var listRules = _requestRuleService.GetList;
            if (listRules != null && listRules.Count() > 0)
            {
                listRules.ForEach(x => RequestRules.Add(new SelectListItem() {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }

        }
        #endregion


        #region Propertices

        /// <summary>
        /// اعمال کلید تابعی در تحلیل
        /// </summary>
        [Display(Name = "استفاده از کلید تابع")]
        public bool FunctionKey { get; set; }
        /// <summary>
        /// اعمال کد کار در تحلیل
        /// </summary>
        [Display(Name = "استفاده از کدکار")]
        public bool WorkCode { get; set; }
        /// <summary>
        /// اعمال کارت تردد در تحلیل
        /// </summary>
        [Display(Name = "استفاده از کارت تردد")]
        public bool Card { get; set; }


        /// <summary>
        /// آیا شناوری در روز دارد
        /// </summary>
        [Display(Name = "شناوری در روز دارد؟")]
        public int FlowTimeId { get; set; }
        public List<SelectListItem> FlowTimeList { get; set; }


        /// <summary>
        /// مدت شناوری برحسب دقیقه
        /// </summary>
        [Display(Name = "شناوری در روز (دقیقه)")]
        [MaxLength(3)]
        public string FlowTimeMin { get; set; }


        /// <summary>
        /// تاخیر در ماه برحسب دقیقه
        /// </summary>
        [Display(Name = "قوانین تردد")]
        public int RequestRuleId { get; set; }
        public List<SelectListItem> RequestRules { get; set; }

        #endregion


    }
}
