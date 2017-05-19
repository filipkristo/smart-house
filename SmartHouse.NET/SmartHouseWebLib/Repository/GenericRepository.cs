using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SmartHouseWebLib.Utils;

namespace SmartHouseWebLib.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        private IDbContext context;
        private DbSet<TEntity> dbSet;

        #endregion

        #region Constructors

        public GenericRepository(IDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        #endregion

        #region Methods

        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return dbSet.CountAsync(filter);

            return dbSet.CountAsync();
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return query.MaxAsync(selector);
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return dbSet.AnyAsync(filter);

            return dbSet.AnyAsync();
        }

        protected virtual IQueryable<TEntity> GetQueryPageing(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int skip = 0, int take = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            var includePropertiesArray = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var includeProperty in includePropertiesArray)
                query = query.Include(includeProperty);

            query = orderBy != null ? orderBy(query) : query;

            if (take > 0)
            {
                query = skip > 0 ? query.Skip(skip) : query;

                query = query.Take(take);
            }

            return query;
        }

        protected virtual IQueryable<TEntity> GetQueryPageing(
            Expression<Func<TEntity, bool>> filter,
            string orderByColumn = null,
            bool asc = true,
            string includeProperties = "",
            int skip = 0, int take = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            var includePropertiesArray = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var includeProperty in includePropertiesArray)
                query = query.Include(includeProperty);

            query = orderByColumn != null ? query.OrderBy(orderByColumn, asc) : query;

            if (take > 0)
            {
                query = skip > 0 ? query.Skip(skip) : query;

                query = query.Take(take);
            }

            return query;
        }

        private Func<IQueryable<T>, IOrderedQueryable<T>> GetOrder<T>(IQueryable<T> source, string orderByProperty,
                          bool asc)
        {
            string command = asc ? "OrderBy" : "OrderByDescending";
            var type = typeof(T);
            var property = type.GetProperty(orderByProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));

            var finalLambda = Expression.Lambda(resultExpression, parameter);
            return (Func<IQueryable<T>, IOrderedQueryable<T>>)finalLambda.Compile();
        }

        public virtual Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int skip = 0, int take = 0)
        {
            return GetQueryPageing(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAsyncSort(
            Expression<Func<TEntity, bool>> filter = null,
            string orderByColumn = null,
            bool asc = true,
            string includeProperties = "", int skip = 0, int take = 0)
        {
            return GetQueryPageing(filter, orderByColumn, asc, includeProperties, skip, take).ToListAsync();
        }

        protected virtual IQueryable<TEntity> GetQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            var includePropertiesArray = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var includeProperty in includePropertiesArray)
                query = query.Include(includeProperty);

            return orderBy != null ? orderBy(query) : query;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return GetQuery(filter, orderBy, includeProperties).AsEnumerable();
        }

        public virtual Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return GetQuery(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public Task<List<TModel>> GetModelAsync<TModel>(
            Expression<Func<TEntity, TModel>> select,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int skip = 0, int take = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = orderBy != null ? orderBy(query) : query;

            if (take > 0)
            {
                query = skip > 0 ? query.Skip(skip) : query;

                query = query.Take(take);
            }

            return query.Select(select).ToListAsync();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual Task<TEntity> GetByIDAsync(object id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity item = dbSet.Find(id);
            Delete(item);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (IsDetached(entityToDelete))
                dbSet.Attach(entityToDelete);

            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            SetEntityStateModified(entityToUpdate);
        }

        public virtual void SetEntityStateModified(TEntity entityToModifie)
        {
            context.SetEntityStateModified<TEntity>(entityToModifie);
        }

        public void SetEntityStateAdded(TEntity entityToAdd)
        {
            context.SetEntityStateAdded<TEntity>(entityToAdd);
        }

        public virtual bool IsDetached(TEntity entityToCheck)
        {
            return context.IsDetached(entityToCheck);
        }

        #endregion

    }
}
