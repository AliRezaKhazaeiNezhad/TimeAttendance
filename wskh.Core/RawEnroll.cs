using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Core
{
    [Table("RawEnrolls")]
    public class RawEnroll
    {
        #region Ctor
        public RawEnroll()
        {

        }
        #endregion
        #region Propertices
        [Key]
        public int Id { get; set; }
        public int DeviceId { get; set; }

        public int EnrollNo { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public int Privileg { get; set; }
        public bool Enabled { get; set; }
        #endregion
    }
}
