using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IUserGroupService
    {
        List<UserGroup> GetList { get; }

        int Count();
        int Count(string search);
        void Create(UserGroup entity);
        void Delete(UserGroup entity);
        List<UserGroup> FilterData(int start, int lenght, string search);
        UserGroup FindById(int id);
        List<UserGroup> List();
        void Update(UserGroup entity);
    }
}