using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core.Enumerator;

namespace TimeAttendance.Core
{
    public class AnalyzedReportLog
    {
        public AnalyzedReportLog()
        {

        }

        [Key]
        public int Id { get; set; }

        [MaxLength(15)]
        public string FirstLogTime { get; set; }

        [MaxLength(15)]
        public string SecondLogTime { get; set; }

        [MaxLength(15)]
        public string Duration { get; set; }

        public TimeType TimeType { get; set; }





        public int AnalyzedReportId { get; set; }
        [ForeignKey("AnalyzedReportId")]
        public virtual AnalyzedReport AnalyzedReport { get; set; }


        public int FirstLogId { get; set; }
        [ForeignKey("FirstLogId")]
        public virtual Log FirstLog { get; set; }

        public int? SecondLogId { get; set; }
        [ForeignKey("SecondLogId")]
        public virtual Log SecondLog { get; set; }
    }
}
