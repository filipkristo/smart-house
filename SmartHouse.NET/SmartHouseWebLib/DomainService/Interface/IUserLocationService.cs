using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.DomainService.Interface
{
    public interface IUserLocationService
    {
        Task<IEnumerable<UserLocation>> GetAllAsync();
        Task<IEnumerable<UserLocation>> GetAllDescAsync();
        Task<UserLocation> GetAsync(int Id);        
        Task<int> Insert(UserLocation model);
        Task<int> Update(UserLocation model);
        Task<int> Delete(UserLocation model);
    }
}
