namespace ELA.Common.DTOs.Order
{
    public class CreateOrderLine
    {
        public CreateOrderLine() { }
        public CreateOrderLine(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}