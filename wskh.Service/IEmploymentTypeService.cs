using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IEmploymentTypeService
    {
        List<EmploymentType> GetList { get; }

        int Count();
        int Count(string search);
        void Create(EmploymentType entity);
        void Delete(EmploymentType entity);
        List<EmploymentType> FilterData(int start, int lenght, string search);
        EmploymentType FindById(int id);
        List<EmploymentType> List();
        void Update(EmploymentType entity);
    }
}