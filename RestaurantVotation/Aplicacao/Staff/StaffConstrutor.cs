using RestaurantVotation.Repository.Classes;

namespace RestaurantVotation.Service.Staff
{
    public class StaffConstrutor
    {
        public static StaffService InstanceOf()
        {
            return new StaffService(new StaffRepository());
        }
    }
}
