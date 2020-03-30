using Lend_er.Data;
using Lend_er.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lend_er.Services.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LenderDbContext _lender;

        public Repository(LenderDbContext lenderDbContext)
        {
            _lender = lenderDbContext;
        }
        public void Delete(string id)
        {
            var entity = _lender.Set<T>().Find(id);
            if (entity != null)
            {
                _lender.Set<T>().Remove(entity);
                 _lender.SaveChanges();
            }
        }

        public void DeleteCreditDebit(Guid id)
        {
            var entity = _lender.Set<T>().Find(id);
            if (entity != null)
            {
                _lender.Set<T>().Remove(entity);
                _lender.SaveChanges();
            }
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _lender.Set<T>().Where(match).ToList();
        }

        public ICollection<T> GetAll()
        {
            return _lender.Set<T>().ToList();
        }

        public T GetById(string id)
        {
            return _lender.Set<T>().Find(id);
        }

        public T GetByIdCreditDebit(Guid id)
        {
            return _lender.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            _lender.Set<T>().Add(entity);
            _lender.SaveChanges();
        }

        public void Update(T entity)
        {
            _lender.Set<T>().Update(entity);
            _lender.SaveChanges();
        }
    }
}
