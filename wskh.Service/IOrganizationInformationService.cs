using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IOrganizationInformationService
    {
        List<OrganizationInformation> GetList { get; }

        int Count();
        int Count(string search);
        void Create(OrganizationInformation entity);
        void Delete(OrganizationInformation entity);
        List<OrganizationInformation> FilterData(int start, int lenght, string search);
        OrganizationInformation FindById(int id);
        List<OrganizationInformation> List();
        void Update(OrganizationInformation entity);
    }
}