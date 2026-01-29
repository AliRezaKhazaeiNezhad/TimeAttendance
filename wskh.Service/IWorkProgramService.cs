using System.Collections.Generic;
using TimeAttendance.Core;
using wskh.Core.Enumerator;

namespace wskh.Service
{
    public interface IWorkProgramService
    {
        List<WorkProgram> GetList { get; }

        int Count();
        int Count(string search, WorkProgramType type);
        void Create(WorkProgram entity);
        void Delete(WorkProgram entity);
        List<WorkProgram> FilterData(int start, int lenght, string search, WorkProgramType type);
        WorkProgram FindById(int id);
        List<WorkProgram> List(WorkProgramType type);
        void Update(WorkProgram entity);
    }
}