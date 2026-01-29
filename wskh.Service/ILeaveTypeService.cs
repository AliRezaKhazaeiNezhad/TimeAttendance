using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ILeaveTypeService
    {
        List<LeaveType> GetList { get; }

        int Count();
        int Count(string search);
        void Create(LeaveType entity);
        void Delete(LeaveType entity);
        List<LeaveType> FilterData(int start, int lenght, string search);
        LeaveType FindById(int id);
        List<LeaveType> List();
        void Update(LeaveType entity);
    }
}