using Eshop.DomainEntities;
using Eshop.Repository.Interface;
using EShop.Repository.Interface;
using Eshop.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.DomainEntities;
using System.Security.Claims;

namespace Eshop.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductsInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger, IRepository<ProductsInShoppingCart> productInShoppingCartRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _logger = logger;
        }

        public bool AddToShoppingCart(Guid id,string userId)
        {
            try
            {
                var user = this._userRepository.Get(userId);
                var shoppingCart = user.UserCart;
                if (shoppingCart?.ProductsInShoppingCarts == null)
                    shoppingCart.ProductsInShoppingCarts = new List<ProductsInShoppingCart>();
                var product = _productRepository.Get(id);

                return true;
            }
            catch (Exception ex) {
                return false;
            };

        }

        public bool AddToShoppingCart(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool AddToShoppingConfirmed(ProductsInShoppingCart model, string userId)
        {
            var user = this._userRepository.Get(userId);
            var shoppingCart = user.UserCart;
            ProductsInShoppingCart itemToAdd = new ProductsInShoppingCart
            {
                Id = Guid.NewGuid(),
                Product = model.Product,
                ProductId = model.ProductId,
                ShoppingCart = shoppingCart,
                ShoppingCartId = shoppingCart.Id,
                Quantity = model.Quantity
            };

            this._productInShoppingCartRepository.Insert(itemToAdd);
            return true;

        }

        public void CreateNewProduct(Product p)
        {

            _productRepository.Insert(p);
            
        }

        public void DeleteProduct(Guid id)
        {
            var product = _productRepository.Get(id);
            _productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return _productRepository.Get(id);
        }

        public ShoppingCartDto GetShoppingCartInfo(string id)
        {

            var user = this._userRepository.Get(id);
            var shoppingCart = user.UserCart;

            ShoppingCartDto model = new ShoppingCartDto()
            {
                ProductsInShoppingCarts = shoppingCart.ProductsInShoppingCarts?? new List<ProductsInShoppingCart>(),
                TotalPrice = shoppingCart.ProductsInShoppingCarts.Sum(z => z.Quantity * z.Product.Price)
                
            };
            return model;
        }

        public void UpdeteExistingProduct(Product p)
        {
            _productRepository.Update(p);
        }
    }
}
