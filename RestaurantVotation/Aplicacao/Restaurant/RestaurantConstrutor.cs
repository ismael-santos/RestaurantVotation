using RestaurantVotation.Repository.Classes;

namespace RestaurantVotation.Service.Restaurant
{
    public class RestaurantConstrutor
    {
        public static RestaurantService InstanceOf()
        {
            return new RestaurantService(new RestaurantRepository());
        }
    }
}
