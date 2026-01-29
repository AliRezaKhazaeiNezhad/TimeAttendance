namespace wskh.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using TimeAttendance.Data.SeedHelper;

    internal sealed class Configuration : DbMigrationsConfiguration<wskh.Data.wskhContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(wskhContext context)
        {
            UserSeed.Initial();
            BasicInformationSeed.Initial();
            SpecialDayGroupingSeed.Initial();
            LeaveTypeSeed.Initial();
            OrganizationInformationSeed.Initial();
            AboutSoftwareSeed.Initial();
        }
    }
}
