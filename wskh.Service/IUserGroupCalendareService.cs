using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IUserGroupCalendareService
    {
        List<UserGroupCalendare> GetList { get; }

        int Count();
        int Count(string search);
        void Create(UserGroupCalendare entity);
        void Delete(UserGroupCalendare entity);
        List<UserGroupCalendare> FilterData(int start, int lenght, string search);
        UserGroupCalendare FindById(int id);
        List<UserGroupCalendare> List();
        void Update(UserGroupCalendare entity);
    }
}