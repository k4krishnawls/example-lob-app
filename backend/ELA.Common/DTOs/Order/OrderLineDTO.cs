namespace ELA.Common.DTOs.Order
{
    public class OrderLineDTO
    {
        public OrderLineDTO() { }
        public OrderLineDTO(CreateOrderLine create)
        {
            // Id is set from DB
            ProductId = create.ProductId;
            Quantity = create.Quantity;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}