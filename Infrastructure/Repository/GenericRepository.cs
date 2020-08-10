using Core.Entity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
           _context.Set<T>().Remove(entity);
        }

        public async Task<T> GetById(object id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;

        }
        public async Task<T> Insert(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        //public void Update(T entity)
        //{
        //        _context.Set<T>().Attach(entity);
        //   _context.Entry(entity).State = EntityState.Modified;
        //}

    }
}
