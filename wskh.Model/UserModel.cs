using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Data;

namespace wskh.Model
{
    public class UserModel
    {
        #region Ctor
        public UserModel()
        {
            SexList = new List<SelectListItem>();
            SexList.Add(new SelectListItem() { Value = "", Text = "انتخاب نمایید" });
            SexList.Add(new SelectListItem() { Value = "0", Text = "خانم" });
            SexList.Add(new SelectListItem() { Value = "1", Text = "آقا" });
            UserGroupList = new List<SelectListItem>();
            UserGroupList.Add(new SelectListItem() { Value = "", Text = "انتخاب نمایید" });
            EducationLevelList = new List<SelectListItem>();
            EducationLevelList.Add(new SelectListItem() { Value = "", Text = "انتخاب نمایید" });
            BranchList = new List<SelectListItem>();
            BranchList.Add(new SelectListItem() { Value = "", Text = "انتخاب نمایید" });
            OrganizationLevelList = new List<SelectListItem>();
            OrganizationLevelList.Add(new SelectListItem() { Value = "", Text = "انتخاب نمایید" });
            EmployeTypeList = new List<SelectListItem>();
            EmployeTypeList.Add(new SelectListItem() { Value = "", Text = "انتخاب نمایید" });
        }
        #endregion
        #region Fileds
        public string Id { get; set; }
        public int? Index { get; set; }

        public string GroupName { get; set; }


        [Display(Name = "نقش")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string UserRoleType { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int? SexId { get; set; }
        public List<SelectListItem> SexList { get; set; }

        [MaxLength(11)]
        [Display(Name = "نام کاربری (09...)")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string UserName { get; set; }

        [MaxLength(50)]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string FirstName { get; set; }

        [MaxLength(75)]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Lastname { get; set; }

        [MaxLength(10)]
        [Display(Name = "کدپرسنلی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string NationalCode { get; set; }

        [MaxLength(10)]
        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Password { get; set; }


        [Display(Name = "گروه کاربری")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int UserGroupId { get; set; }
        public List<SelectListItem> UserGroupList { get; set; }

        [Display(Name = "مقطع تحصیلی")]
        public int EducationLevelId { get; set; }
        public List<SelectListItem> EducationLevelList { get; set; }

        [Display(Name = "نوع استخدامی")]
        public int EmployeTypeId { get; set; }
        public List<SelectListItem> EmployeTypeList { get; set; }

        [Display(Name = "شعبه")]
        public int BranchId { get; set; }
        public List<SelectListItem> BranchList { get; set; }

        [Display(Name = "سمت سازمانی")]
        public int OrganizationLevelId { get; set; }
        public List<SelectListItem> OrganizationLevelList { get; set; }


        public string Active { get; set; }
        public string SexName { get; set; }
        public string _userName { get; set; }
        public string _nationalCode { get; set; }

        public bool Hashing()
        {
            bool result = false;
            try
            {
                _userName = HashHelper.Encrypt(UserName);
                _nationalCode = HashHelper.Encrypt(NationalCode);
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        #endregion
    }

    public class UserInformationModel
    {
        #region Ctor
        public UserInformationModel()
        {
            SexList = new List<SelectListItem>();
            SexList.Add(new SelectListItem() { Value = "0", Text = "مونث" });
            SexList.Add(new SelectListItem() { Value = "1", Text = "مذکر" });
        }
        #endregion
        #region Fileds
        public string Id { get; set; }

        [Display(Name = "نقش")]
        public string UserRoleType { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int SexId { get; set; }
        public List<SelectListItem> SexList { get; set; }

        [MaxLength(50)]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string FirstName { get; set; }

        [MaxLength(75)]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Lastname { get; set; }

        [MaxLength(10)]
        [Display(Name = "کدپرسنلی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string NationalCode { get; set; }

        [MaxLength(10)]
        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Password { get; set; }

        [Display(Name = "مقطع تحصیلی")]
        public string EducationLevel { get; set; }

        [Display(Name = "نوع استخدامی")]
        public string EmployeType { get; set; }

        [Display(Name = "شعبه")]
        public string Branch { get; set; }

        [Display(Name = "سمت سازمانی")]
        public string OrganizationLevel { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
    }
}
