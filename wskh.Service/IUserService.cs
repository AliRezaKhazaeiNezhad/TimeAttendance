using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IUserService
    {
        List<wskhUser> GetList { get; }

        int Count();
        int Count(string search);
        void Create(wskhUser entity);
        void Delete(wskhUser entity);
        void Dispose();
        List<wskhUser> FilterData(int start, int lenght, string search);
        int InstantCount(string search, int type = 0, string userId = null, int usergroupId = 0);
        List<wskhUser> InstantFilterData(int start, int lenght, string search, int type = 0, string userId = null, int usergroupId = 0);
        List<wskhUser> List();
        List<wskhUser> List2();
        void Update(wskhUser entity);
    }
}