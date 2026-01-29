using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ISpecialDayGroupingService
    {
        List<SpecialDayGrouping> GetList { get; }

        int Count();
        int Count(string search);
        void Create(SpecialDayGrouping entity);
        void Delete(SpecialDayGrouping entity);
        List<SpecialDayGrouping> FilterData(int start, int lenght, string search);
        SpecialDayGrouping FindById(int id);
        List<SpecialDayGrouping> List();
        void Update(SpecialDayGrouping entity);
    }
}