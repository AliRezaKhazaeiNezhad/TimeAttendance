using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core.Enumerator;

namespace wskh.Core
{
    [Table("FingerDevices")]
    public class FingerDevice
    {
        #region Ctor
        public FingerDevice()
        {
            Enrolls = new List<Enroll>();
            DeviceCards = new List<DeviceCard>();
            PatchHistories = new List<PatchHistory>();
            DeviceWorkCodes = new List<DeviceWorkCode>();
            Logs = new List<Log>();
        }
        #endregion
        #region فیلدهای ای دی و حذف
        [Key]
        public int Id { get; set; }
        public bool Remove { get; set; }
        #endregion

        #region اطلاعات عملیاتی (اتصال)
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(4)]
        public string DeviceInnerId { get; set; }
        [MaxLength(4)]
        public string PortNo { get; set; }
        [MaxLength(5)]
        public string CommKey { get; set; }
        [MaxLength(15)]
        public string IP { get; set; }
        #endregion
        #region اطلاعات تکمیلی
        [MaxLength(30)]
        public string SDKVersion { get; set; }
        [MaxLength(30)]
        public string FirmwareVersion { get; set; }
        [MaxLength(30)]
        public string Manufacturer { get; set; }
        [MaxLength(30)]
        public string SerialNo { get; set; }
        [MaxLength(30)]
        public string MacAddress { get; set; }
        [MaxLength(30)]
        public string ModelName { get; set; }
        [MaxLength(30)]
        public string FTPDescription { get; set; }
        public bool IsColorScreen { get; set; }
        #endregion
        #region کلیدهای تابعی
        public FunctionKeyType F1 { get; set; }
        public FunctionKeyType F2 { get; set; }
        public FunctionKeyType F3 { get; set; }
        public FunctionKeyType F4 { get; set; }
        public FunctionKeyType F5 { get; set; }
        public FunctionKeyType F6 { get; set; }
        public FunctionKeyType F7 { get; set; }
        public FunctionKeyType F8 { get; set; }
        #endregion
        #region روابط
        public virtual List<Enroll> Enrolls { get; set; }
        public virtual List<DeviceCard> DeviceCards { get; set; }
        public virtual List<DeviceWorkCode> DeviceWorkCodes { get; set; }
        public virtual List<PatchHistory> PatchHistories { get; set; }
        public virtual List<Log> Logs { get; set; }
        #endregion
    }
}
