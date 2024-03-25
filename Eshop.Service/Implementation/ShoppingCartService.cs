using Eshop.DomainEntities;
using Eshop.Repository.Interface;
using EShop.Repository.Interface;
using Eshop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<ProductsInOrders> _productInOrderRepository;
        private readonly IRepository<ProductsInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<ProductsInOrders> productInOrderRepository, IRepository<ProductsInShoppingCart> productInShoppingCartRepository, IRepository<EmailMessage> mailRepository, IEmailService emailService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _mailRepository = mailRepository;
            _emailService = emailService;
        }


        public bool deleteProductFromSoppingCart(string userId, Guid productId)
        {
            if (!string.IsNullOrEmpty(userId) && productId != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.ProductsInShoppingCarts.Where(z => z.ProductId.Equals(productId)).FirstOrDefault();

                userShoppingCart.ProductsInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }
        public ShoppingCartDto getShoppingCartInfo(string id)
        {

            var user = this._userRepository.Get(id);
            var shoppingCart = user.UserCart;

            ShoppingCartDto model = new ShoppingCartDto()
            {
                ProductsInShoppingCarts = shoppingCart.ProductsInShoppingCarts ?? new List<ProductsInShoppingCart>(),
                TotalPrice = shoppingCart.ProductsInShoppingCarts.Sum(z => z.Quantity * z.Product.Price)

            };
            return model;
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

            _productInShoppingCartRepository.Insert(itemToAdd);
            return true;

        }

        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userCard = loggedInUser.UserCart;

                EmailMessage message = new EmailMessage();
                message.Subject = "Successfull order";
                message.MailTo = loggedInUser.Email;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    EshopApplicationUserId = Guid.Parse(userId)  
                };
                this._orderRepository.Insert(order);

                List<ProductsInOrders> productInOrders = new List<ProductsInOrders>();

                var result = userCard.ProductsInShoppingCarts.Select(z => new ProductsInOrders
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.Product.Id,
                    Product = z.Product,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var currentItem = result[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.Product.Price;
                    sb.AppendLine(i.ToString() + ". " + currentItem.Product.Name + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Product.Price);
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                message.Content = sb.ToString();


                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.ProductsInShoppingCarts.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(message);
                this._emailService.SendEmailAsync(message);
                return true;
            }

            return false;
        }
    }
}
