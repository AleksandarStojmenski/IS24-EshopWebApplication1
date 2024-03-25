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

namespace EshopWebApplication1.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _shoppingCartService;

        public ShoppingCartsController(IProductService? shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.GetShoppingCartInfo(userId));
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var result = this._shoppingCartService.deleteProductFromSoppingCart(userId, id);
            var result = true;
            if (result)
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
        }

        public Boolean Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result= new Order();// this._shoppingCartService.order(userId);

            return true;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            //var customerService = new CustomerService();
            //var chargeService = new ChargeService();
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var order = this._shoppingCartService.getShoppingCartInfo(userId);

            //var customer = customerService.Create(new CustomerCreateOptions
            //{
            //    Email = stripeEmail,
            //    Source = stripeToken
            //});

            //var charge = chargeService.Create(new ChargeCreateOptions
            //{
            //    Amount = (Convert.ToInt32(order.TotalPrice) * 100),
            //    Description = "EShop Application Payment",
            //    Currency = "usd",
            //    Customer = customer.Id
            //});

            //if (charge.Status == "succeeded")
            //{
            //    var result = this.Order();

            //    if (result)
            //    {
            //        return RedirectToAction("Index", "ShoppingCarts");
            //    }
            //    else
            //    {
            //        return RedirectToAction("Index", "ShoppingCarts");
            //    }
            //}

            return RedirectToAction("Index", "ShoppingCard");
        }
    }

   
}
