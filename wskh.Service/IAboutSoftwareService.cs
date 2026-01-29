using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IAboutSoftwareService
    {
        List<AboutSoftware> GetList { get; }

        int Count();
        void Create(AboutSoftware entity);
        void Delete(AboutSoftware entity);
        AboutSoftware FindById(int id);
        List<AboutSoftware> List();
        void Update(AboutSoftware entity);
    }
}