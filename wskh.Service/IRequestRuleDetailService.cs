using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IRequestRuleDetailService
    {
        List<RequestRuleDetail> GetList { get; }

        int Count();
        int Count(string search);
        void Create(RequestRuleDetail entity);
        void Delete(RequestRuleDetail entity);
        List<RequestRuleDetail> FilterData(int start, int lenght, string search);
        RequestRuleDetail FindById(int id);
        List<RequestRuleDetail> List();
        void Update(RequestRuleDetail entity);
    }
}