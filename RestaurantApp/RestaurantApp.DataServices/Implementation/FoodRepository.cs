using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;

namespace RestaurantApp.DataServices.Implementation
{
    public class FoodRepository : GenericRepository<Food>, IFoodRepository
    {
        public FoodRepository(Entities dbContext) : base(dbContext)
        {
        }

        protected override string TableName => "Foods";
    }
}
