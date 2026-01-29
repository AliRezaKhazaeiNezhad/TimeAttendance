using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAttendance.Core;

namespace wskh.Core
{
    [Table("AspNetUsers")]
    public class wskhUser : IdentityUser
    {
        #region Ctor
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<wskhUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public wskhUser()
        {
            Enrolls = new List<Enroll>();
            UserRequests = new List<Request>();
            ManagerRequests = new List<Request>();
            RequestUsers = new List<Ticket>();
            ResponseUsers = new List<Ticket>();
            ReportDays = new List<ReportDay>();
            Logs = new List<Log>();
            Commands = new List<Command>();
            AnalyzedReports = new List<AnalyzedReport>();
            LeaveRequestUsers = new List<Leave>();
            LeaveSecondUsers = new List<Leave>();
            LeaveApproveUsers = new List<Leave>();
        }
        #endregion
        #region Propertices
        [MaxLength(250)]
        public string UserRoleType { get; set; }
        [MaxLength(100)]
        public string NationalCode { get; set; }

        public bool Sex { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(75)]
        public string Lastname { get; set; }
        public bool Active { get; set; }
        #endregion
        #region Relations
        public virtual List<Enroll> Enrolls { get; set; }

        public int? UserGroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }

        public int? EducationLevelId { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }

        public int? EmploymentTypeId { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }

        public int? OrganizationBranchId { get; set; }
        public virtual OrganizationBranch OrganizationBranch { get; set; }

        public int? OrganizationLevelId { get; set; }
        public virtual OrganizationLevel OrganizationLevel { get; set; }


        [InverseProperty("UserRequester")]
        public virtual List<Request> UserRequests { get; set; }
        [InverseProperty("UserRequesteManager")]
        public virtual List<Request> ManagerRequests { get; set; }


        [InverseProperty("RequestUser")]
        public virtual List<Ticket> RequestUsers { get; set; }
        [InverseProperty("ResponseUser")]
        public virtual List<Ticket> ResponseUsers { get; set; }


        public virtual List<ReportDay> ReportDays { get; set; }
        public virtual List<Log> Logs { get; set; }
        public virtual List<Command> Commands { get; set; }
        public virtual List<AnalyzedReport> AnalyzedReports { get; set; }


        [InverseProperty("RequestUser")]
        public virtual List<Leave> LeaveRequestUsers { get; set; }

        [InverseProperty("SecondUser")]
        public virtual List<Leave> LeaveSecondUsers { get; set; }

        [InverseProperty("ApproveUser")]
        public virtual List<Leave> LeaveApproveUsers { get; set; }

        #endregion
    }
}