using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IPatchHistoryService
    {
        List<PatchHistory> GetList { get; }

        int Count();
        int Count(string search);
        void Create(PatchHistory entity);
        void Delete(PatchHistory entity);
        List<PatchHistory> FilterData(int start, int lenght, string search);
        PatchHistory FindById(int id);
        List<PatchHistory> List();
        void Update(PatchHistory entity);
    }
}