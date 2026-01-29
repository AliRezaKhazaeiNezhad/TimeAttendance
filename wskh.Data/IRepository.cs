using System.Collections.Generic;
using System.Data.Entity;

namespace wskh.Data
{
    public interface IRepository<T> where T : class
    {
        IDbSet<T> Context { get; }

        int Count();
        void Create(T entity);
        void Delete(T entity);
        void Dispose();
        T FindById(int id);
        List<T> List();
        void Update(T entity);
    }
}