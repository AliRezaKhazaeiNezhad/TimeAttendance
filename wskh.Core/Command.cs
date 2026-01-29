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
    public class Command : BaseEntity
    {
        public Command()
        {
            Count = 0;
            CreateDateTime = DateTime.Now;
        }


        /// <summary>
        /// فرمان ها را طبقه بندی میکند
        /// </summary>
        public CommandCategory CommandCategory { get; set; }

        /// <summary>
        /// وضعیت فرمان را مشخص میکند 
        /// </summary>
        public CommandState State { get; set; }

        public int? EntityId { get; set; }




        /// <summary>
        /// شرح فرمان
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// شمارنده تعدادی (برای لاگ یا انرول دریافت شده)
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// درصورت شکست این فرمان، دلیل را مینویسد
        /// </summary>
        [MaxLength(500)]
        public string ErorrMessage { get; set; }

        /// <summary>
        /// تاریخ ایجاد فعالیت
        /// </summary>
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// تاریخ شروع فعالیت
        /// </summary>
        public DateTime? StartingDateTime { get; set; }

        /// <summary>
        /// تاریخ اتمام فعالیت
        /// </summary>
        public DateTime? FinishDateTime { get; set; }


        /// <summary>
        /// ای دی کاربر درخواست کننده فرمان
        /// </summary>
        public string UserId { get; set; }
        public virtual wskhUser User { get; set; }
    }
}
