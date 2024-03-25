using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EshopWebApplication1.Data;
using Eshop.DomainEntities;
using System.Security.Claims;
using Eshop.Service.Interface;
using Stripe;

namespace EshopWebApplication1.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService? shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult Delete(Guid id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.deleteProductFromSoppingCart(userId, id);
            if (result)
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
        }

        public Boolean OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.order(userId);

            return result;
        }
        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            StripeConfiguration.ApiKey = "sk_test_51Io84IHBiOcGzrvu4sxX66rTHq8r5nxIxRiJPbOHB4NwVJOE1jSlxgYe741ITs024uXhtpBFtxm3RoCZc3kafocC00IhvgxkL0";
            var order = this._shoppingCartService.getShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "EShop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.OrderNow();
                return RedirectToAction("Index", "ShoppingCarts");
            }

            return RedirectToAction("Index", "ShoppingCarts");
        }

    }

   
}
