using System.Collections.Generic;
using wskh.Core;

namespace wskh.Service
{
    public interface IEnrollService
    {
        List<Enroll> GetList { get; }

        int Count(int deviceId);
        int Count(string search, int deviceId);
        void Create(Enroll entity);
        void Delete(Enroll entity);
        void Dispose();
        List<Enroll> FilterData(int start, int lenght, string search, int deviceId);
        Enroll FindById(int id);
        List<Enroll> List();
        void Update(Enroll entity);
    }
}