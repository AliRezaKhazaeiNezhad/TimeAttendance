using System.Collections.Generic;
using TimeAttendance.Core;

namespace TimeAttendance.Service
{
    public interface IDeviceWorkCodeService
    {
        List<DeviceWorkCode> GetList { get; }

        int Count();
        int Count(string search);
        void Create(DeviceWorkCode entity);
        void Delete(DeviceWorkCode entity);
        DeviceWorkCode FindById(int id);
        List<DeviceWorkCode> List();
        void Update(DeviceWorkCode entity);
    }
}