using System.ComponentModel.DataAnnotations;

namespace Eshop.DomainEntities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
