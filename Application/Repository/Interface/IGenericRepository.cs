using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Interface
{
    public interface IGenericRepository <T> where T   : class
    {
        Task  Insert(T entity);
        Task Delete(T entity);
        Task InsertRange (List<T> entity);
        Task DeleteRange(List<T> entity);

         void Update(T entity);
        Task <int>  SaveChanges();
        Task<T> GetById(int id);
         IQueryable<T> GetAll();

    }
}
