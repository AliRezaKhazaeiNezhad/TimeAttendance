using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Data;

namespace TimeAttendance.Data.SeedHelper
{
    public static class BasicInformationSeed
    {
        public static void Initial()
        {
            wskhContext context = new wskhContext();
            #region مقاطع تحصیلی
            if (context.EducationLevels == null || context.EducationLevels.Count() == 0)
            {
                context.EducationLevels.Add(new EducationLevel()
                {
                    Title = "بی سواد",
                    Remove = false
                });
                context.EducationLevels.Add(new EducationLevel()
                {
                    Title = "کاردانی",
                    Remove = false
                });
                context.EducationLevels.Add(new EducationLevel()
                {
                    Title = "کارشناسی",
                    Remove = false
                });
                context.EducationLevels.Add(new EducationLevel()
                {
                    Title = "کارشناسی ارشد",
                    Remove = false
                });
                context.EducationLevels.Add(new EducationLevel()
                {
                    Title = "دکتری",
                    Remove = false
                });
                context.EducationLevels.Add(new EducationLevel()
                {
                    Title = "سایر",
                    Remove = false
                });
            }
            #endregion
            #region نوع استخدامی
            if (context.EmploymentTypes == null || context.EmploymentTypes.Count() == 0)
            {
                context.EmploymentTypes.Add(new EmploymentType()
                {
                    Title = "فصلی",
                    Remove = false
                });
                context.EmploymentTypes.Add(new EmploymentType()
                {
                    Title = "6 ماهه",
                    Remove = false
                });
                context.EmploymentTypes.Add(new EmploymentType()
                {
                    Title = "یک ساله",
                    Remove = false
                });
                context.EmploymentTypes.Add(new EmploymentType()
                {
                    Title = "استخدامی",
                    Remove = false
                });

            }
            #endregion
            #region شعبات سازمان
            if (context.OrganizationBranchs == null || context.OrganizationBranchs.Count() == 0)
            {
                context.OrganizationBranchs.Add(new OrganizationBranch()
                {
                    Title = "دفتر مرکزی",
                    Remove = false
                });
                context.OrganizationBranchs.Add(new OrganizationBranch()
                {
                    Title = "اداری",
                    Remove = false
                });
                context.OrganizationBranchs.Add(new OrganizationBranch()
                {
                    Title = "فروشگاه",
                    Remove = false
                });
                context.OrganizationBranchs.Add(new OrganizationBranch()
                {
                    Title = "سایر",
                    Remove = false
                });
            }
            #endregion
            #region سمت سازمانی
            if (context.OrganizationLevels == null || context.OrganizationLevels.Count() == 0)
            {
                context.OrganizationLevels.Add(new OrganizationLevel()
                {
                    Title = "اداری",
                    Remove = false
                });
                context.OrganizationLevels.Add(new OrganizationLevel()
                {
                    Title = "حسابدار",
                    Remove = false
                });
                context.OrganizationLevels.Add(new OrganizationLevel()
                {
                    Title = "کارشناس فنی",
                    Remove = false
                });
                context.OrganizationLevels.Add(new OrganizationLevel()
                {
                    Title = "مدیر بازرگانی",
                    Remove = false
                });
                context.OrganizationLevels.Add(new OrganizationLevel()
                {
                    Title = "مدیر فنی",
                    Remove = false
                });
                context.OrganizationLevels.Add(new OrganizationLevel()
                {
                    Title = "سایر",
                    Remove = false
                });
            }
            #endregion
            context.SaveChanges();
        }
    }
}
