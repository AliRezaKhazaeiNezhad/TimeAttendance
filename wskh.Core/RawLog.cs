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
    [Table("RawLogs")]
    public class RawLog
    {
        public RawLog()
        {
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]
        public string EnrollNo { get; set; }
        [MaxLength(4)]
        public string Year { get; set; }
        [MaxLength(4)]
        public string Month { get; set; }
        [MaxLength(4)]
        public string Day { get; set; }
        [MaxLength(4)]
        public string Hour { get; set; }
        [MaxLength(4)]
        public string Minute { get; set; }
        [MaxLength(4)]
        public string Second { get; set; }
        [MaxLength(10)]
        public string VerifyMode { get; set; }
        [MaxLength(10)]
        public string InOutMode { get; set; }
        [MaxLength(10)]
        public string WorkCode { get; set; }
        public int DeviceId { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? LogDate { get; set; }
        [MaxLength(15)]
        public string LogTime { get; set; }
        public int? EnrollId { get; set; }
    }
}
