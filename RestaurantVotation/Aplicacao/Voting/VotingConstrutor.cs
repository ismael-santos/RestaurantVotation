using RestaurantVotation.Repository.Classes;

namespace RestaurantVotation.Service.Voting
{
    public class VotingConstrutor
    {
        public static VotingService InstanceOf()
        {
            return new VotingService(new VotingRepository());
        }
    }
}
