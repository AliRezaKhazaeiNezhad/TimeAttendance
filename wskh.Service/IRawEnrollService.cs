using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IRawEnrollService
    {
        List<RawEnroll> GetList { get; }

        int Count();
        int Count(string search);
        void Create(RawEnroll entity);
        void Delete(RawEnroll entity);
        RawEnroll FindById(int id);
        List<RawEnroll> List();
        void Update(RawEnroll entity);
    }
}