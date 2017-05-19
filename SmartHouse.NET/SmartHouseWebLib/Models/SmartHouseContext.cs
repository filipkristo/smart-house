using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Models
{
    public class SmartHouseContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        public DbSet<House> House { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<UserLocation> UserLocation { get; set; }
        public DbSet<TelemetryData> TelemetryData { get; set; }

        public SmartHouseContext()
            : base("SmartHouseConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public static SmartHouseContext Create()
        {
            return new SmartHouseContext();
        }

        public bool IsDetached<TEntity>(TEntity entity) where TEntity : class
        {
            return this.Entry(entity).State == EntityState.Detached;
        }

        public void SetEntityStateModified<TEntity>(TEntity entityToModifie) where TEntity : class
        {
            this.Entry(entityToModifie).State = EntityState.Modified;
        }

        public void SetEntityStateAdded<TEntity>(TEntity entityToModifie) where TEntity : class
        {
            this.Entry(entityToModifie).State = EntityState.Added;
        }

        public async Task<List<T>> ExecuteSqlCommandAsync<T>(string sql, params object[] parameters)
        {
            return await Database.SqlQuery<T>(sql, parameters).ToListAsync();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var message = HandleValidationException(ex);
                throw new DbEntityValidationException(message, ex);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var message = HandleValidationException(ex);
                throw new DbEntityValidationException(message, ex);
            }
        }

        private string HandleValidationException(DbEntityValidationException ex)
        {
            var sb = new StringBuilder();

            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine();
                }
            }

            return $"Entity Validation Failed - errors follow:\n{sb}";
        }
    }
}
