using RestaurantVotation.Domain.Model;
using System.Collections.Generic;

namespace RestaurantVotation.Business.Interface
{
    public interface IVoting
    {
        void Vote(VotingDTO voting);

        void FinishingVoting();

        IEnumerable<VotingDTO> ListVoting();

        IEnumerable<RestaurantDTO> ListRestaurante();

        IEnumerable<StaffDTO> ListStaff();

        IEnumerable<VotingDTO> GetPartial();

        IEnumerable<VotingDTO> ListWinnersOfWeek();

        void RestartVoting();

        string GetWinnerRestaurant();

        void RestartVotingLastWeek(VotingDTO voting = null);
    }
}
