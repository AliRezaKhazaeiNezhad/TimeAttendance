using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAttendance.Core;
using TimeAttendance.Model;
using wskh.Core;
using wskh.Model;


namespace wskh.Web.Helper
{
    public static class AutoMapperHelper
    {
        public static void Initialize()
        {
            EducationLeve();
        }

        private static void EducationLeve()
        {
            Mapper.CreateMap<EducationLevelModel, EducationLevel>().ReverseMap();
            Mapper.CreateMap<EmploymentTypeModel, EmploymentType>().ReverseMap();
            Mapper.CreateMap<OrganizationBranchModel, OrganizationBranch>().ReverseMap();
            Mapper.CreateMap<OrganizationLevelModel, OrganizationLevel>().ReverseMap();
            Mapper.CreateMap<UserGroupModel, UserGroup>().ReverseMap();
            Mapper.CreateMap<FingerDeviceModel, FingerDevice>().ReverseMap();
            Mapper.CreateMap<FlowWorkProgramModel, WorkProgram>().ReverseMap();
            Mapper.CreateMap<SpecialDayGroupingModel, SpecialDayGrouping>().ReverseMap();
        }
    }
}