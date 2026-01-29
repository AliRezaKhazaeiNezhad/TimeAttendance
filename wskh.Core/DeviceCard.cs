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
    public class DeviceCard
    {
        #region Ctor
        public DeviceCard()
        {

        }
        #endregion
        #region Propertices
        [Key]
        public int Id { get; set; }

        public int EnrollNo { get; set; }
        public FunctionKeyType Type { get; set; }
        #endregion
        #region Relations
        public int FingerDeviceId { get; set; }
        public virtual FingerDevice FingerDevice { get; set; }
        #endregion
    }
}
