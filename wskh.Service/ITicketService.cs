using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ITicketService
    {
        List<Ticket> GetList { get; }

        int Count();
        int Count(string search);
        void Create(Ticket entity);
        void Delete(Ticket entity);
        List<Ticket> FilterData(int start, int lenght, string search);
        Ticket FindById(int id);
        List<Ticket> List();
        void Update(Ticket entity);
    }
}