using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IFingerDeviceService
    {
        List<FingerDevice> GetList { get; }

        int Count();
        int Count(string search);
        void Create(FingerDevice entity);
        void Delete(FingerDevice entity);
        List<FingerDevice> FilterData(int start, int lenght, string search);
        FingerDevice FindById(int id);
        List<FingerDevice> List();
        void Update(FingerDevice entity);
    }
}