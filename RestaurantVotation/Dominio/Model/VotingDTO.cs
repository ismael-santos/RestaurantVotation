using System;

namespace RestaurantVotation.Domain.Model
{
    public class VotingDTO
    {
        public int Id { get; set; }

        public StaffDTO Staff { get; set; }

        public RestaurantDTO Restaurant { get; set; }

        public DateTime DateVoting { get; set; }

        public int NumberOfVotes { get; set; }

        public bool IsFinished { get; set; } = false;
    }
}
