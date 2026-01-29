using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wskh.Core;
using wskh.FingerTec;
using wskh.Service;

namespace TimeAttendance.Web.Helper
{
    public static class FingerTecHelper
    {
        public static FingerTec Connector(ConnectionType connectionType, int deviceId)
        {
            IFingerDeviceService _deviceService = DependencyResolver.Current.GetService<IFingerDeviceService>();
            FingerDevice entity = _deviceService.FindById(deviceId);
            FingerTec fingerTec = new FingerTec(connectionType, entity.ModelName, entity.IP, int.Parse(entity.DeviceInnerId), int.Parse(entity.DeviceInnerId), int.Parse(entity.CommKey), int.Parse(entity.PortNo), 0);
            return fingerTec;
        }

        public static FingerTec Connector(ConnectionType connectionType, string deviceName, int deviceInnerId, string iP, int portNo, int commKey)
        {
            FingerTec fingerTec = new FingerTec(connectionType, deviceName, iP, deviceInnerId, deviceInnerId, commKey, portNo, 0);
            return fingerTec;
        }
    }
}