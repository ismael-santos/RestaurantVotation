using RestaurantVotation.Domain.Model;
using RestaurantVotation.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantVotation.Repository.Classes
{
    public class RestaurantRepository : IRepository<RestaurantDTO>
    {
        private List<RestaurantDTO> _restaurante = new List<RestaurantDTO>();

        public RestaurantRepository()
        {
            // Mock do restaurante objeto pode ser implementado em um banco (DbContext)
            _restaurante.AddRange
                (
                    new List<RestaurantDTO>
                    {
                        new RestaurantDTO { Id = 1, RestaurantName = "Silva Lanches"},
                        new RestaurantDTO { Id = 2, RestaurantName = "Panorama"},
                        new RestaurantDTO { Id = 3, RestaurantName = "Espaço 32"},
                        new RestaurantDTO { Id = 4, RestaurantName = "Maza"},
                        new RestaurantDTO { Id = 5, RestaurantName = "Palatus"}
                    }
                );
        }

        public void Delete(RestaurantDTO entidade)
        {
            _restaurante.Remove(entidade);
        }

        public RestaurantDTO GetByID(int id)
        {
            return _restaurante.First(x => x.Id == id);
        }

        public void ClearAll()
        {
            _restaurante = new List<RestaurantDTO>();
        }

        public IEnumerable<RestaurantDTO> GetAll()
        {

            return _restaurante;
        }

        public void Save(RestaurantDTO entidade)
        {
            _restaurante.Add(entidade);
        }
    }
}
