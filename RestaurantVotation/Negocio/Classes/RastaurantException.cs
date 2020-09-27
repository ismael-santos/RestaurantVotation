using System;

namespace RestaurantVotation.Business.Classes
{
    public class RastaurantException : Exception
    {
        public RastaurantException(string message) : base(message) { }
    }

    public class RestaurantDrawException : RastaurantException
    {
        public RestaurantDrawException(string message) : base(message) { }
    }

    public class AlreadyVotedException : RastaurantException
    {
        public AlreadyVotedException(string message) : base(message) { }
    }

    public class VotingClosedException : RastaurantException
    {
        public VotingClosedException(string message) : base(message) { }
    }

    public class RestaurantOfTheWeekException : RastaurantException
    {
        public RestaurantOfTheWeekException(string message) : base(message) { }
    }
}
