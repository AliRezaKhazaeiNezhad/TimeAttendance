using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Core.Enumerator;

namespace TimeAttendance.Core
{
    public class RequestRule : BaseEntity
    {
        #region Ctor
        public RequestRule()
        {
            RequestRuleDetails = new List<RequestRuleDetail>();
            Calendars = new List<Calendar>();
        }
        #endregion


        #region Propertices
        /// <summary>
        /// عنوان گروه
        /// </summary>
        [MaxLength(50)]
        public string Title { get; set; }



        /// <summary>
        /// سقف مرخصی ساعتی
        /// </summary>
        [MaxLength(5)]
        public string MaximumAbsence { get; set; }

        /// <summary>
        /// طول یک روز برای محاسبه ساعت مرخصی و ماموریت روزانه
        /// </summary>
        [MaxLength(5)]
        public string DayDuration { get; set; }

        /// <summary>
        /// درصورت تاخیر ورود چه اتفاقی رخ دهد؟
        /// </summary>
        public DelayAction DelayAction { get; set; }

        /// <summary>
        /// ضریب کسر
        /// </summary>
        [MaxLength(8)]
        public string DelayActionPercent { get; set; }

        /// <summary>
        /// تاخیر در ماه برحسب دقیقه
        /// </summary>
        [MaxLength(3)]
        public string DelyMonthMin { get; set; }

        /// <summary>
        /// تاخیر در روز برحسب دقیقه
        /// </summary>
        [MaxLength(3)]
        public string DelayDayMin { get; set; }




        /// <summary>
        /// اعمال کلید تابعی در تحلیل
        /// </summary>
        public bool FunctionKey { get; set; }
        /// <summary>
        /// اعمال کد کار در تحلیل
        /// </summary>
        public bool WorkCode { get; set; }
        /// <summary>
        /// اعمال کارت تردد در تحلیل
        /// </summary>
        public bool Card { get; set; }





        /// <summary>
        /// آیا شناوری در روز دارد
        /// </summary>
        public bool FlowTime { get; set; }
        /// <summary>
        /// مدت شناوری برحسب دقیقه
        /// </summary>
        [MaxLength(3)]
        public string FlowTimeMin { get; set; }
        #endregion


        #region Relations
        public virtual List<RequestRuleDetail> RequestRuleDetails { get; set; }
        public virtual List<Calendar> Calendars { get; set; }
        #endregion
    }
}
