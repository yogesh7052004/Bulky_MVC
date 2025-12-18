using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null); //for writing Linq operations
        void Add(T entity);
        //void Update(T entity); we dont want to inclide this sometimes we may have different logic for updating such as only specific fields like that so recommended to not put this here.
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
