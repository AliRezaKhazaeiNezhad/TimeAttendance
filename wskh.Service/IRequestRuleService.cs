using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IRequestRuleService
    {
        List<RequestRule> GetList { get; }

        int Count();
        int Count(string search);
        void Create(RequestRule entity);
        void Delete(RequestRule entity);
        List<RequestRule> FilterData(int start, int lenght, string search);
        RequestRule FindById(int id);
        List<RequestRule> List();
        void Update(RequestRule entity);
    }
}