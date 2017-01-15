using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            this.repository = repo;
            this.orderProcessor = proc;
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == productID);

            if(product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == productID);

            if(product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult DecrementProductInCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == productID);

            if (product != null)
            {
                cart.DecrementItem(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        // GET: Cart
        public ViewResult Index(Cart cart, string returnUrl)
        {
            if(cart.Lines.Count() == 0)
            {
                return View("EmptyCart", new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
            }
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Your cart is empty!");
            }

            if(ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}