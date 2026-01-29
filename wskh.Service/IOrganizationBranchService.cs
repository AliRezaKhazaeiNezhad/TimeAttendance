using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IOrganizationBranchService
    {
        List<OrganizationBranch> GetList { get; }

        int Count();
        int Count(string search);
        void Create(OrganizationBranch entity);
        void Delete(OrganizationBranch entity);
        List<OrganizationBranch> FilterData(int start, int lenght, string search);
        OrganizationBranch FindById(int id);
        List<OrganizationBranch> List();
        void Update(OrganizationBranch entity);
    }
}