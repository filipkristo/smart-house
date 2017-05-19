using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageing = 0, int count = 0);
        Task<List<TEntity>> GetAsyncSort(Expression<Func<TEntity, bool>> filter = null, string orderByColumn = null, bool asc = true, string includeProperties = "", int skip = 0, int take = 0);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<List<TModel>> GetModelAsync<TModel>(Expression<Func<TEntity, TModel>> select, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int pageing = 0, int count = 0);
        TEntity GetByID(object id);
        Task<TEntity> GetByIDAsync(object id);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, Func<System.Linq.IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void SetEntityStateModified(TEntity entityToModifie);
        void SetEntityStateAdded(TEntity entityToAdd);
        bool IsDetached(TEntity entityToCheck);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, TResult>> selector = null);
    }
}
