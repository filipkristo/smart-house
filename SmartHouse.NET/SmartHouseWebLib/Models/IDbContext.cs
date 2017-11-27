using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Models
{
    public interface IDbContext : IDisposable
    {
        bool IsDetached<TEntity>(TEntity entity) where TEntity : class;
        void SetEntityStateModified<TEntity>(TEntity entityToModifie) where TEntity : class;
        void SetEntityStateAdded<TEntity>(TEntity entityToModifie) where TEntity : class;
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<List<T>> ExecuteSqlCommandAsync<T>(string sql, params object[] parameters);
    }
}
