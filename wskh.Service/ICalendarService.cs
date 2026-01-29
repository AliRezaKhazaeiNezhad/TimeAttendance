using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ICalendarService
    {
        List<Calendar> GetList { get; }

        int Count();
        int Count(string search);
        void Create(Calendar entity);
        void Delete(Calendar entity);
        List<Calendar> FilterData(int start, int lenght, string search);
        Calendar FindById(int id);
        List<Calendar> List();
        void Update(Calendar entity);
    }
}