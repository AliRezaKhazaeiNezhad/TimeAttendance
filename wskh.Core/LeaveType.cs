using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    public class LeaveType : BaseEntity
    {
        #region Ctor
        public LeaveType()
        {
            RequestRuleDetails = new List<RequestRuleDetail>();
            Requests = new List<Request>();
        }
        #endregion


        #region Propertices

        [MaxLength(50)]
        public string Title { get; set; }


        public bool AllowRemove { get; set; }

        #endregion

        #region Relations
        public virtual List<RequestRuleDetail> RequestRuleDetails { get; set; }

        public virtual List<Request> Requests { get; set; }

        #endregion
    }
}
