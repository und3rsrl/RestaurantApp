namespace RestaurantApp.BusinessEntities.DTOs.Order
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public double Total { get; set; }
    }
}
