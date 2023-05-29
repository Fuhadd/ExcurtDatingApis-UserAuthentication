using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<string>? includes = null,
            int limit = 0
            );

        Task<T?> Get(
            Expression<Func<T, bool>> expression,
            List<string>? includes = null
            );

        Task Delete(int id);

        void Update(T entity);

        Task Create(T entity);
    }
}
