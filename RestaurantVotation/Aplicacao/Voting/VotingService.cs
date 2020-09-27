using RestaurantVotation.Domain.Model;
using RestaurantVotation.Repository.Interface;
using System.Collections.Generic;

namespace RestaurantVotation.Service.Voting
{
    public class VotingService
    {
        private readonly IRepository<VotingDTO> _repository;

        internal VotingService(IRepository<VotingDTO> repository)
        {
            _repository = repository;
        }

        public IEnumerable<VotingDTO> GetAll()
        {
            return _repository.GetAll();
        }

        public void Save(VotingDTO voting)
        {
            _repository.Save(voting);
        }

        public void Delete(VotingDTO voting)
        {
            _repository.Delete(voting);
        }

        public void ClearAll()
        {
            _repository.ClearAll();
        }

    }
}
