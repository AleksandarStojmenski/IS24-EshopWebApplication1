using System.ComponentModel.DataAnnotations;

namespace Eshop.DomainEntities
{
    public class ProductsInOrders : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid OrderId { get; set; }
        public Order? UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
