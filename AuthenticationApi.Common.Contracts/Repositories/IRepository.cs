using System.Collections.Generic;
using System.Linq;

namespace AuthenticationApi.Common.Contracts.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IEnumerable<T> GetAll2();
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}