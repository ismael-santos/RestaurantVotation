using RestaurantVotation.Domain.Model;
using RestaurantVotation.Repository.Interface;
using System.Collections.Generic;

namespace RestaurantVotation.Service.Restaurant
{
    public class RestaurantService
    {
        private readonly IRepository<RestaurantDTO> _repository;

        internal RestaurantService(IRepository<RestaurantDTO> repository)
        {
            _repository = repository;
        }

        public IEnumerable<RestaurantDTO> GetAll()
        {
            return _repository.GetAll();
        }

        public void Save(RestaurantDTO restaurant)
        {
            _repository.Save(restaurant);
        }

        public void Delete(RestaurantDTO restaurant)
        {
            _repository.Delete(restaurant);
        }

        public RestaurantDTO GetByID(int id)
        {
            return _repository.GetByID(id);
        }
    }
}
