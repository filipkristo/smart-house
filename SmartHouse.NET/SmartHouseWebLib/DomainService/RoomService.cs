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
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Delete(Room model)
        {
            unitOfWork.RoomRepository.Delete(model);
            return await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await unitOfWork.RoomRepository.GetAsync();
        }

        public async Task<Room> GetAsync(int Id)
        {
            return await unitOfWork.RoomRepository.GetByIDAsync(Id);
        }

        public async Task<int> Insert(Room model)
        {
            unitOfWork.RoomRepository.Insert(model);
            return await unitOfWork.SaveAsync();
        }

        public async Task<int> Update(Room model)
        {
            unitOfWork.RoomRepository.Update(model);
            return await unitOfWork.SaveAsync();
        }
    }
}
