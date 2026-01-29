using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Core.Enumerator;

namespace TimeAttendance.Core
{
    public class Request : BaseEntity
    {
        #region Ctor
        public Request()
        {
        }
        #endregion


        #region Propertices

        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public RequestState State { get; set; }

        /// <summary>
        /// نوع درخواست
        /// </summary>
        public RequestType Type { get; set; }

        /// <summary>
        /// دلیل رد درخواست
        /// </summary>
        [MaxLength(100)]
        public string RejectReason { get; set; }

        /// <summary>
        /// تاریخ شروع درخواست
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاریخ پایان درخواست
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// زمان شروع (مرخصی ساعتی
        /// </summary>
        [MaxLength(5)]
        public string StartTime { get; set; }

        /// <summary>
        /// زمان پایان (مرخصی ساعتی
        /// </summary>
        [MaxLength(5)]
        public string EndTime { get; set; }




        /// <summary>
        /// کل زمان عدم حضور
        /// </summary>
        [MaxLength(10)]
        public string TotalTime { get; set; }
        #endregion


        #region Relations
        /// <summary>
        /// کاربر درخواست کننده
        /// </summary>
        public string UserRequesterId { get; set; }
        [ForeignKey("UserRequesterId")]
        public virtual wskhUser UserRequester { get; set; }


        /// <summary>
        /// کاربر تایید/رد کننده
        /// </summary>
        public string UserRequesteManagerId { get; set; }
        [ForeignKey("UserRequesteManagerId")]
        public virtual wskhUser UserRequesteManager { get; set; }


        /// <summary>
        /// مشخص میکند برای کدام تقویم میباشد
        /// </summary>
        public int CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }


        /// <summary>
        /// نوع مرخصی را مشخص میکند
        /// </summary>
        public int? LeaveTypeId { get; set; }
        public virtual LeaveType LeaveType { get; set; }


        #endregion

    }
}
