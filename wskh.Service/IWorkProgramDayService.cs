using System.Collections.Generic;
using TimeAttendance.Core;
using wskh.Core.Enumerator;

namespace wskh.Service
{
    public interface IWorkProgramDayService
    {
        List<WorkProgramDay> GetList { get; }

        int Count();
        int Count(WorkType type);
        void Create(WorkProgramDay entity);
        void Delete(WorkProgramDay entity);
        List<WorkProgramDay> FilterData(int start, int lenght, WorkType type);
        WorkProgramDay FindById(int id);
        List<WorkProgramDay> List(WorkType type);
        void Update(WorkProgramDay entity);
        void Dispose();
    }
}