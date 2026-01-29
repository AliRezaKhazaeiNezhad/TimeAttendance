using System.Collections.Generic;
using TimeAttendance.Core;

namespace TimeAttendance.Service
{
    public interface IDeviceCardService
    {
        List<DeviceCard> GetList { get; }

        int Count();
        int Count(string search);
        void Create(DeviceCard entity);
        void Delete(DeviceCard entity);
        DeviceCard FindById(int id);
        List<DeviceCard> List();
        void Update(DeviceCard entity);
    }
}