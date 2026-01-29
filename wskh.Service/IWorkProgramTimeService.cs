using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IWorkProgramTimeService
    {
        List<WorkProgramTime> GetList { get; }

        int Count();
        int Count(string search);
        void Create(WorkProgramTime entity);
        void Delete(WorkProgramTime entity);
        List<WorkProgramTime> FilterData(int start, int lenght, string search);
        WorkProgramTime FindById(int id);
        List<WorkProgramTime> List();
        void Update(WorkProgramTime entity);
    }
}