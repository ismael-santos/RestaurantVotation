using RestaurantVotation.Domain.Model;
using RestaurantVotation.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantVotation.Repository.Classes
{
    public class StaffRepository : IRepository<StaffDTO>
    {
        private List<StaffDTO> _colaborador = new List<StaffDTO>();

        public StaffRepository()
        {
            // Mock do colaborador objeto pode ser implementado em um banco (DbContext)
            _colaborador.AddRange
               (
                   new List<StaffDTO>
                   {
                        new StaffDTO{ Id = 1, StaffName = "João Neves" },
                        new StaffDTO{ Id = 2, StaffName = "Maria Augusta" },
                        new StaffDTO{ Id = 3, StaffName = "Carlos Silva" },
                        new StaffDTO{ Id = 4, StaffName = "Verônica Paz" },
                        new StaffDTO{ Id = 5, StaffName = "Ismael Santos" },
                        new StaffDTO{ Id = 6, StaffName = "Alice Nunes" }
                   }
               );
        }

        public IEnumerable<StaffDTO> GetAll()
        {
            return _colaborador;
        }

        public void Save(StaffDTO entidade)
        {
            _colaborador.Add(entidade);
        }

        public void Delete(StaffDTO entidade)
        {
            _colaborador.Remove(entidade);
        }

        public void ClearAll()
        {
            _colaborador = new List<StaffDTO>();
        }

        public StaffDTO GetByID(int id)
        {
            return _colaborador.First(x => x.Id == id);
        }
    }
}
