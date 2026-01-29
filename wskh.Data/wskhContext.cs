using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core;

namespace wskh.Data
{
    public class wskhContext : IdentityDbContext<wskhUser>
    {
        public wskhContext() : base("wskhContext", throwIfV1Schema: false)
        {
        }
        public static wskhContext Create()
        {
            return new wskhContext();
        }



        #region Basic
        public DbSet<OrganizationLevel> OrganizationLevels { get; set; }
        public DbSet<OrganizationBranch> OrganizationBranchs { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<RawLog> RawLogs { get; set; }
        public DbSet<RawEnroll> RawEnrolls { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<FingerDevice> FingerDevices { get; set; }
        public DbSet<WorkProgramDay> WorkProgramDays { get; set; }
        public DbSet<WorkProgramTime> WorkProgramTimes { get; set; }
        public DbSet<WorkProgram> WorkPrograms { get; set; }
        public DbSet<SpecialDay> SpecialDays { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarDay> CalendarDays { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ReportDay> ReportDays { get; set; }
        public DbSet<DeviceCard> DeviceCards { get; set; }
        public DbSet<DeviceWorkCode> DeviceWorkCodes { get; set; }
        public DbSet<SpecialDayGrouping> SpecialDayGroupings { get; set; }
        public DbSet<PatchHistory> PatchHistories { get; set; }
        public DbSet<UserGroupCalendare> UserGroupCalendares { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<AboutSoftware> AboutSoftwares { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<OrganizationInformation> OrganizationInformation { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<AnalyzedReport> AnalyzedReports { get; set; }
        public DbSet<AnalyzedReportLog> AnalyzedReportLogs { get; set; }
        #endregion

        #region OnModelCreating
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //}
        #endregion
    }
}
