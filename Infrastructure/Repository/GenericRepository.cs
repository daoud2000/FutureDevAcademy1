using Application.Repository.Interface;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class

        
    {
        private readonly FutureDevDbContext _dbContext;
        public GenericRepository(FutureDevDbContext dbContext) { 
        
        _dbContext = dbContext;
        
        
        
        
        
        }
        public async Task Delete(T entity)
        {
              _dbContext.Set<T>().Remove(entity);
        }

        public async  Task DeleteRange(List<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
        }

        public  IQueryable<T> GetAll() 
        {
            return _dbContext.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
          return  await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task Insert(T entity)
        {
           await _dbContext.Set<T>().AddAsync(entity);
        }

        public async  Task InsertRange(List<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async void   Update(T entity)
        {
             _dbContext.Set<T>().Update(entity);
        }
    }
}
