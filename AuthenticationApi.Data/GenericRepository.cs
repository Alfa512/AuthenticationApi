using System.Collections.Generic;
using System.Linq;
using AuthenticationApi.Common.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Data
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Context;

        protected GenericRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public IEnumerable<T> GetAll2()
        {
            return Context.Set<T>().ToList();
        }

        public T Create(T entity)
        {
            var nEntity = Context.Set<T>().Add(entity).Entity;
            Context.SaveChanges();
            return nEntity;
        }

        public T Update(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Context.Set<T>().Attach(entity);
                entry = Context.Entry(entity);
            }

            entry.State = EntityState.Modified;

            return entity;
        }

        public T Delete(T entity)
        {
            return Context.Set<T>().Remove(entity).Entity;
        }
    }
}