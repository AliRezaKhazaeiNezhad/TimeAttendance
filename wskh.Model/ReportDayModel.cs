using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Model;

namespace TimeAttendance.Core
{
    public class ReportDayModel : BaseModel
    {
        #region Ctor
        public ReportDayModel()
        {
        }
        #endregion
        #region Fileds
        public string DayName { get; set; }
        public string ReportDate { get; set; }
        public string DeviceName { get; set; }

        public string EnrollDetail { get; set; }
        public string EnrollInSoftWareDetail { get; set; }

        public string ContractName { get; set; }
        public string Logs { get; set; }
        #endregion

    }
}
