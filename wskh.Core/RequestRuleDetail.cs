using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    public class RequestRuleDetail : BaseEntity
    {
        #region Ctor
        public RequestRuleDetail()
        {
        }
        #endregion


        #region Propertices

        /// <summary>
        /// انتقال مرخصی به سال بعد
        /// </summary>
        [MaxLength(8)]
        public string RestToNextYear { get; set; }

        /// <summary>
        /// انتقال مرخصی به ماه بعد
        /// </summary>
        [MaxLength(8)]
        public string RestToNextMonth { get; set; }



        /// <summary>
        /// تعداد روز مرخصی در ماه
        /// </summary>
        [MaxLength(8)]
        public string RestDayCountMonthly { get; set; }

        /// <summary>
        /// تعداد روز مرخصی در سال
        /// </summary>
        [MaxLength(8)]
        public string RestDayCountYearly { get; set; }



        /// <summary>
        /// تعداد ساعات مرخصی در ماه
        /// </summary>
        [MaxLength(8)]
        public string RestDayHourCountMonthly { get; set; }



        /// <summary>
        /// میزان مرخصی درسال برحسب دقیقه
        /// </summary>
        [MaxLength(10)]
        public string YearlyRestMin { get; set; }


        /// <summary>
        /// میزان مرخصی درماه برحسب دقیقه
        /// </summary>
        [MaxLength(10)]
        public string MonthlyRestMin { get; set; }


        #endregion


        #region relations
        public int LeaveTypeId { get; set; }
        public virtual LeaveType LeaveType { get; set; }

        #endregion
    }
}
