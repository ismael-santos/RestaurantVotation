using RestaurantVotation.Domain.Model;
using RestaurantVotation.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantVotation.Repository.Classes
{
    public class VotingRepository : IRepository<VotingDTO>
    {
        private static List<VotingDTO> _votacao = new List<VotingDTO>();

        public void Delete(VotingDTO entidade)
        {
            _votacao.Remove(entidade);
        }

        public IEnumerable<VotingDTO> GetAll()
        {
            return _votacao;
        }

        public void Save(VotingDTO entidade)
        {
            _votacao.Add(entidade);
        }

        public void ClearAll()
        {
            _votacao = new List<VotingDTO>();
        }

        public VotingDTO GetByID(int id)
        {
            return _votacao.First(x => x.Id == id);
        }
    }
}
