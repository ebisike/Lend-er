using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Lend_er.Services.Interface
{
    public interface IRepository<T> where T: class
    {
        ICollection<T> GetAll();
        T GetById(string id);
        T GetByIdCreditDebit(Guid id);
        void Insert(T entity);
        void Update(T entity);
        void DeleteCreditDebit(Guid id);
        public ICollection<T> FindAll(Expression<Func<T, bool>> match);
    }
}
