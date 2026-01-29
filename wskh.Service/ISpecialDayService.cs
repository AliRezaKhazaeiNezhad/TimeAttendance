using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ISpecialDayService
    {
        List<SpecialDay> GetList { get; }

        int Count();
        int Count(string search);
        void Create(SpecialDay entity);
        void Delete(SpecialDay entity);
        List<SpecialDay> FilterData(int start, int lenght, string search);
        SpecialDay FindById(int id);
        List<SpecialDay> List();
        void Update(SpecialDay entity);
        void Dispose();
    }
}