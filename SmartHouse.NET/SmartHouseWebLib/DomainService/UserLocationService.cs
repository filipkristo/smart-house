using SmartHouseWebLib.DomainService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHouseWebLib.Models;
using SmartHouseWebLib.UnitOfWork;

namespace SmartHouseWebLib.DomainService
{
    public class UserLocationService : IUserLocationService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserLocationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Delete(UserLocation model)
        {
            unitOfWork.UserLocationRepository.Delete(model);
            return await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<UserLocation>> GetAllAsync()
        {
            return await unitOfWork.UserLocationRepository.GetAsync();
        }

        public async Task<IEnumerable<UserLocation>> GetAllDescAsync()
        {
            return await unitOfWork.UserLocationRepository.GetAsync(orderBy: x => x.OrderByDescending(o => o.UpdatedUtc).ThenByDescending(o => o.Id));
        }

        public async Task<UserLocation> GetAsync(int Id)
        {
            return await unitOfWork.UserLocationRepository.GetByIDAsync(Id);
        }

        public async Task<int> Insert(UserLocation model)
        {
            unitOfWork.UserLocationRepository.Insert(model);
            return await unitOfWork.SaveAsync();
        }

        public async Task<int> Update(UserLocation model)
        {
            unitOfWork.UserLocationRepository.Update(model);
            return await unitOfWork.SaveAsync();
        }
    }
}
