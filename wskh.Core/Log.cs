using Newtonsoft.Json;
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
    [Table("Logs")]
    public class Log
    {
        #region Ctor
        public Log()
        {
            FirstAnalyzedReportLogs = new List<AnalyzedReportLog>();

            SecondAnalyzedReportLogs = new List<AnalyzedReportLog>();
        }
        #endregion
        #region Propertices
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]
        public string VerifyMode { get; set; }
        [MaxLength(10)]
        public string InOutMode { get; set; }
        [MaxLength(10)]
        public string WorkCode { get; set; }
        public bool Orgin { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? LogDate { get; set; }
        [MaxLength(15)]
        public string LogTime { get; set; }



        public LogState State { get; set; }
        public LogTransportType TransportType { get; set; }
        //public LogType Type { get; set; }


        #endregion
        #region Relations
        public int? ReportDayId { get; set; }
        public virtual ReportDay ReportDay { get; set; }
        public int? EnrollId { get; set; }
        public int? DeviceId { get; set; }
        public virtual FingerDevice Device { get; set; }
        public virtual Enroll Enroll { get; set; }
        public int? EnrollNo { get; set; }
        public bool Remove { get; set; }


        public string CreatorUserId { get; set; }
        public virtual wskhUser CreatorUser { get; set; }




        [InverseProperty("FirstLog")]
        public virtual List<AnalyzedReportLog> FirstAnalyzedReportLogs { get; set; }

        [InverseProperty("SecondLog")]
        public virtual List<AnalyzedReportLog> SecondAnalyzedReportLogs { get; set; }

        #endregion
    }
}
