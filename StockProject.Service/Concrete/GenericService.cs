using StockProject.Entities.Entities;
using StockProject.Repositories.Abstract;
using StockProject.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Service.Concrete
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> repository;

        public GenericService(IGenericRepository<T> repository)
        {
            this.repository = repository;
        }

        public bool Activate(int id)
        {
            if (id == 0 || GetById(id) == null)
                return false;
            else
                return repository.Activate(id);
        }

        public bool Add(T item)
        {
           return repository.Add(item);
        }

        public bool Add(List<T> items)
        {
            return repository.Add(items);
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            if (exp != null)
                return repository.Any(exp);
            else 
                return false;
        }

        public List<T> GetActive()
        {
            return repository.GetActive();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            return repository.GetActive(includes);
        }

        public List<T> GetAll()
        {
            return repository.GetAll();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return repository.GetAll(includes);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            return repository.GetAll(exp,includes);
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return repository.GetByDefault(exp);
        }

        public T GetById(int id)
        {
            return repository.GetById(id);
        }

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            return repository.GetById(id,includes);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
          return repository.GetDefault(exp);
        }

        public bool Remove(T item)
        {
            if (item == null)
                return false;
            else
                return repository.Remove(item);
        }

        public bool Remove(int id)
        {
            if(id<=0)
                return false;
            else
                return repository.Remove(id);
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            return repository.RemoveAll(exp);
        }

        public bool Update(T item)
        {

            if (item == null)
                return false;
            else
                return repository.Update(item);
        }
    }
}
