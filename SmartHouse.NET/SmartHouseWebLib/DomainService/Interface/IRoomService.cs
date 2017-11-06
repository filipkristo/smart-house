using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.DomainService.Interface
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetAsync(int Id);
        Task<int> Insert(Room model);
        Task<int> Update(Room model);
        Task<int> Delete(Room model);
    }
}
