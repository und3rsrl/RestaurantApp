namespace RestaurantApp.DataModel.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public double Total { get; set; }

        public Order Order { get; set; }

        public Food Food { get; set; }
    }
}