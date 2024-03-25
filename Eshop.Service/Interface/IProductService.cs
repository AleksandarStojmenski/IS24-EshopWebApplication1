using Eshop.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Service.Interface
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetDetailsForProduct(Guid? id);
        void CreateNewProduct(Product p);
        void UpdeteExistingProduct(Product p);
        ShoppingCartDto GetShoppingCartInfo(string? id);
        void DeleteProduct(Guid id);
        bool AddToShoppingCart(Guid id);
        bool AddToShoppingConfirmed(ProductsInShoppingCart model, string userId);
    }
}
