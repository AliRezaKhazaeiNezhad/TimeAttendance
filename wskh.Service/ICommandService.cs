using System.Collections.Generic;
using System.Data;
using TimeAttendance.Core;
using wskh.Core.Enumerator;

namespace wskh.Service
{
    public interface ICommandService
    {
        List<Command> GetList { get; }

        int Count(CommandCategory cat);
        void Create(Command entity);
        void Delete(Command entity);
        List<Command> FilterData(int start, int lenght, CommandCategory cat);
        Command FindById(int id);
        List<Command> List();
        void Update(Command entity);
        void Dispose();
    }
}