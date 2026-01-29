using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IEducationLevelService
    {
        List<EducationLevel> GetList { get; }

        int Count();
        int Count(string search);
        void Create(EducationLevel entity);
        void Delete(EducationLevel entity);
        List<EducationLevel> FilterData(int start, int lenght, string search);
        EducationLevel FindById(int id);
        List<EducationLevel> List();
        void Update(EducationLevel entity);
    }
}