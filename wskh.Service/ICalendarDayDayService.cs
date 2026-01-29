using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ICalendarDayService
    {
        List<CalendarDay> GetList { get; }

        int Count();
        int Count(string search);
        void Create(CalendarDay entity);
        void Delete(CalendarDay entity);
        List<CalendarDay> FilterData(int start, int lenght, string search);
        CalendarDay FindById(int id);
        List<CalendarDay> List();
        void Update(CalendarDay entity);
    }
}