using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core;

namespace wskh.Core
{
    [Table("Enrolls")]
    public class Enroll
    {
        #region Ctor
        public Enroll()
        {
            ReportDays = new List<ReportDay>();
            Logs = new List<Log>();
        }
        #endregion
        #region Propertices
        [Key]
        public int Id { get; set; }


        public int EnrollNo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Privileg { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        #endregion
        #region relations
        public int FingerDeviceId { get; set; }
        [JsonIgnore]
        public virtual FingerDevice FingerDevice { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        public virtual wskhUser User { get; set; }
        [JsonIgnore]
        public virtual List<ReportDay> ReportDays { get; set; }

        [JsonIgnore]
        public virtual List<Log> Logs { get; set; }
        #endregion
    }
}
