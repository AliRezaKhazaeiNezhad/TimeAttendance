using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IRawLogService
    {
        List<RawLog> GetList { get; }

        int Count();
        int Count(string search);
        void Create(RawLog entity);
        void Delete(RawLog entity);
        RawLog FindById(int id);
        List<RawLog> List();
        void Update(RawLog entity);
        List<RawLog> FilterData(int start, int lenght, string search);
    }
}