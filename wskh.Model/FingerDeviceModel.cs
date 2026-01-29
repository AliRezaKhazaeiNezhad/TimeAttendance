using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Model;

namespace wskh.Model
{
    public class FingerDeviceModel : BaseModel
    {
        #region Ctor
        public FingerDeviceModel()
        {
            DeviceInnerId = "1";
            PortNo = "4370";
            CommKey = "0";
            IP = "192.168.1.201";

            F1List = new List<SelectListItem>();
            F2List = new List<SelectListItem>();
            F3List = new List<SelectListItem>();
            F4List = new List<SelectListItem>();
            F5List = new List<SelectListItem>();
            F6List = new List<SelectListItem>();
            F7List = new List<SelectListItem>();
            F8List = new List<SelectListItem>();
        }
        #endregion
        #region ای دی ، حذف و پیام ها
        [Description("این فیلد تنها برای بررسی قابلیت اتصال دستگاه در زمان پیش از ثبت  میباشد")]
        public bool Success { get; set; }
        public bool Remove { get; set; }
        #endregion
        #region اطلاعات اتصال
        [MaxLength(50)]
        [Display(Name = "نام دستگاه"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }
        [MaxLength(4)]
        [Display(Name = "شماره دستگاه"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string DeviceInnerId { get; set; }
        [MaxLength(4)]
        [Display(Name = "شماره پورت"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string PortNo { get; set; }
        [MaxLength(5)]
        [Display(Name = "رمزعبور اتصال (comkey)"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string CommKey { get; set; }
        [MaxLength(15)]
        [Display(Name = "آی پی"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string IP { get; set; }
        #endregion
        #region اطلاعات تکمیلی
        [Display(Name = "ورژن رابط(SDK)")]
        public string SDKVersion { get; set; }
        [Display(Name = "ورژن Firmware")]
        public string FirmwareVersion { get; set; }
        [Display(Name = "سازنده")]
        public string Manufacturer { get; set; }
        [Display(Name = "شماره سریال")]
        public string SerialNo { get; set; }
        [Display(Name = "آدرس مک")]
        public string MacAddress { get; set; }
        [Display(Name = "مدل")]
        public string ModelName { get; set; }
        [Display(Name = "شرح FTP")]
        public string FTPDescription { get; set; }
        [Display(Name = "صفحه رنگی")]
        public bool IsColorScreen { get; set; }
        public string IsColorScreenString { get; set; }
        #endregion
        #region کلیدهای تابعی
        [Display(Name = "کلید F1")]
        public int? F1Id { get; set; }
        public List<SelectListItem> F1List { get; set; }

        [Display(Name = "کلید F2")]
        public int? F2Id { get; set; }
        public List<SelectListItem> F2List { get; set; }

        [Display(Name = "کلید F3")]
        public int? F3Id { get; set; }
        public List<SelectListItem> F3List { get; set; }

        [Display(Name = "کلید F4")]
        public int? F4Id { get; set; }
        public List<SelectListItem> F4List { get; set; }

        [Display(Name = "کلید F5")]
        public int? F5Id { get; set; }
        public List<SelectListItem> F5List { get; set; }

        [Display(Name = "کلید F6")]
        public int? F6Id { get; set; }
        public List<SelectListItem> F6List { get; set; }

        [Display(Name = "کلید F7")]
        public int? F7Id { get; set; }
        public List<SelectListItem> F7List { get; set; }

        [Display(Name = "کلید F8")]
        public int? F8Id { get; set; }
        public List<SelectListItem> F8List { get; set; }

        [Display(Name = "کلید F9")]
        public int? F9Id { get; set; }
        public List<SelectListItem> F9List { get; set; }
        #endregion
    }
}
