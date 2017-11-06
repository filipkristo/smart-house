using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHouseWebLib.Models;
using SmartHouseWebLib.Repository;

namespace SmartHouseWebLib.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext dbContext;

        private IGenericRepository<House> houseRepository;
        private IGenericRepository<UserLocation> userLocationRepository;
        private IGenericRepository<ApplicationUser> applicationUserRepository;
        private IGenericRepository<Room> roomRepository;
        private IGenericRepository<TelemetryData> telemetryDataRepository;

        public IGenericRepository<House> HouseRepository => houseRepository = (houseRepository ?? new GenericRepository<House>(dbContext));
        public IGenericRepository<UserLocation> UserLocationRepository => userLocationRepository = (userLocationRepository ?? new GenericRepository<UserLocation>(dbContext));
        public IGenericRepository<ApplicationUser> ApplicationUserRepository => applicationUserRepository = (applicationUserRepository ?? new GenericRepository<ApplicationUser>(dbContext));
        public IGenericRepository<Room> RoomRepository => roomRepository = (roomRepository ?? new GenericRepository<Room>(dbContext));
        public IGenericRepository<TelemetryData> TelemetryDataRepository => telemetryDataRepository = (telemetryDataRepository ?? new GenericRepository<TelemetryData>(dbContext));

        public UnitOfWork(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Methods

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public async Task<List<T>> ExecuteSqlCommandAsync<T>(string sql, params object[] parameters)
        {
            return await dbContext.ExecuteSqlCommandAsync<T>(sql, parameters);
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        #endregion
    }
}
