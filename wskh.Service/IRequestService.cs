using System.Collections.Generic;
using TimeAttendance.Core;
using wskh.Core.Enumerator;

namespace wskh.Service
{
    public interface IRequestService
    {
        List<Request> GetList { get; }

        int Count();
        int Count(string search, RequestType type, RequestState state, string userId);
        void Create(Request entity);
        void Delete(Request entity);
        List<Request> FilterData(int start, int lenght, string search, RequestType type, RequestState state, string userId);
        Request FindById(int id);
        List<Request> List();
        void Update(Request entity);
    }
}