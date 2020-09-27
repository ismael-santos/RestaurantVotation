using System.Collections.Generic;

namespace RestaurantVotation.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        void Save(T entidade);

        void Delete(T entidade);

        IEnumerable<T> GetAll();

        void ClearAll();

        T GetByID(int id);
    }
}
