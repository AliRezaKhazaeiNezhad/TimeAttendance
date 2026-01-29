using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IOrganizationLevelService
    {
        List<OrganizationLevel> GetList { get; }

        int Count();
        int Count(string search);
        void Create(OrganizationLevel entity);
        void Delete(OrganizationLevel entity);
        List<OrganizationLevel> FilterData(int start, int lenght, string search);
        OrganizationLevel FindById(int id);
        List<OrganizationLevel> List();
        void Update(OrganizationLevel entity);
    }
}