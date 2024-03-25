using Eshop.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteProductFromSoppingCart(string userId, Guid productId);
        bool order(string userId);
        bool AddToShoppingConfirmed(ProductsInShoppingCart model, string userId);
    }
}
