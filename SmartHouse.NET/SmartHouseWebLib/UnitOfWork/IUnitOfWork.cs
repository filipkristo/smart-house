using SmartHouseWebLib.Models;
using SmartHouseWebLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<House> HouseRepository { get; }
        IGenericRepository<UserLocation> UserLocationRepository { get; }
        IGenericRepository<ApplicationUser> ApplicationUserRepository { get; }
        IGenericRepository<Room> RoomRepository { get; }
        IGenericRepository<TelemetryData> TelemetryDataRepository { get; }

        Task<List<T>> ExecuteSqlCommandAsync<T>(string sql, params object[] parameters);
        int Save();
        Task<int> SaveAsync();
    }
}
