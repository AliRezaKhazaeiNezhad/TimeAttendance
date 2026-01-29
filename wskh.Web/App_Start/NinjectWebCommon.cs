[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(wskh.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(wskh.Web.App_Start.NinjectWebCommon), "Stop")]

namespace wskh.Web.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using TimeAttendance.Service;
    using wskh.Data;
    using wskh.Service;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }


        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region Context Service
            kernel.Bind<DbContext>().To<wskhContext>().InRequestScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind<IEducationLevelService>().To<EducationLevelService>();
            kernel.Bind<IEmploymentTypeService>().To<EmploymentTypeService>();
            kernel.Bind<IOrganizationBranchService>().To<OrganizationBranchService>();
            kernel.Bind<IOrganizationLevelService>().To<OrganizationLevelService>();
            kernel.Bind<IUserGroupService>().To<UserGroupService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IFingerDeviceService>().To<FingerDeviceService>();
            kernel.Bind<IDeviceCardService>().To<DeviceCardService>();
            kernel.Bind<IRawEnrollService>().To<RawEnrollService>();
            kernel.Bind<IRawLogService>().To<RawLogService>();
            kernel.Bind<IWorkProgramService>().To<WorkProgramService>(); 
            kernel.Bind<IWorkProgramTimeService>().To<WorkProgramTimeService>();
            kernel.Bind<IWorkProgramDayService>().To<WorkProgramDayService>();
            kernel.Bind<ICalendarService>().To<CalendarService>();
            kernel.Bind<ICalendarDayService>().To<CalendarDayService>();
            kernel.Bind<ISpecialDayService>().To<SpecialDayService>();
            kernel.Bind<ISpecialDayGroupingService>().To<SpecialDayGroupingService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<IEnrollService>().To<EnrollService>();
            kernel.Bind<IPatchHistoryService>().To<PatchHistoryService>();
            kernel.Bind<IReportDayService>().To<ReportDayService>();
            kernel.Bind<IDeviceWorkCodeService>().To<DeviceWorkCodeService>();
            kernel.Bind<IUserGroupCalendareService>().To<UserGroupCalendareService>();
            kernel.Bind<IRequestService>().To<RequestService>();
            kernel.Bind<ILeaveTypeService>().To<LeaveTypeService>();
            kernel.Bind<IOrganizationInformationService>().To<OrganizationInformationService>();
            kernel.Bind<IAboutSoftwareService>().To<AboutSoftwareService>();
            kernel.Bind<IRequestRuleService>().To<RequestRuleService>();
            kernel.Bind<IRequestRuleDetailService>().To<RequestRuleDetailService>();
            kernel.Bind<ITicketService>().To<TicketService>();
            kernel.Bind<ICommandService>().To<CommandService>();
            kernel.Bind<IAnalyzedReportService>().To<AnalyzedReportService>();
            #endregion
        }
    }
}