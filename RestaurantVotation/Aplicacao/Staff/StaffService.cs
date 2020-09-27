using RestaurantVotation.Domain.Model;
using RestaurantVotation.Repository.Interface;
using System.Collections.Generic;

namespace RestaurantVotation.Service.Staff
{
    public class StaffService
    {
        private readonly IRepository<StaffDTO> _repository;

        internal StaffService(IRepository<StaffDTO> repository)
        {
            _repository = repository;
        }

        public IEnumerable<StaffDTO> GetAll()
        {
            return _repository.GetAll();
        }

        public void Save(StaffDTO staff)
        {
            _repository.Save(staff);
        }

        public void Delete(StaffDTO staff)
        {
            _repository.Delete(staff);
        }

        public StaffDTO GetByID(int id)
        {
            return _repository.GetByID(id);
        }
    }
}
