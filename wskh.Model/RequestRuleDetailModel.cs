using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class RequestRuleDetailModel : BaseModel
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public RequestRuleDetailModel()
        {
            RestDayCountMonthly = "0";
            RestDayCountYearly = "26";
            RestDayHourCountMonthly = "0";

            RestToNextMonth = "0";
            RestToNextYear = "0";

            CalculateYearly = "true";

            LeaveType = new LeaveType();
        }
        #endregion

        #region Propertices



        /// <summary>
        /// محاسبه بصورت سالانه
        /// </summary>
        public string CalculateYearly { get; set; }


        /// <summary>
        /// انتقال مرخصی به سال بعد
        /// </summary>
        [Display(Name = "انتقال مرخصی به سال بعد")]
        public string RestToNextYear { get; set; }

        /// <summary>
        /// انتقال مرخصی به ماه بعد
        /// </summary>
        [Display(Name = "انتقال مرخصی به ماه بعد")]
        public string RestToNextMonth { get; set; }




        /// <summary>
        /// تعداد روز مرخصی در ماه
        /// </summary>
        [MaxLength(8)]
        [Display(Name = "تعداد روز مرخصی در ماه")]
        public string RestDayCountMonthly { get; set; }

        /// <summary>
        /// تعداد روز مرخصی در سال
        /// </summary>
        [MaxLength(8)]
        [Display(Name = "تعداد روز مرخصی در سال")]
        public string RestDayCountYearly { get; set; }

        /// <summary>
        /// تعداد ساعات مرخصی در ماه
        /// </summary>
        [MaxLength(8)]
        [Display(Name = "تعداد ساعات مرخصی در ماه")]
        public string RestDayHourCountMonthly { get; set; }



        public string LeaveTypeName { get; set; }
        public int LeaveTypeId { get; set; }
        #endregion


        #region Relations
        public LeaveType LeaveType { get; set; }
        #endregion
    }
}
