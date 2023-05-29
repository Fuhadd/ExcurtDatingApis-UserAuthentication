using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;

        public Repository(DatabaseContext context)
        {
            _context = context;

            _db = _context.Set<T>();

        }

        public async Task Create(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            if(entity != null)
            {
                _db.Remove(entity);
            }
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression, List<string>? includes = null)
        {
            IQueryable<T> query = _db.AsQueryable();

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            var result = await query.AsNoTracking().FirstOrDefaultAsync(expression);

            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? includes = null, int limit = 0)
        {
            IQueryable<T> query = _db;


            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            var result = await query.AsNoTracking().ToListAsync();

            return result;
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

        }
    }
}
