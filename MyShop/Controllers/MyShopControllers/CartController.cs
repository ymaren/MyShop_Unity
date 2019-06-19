
using MyShop.Models;
using MyShop.Models.interfaces.Repositories;
using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;


namespace StoreWeb.Controllers
{
    public class CartController : Controller
    {
        
        Cart cart;
       
        User currentUser;
        private readonly IGenericRepository<Product, int> _product;
        private readonly IGenericRepository<OrderType, int> _orderType;
        private readonly IGenericRepository<Order, int> _order;
        private readonly IGenericRepository<User, int> _user;
        public CartController(IGenericRepository<Product, int> productRepository, IGenericRepository<OrderType, int> orderTypeRepository,
            IGenericRepository<Order, int> orderRepository, IGenericRepository<User, int> userRepository
            )
        {
            
            _product = productRepository;
            _orderType = orderTypeRepository;
            _order = orderRepository;
            _user = userRepository;
             cart = new Cart();
             currentUser = new User();
           


        }

        public RedirectToRouteResult AddToCart(Cart cart, int? Id, string returnUrl)
        {
            
            Product prod = _product.GetAll().FirstOrDefault(p => p.Id == Id);

            if (prod != null)
            {
                cart.AddItem(prod, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int? Id, string returnUrl)
        {
            Product prod = _product.GetAll().FirstOrDefault(p => p.Id == Id);

            if (prod != null)
            {
                cart.RemoveLine(prod);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

                
        public ViewResult Index(Cart cart, string returnUrl)
        {
            
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
               
            });
        }

        public ActionResult IndexSearch(int? Id)
        {
            ViewBag.OrderTypes = new SelectList(_orderType.GetAll(), "Id", "OrderTypeName");

            var selected_orders = _order.GetAll().Where(u => u.User.UserEmail == HttpContext.User.Identity.Name).
               Where(u => Id == null || u.OrderTypeId == Id).OrderBy(o => o.OrderDate).ThenBy(n => n.OrderNumber);


            return PartialView(selected_orders);
        }

        public ActionResult IndexSubSearchFilter(int? OrderTypeId)
        {
            
            ViewBag.OrderTypes = new SelectList(_orderType.GetAll().ToList(), "Id", "OrderTypeName");
            var selected_orders = _order.GetAll().ToList().Where(u => u.User.UserEmail == HttpContext.User.Identity.Name).
               Where(u => OrderTypeId == null || u.OrderTypeId == OrderTypeId).OrderBy(o => o.OrderDate).ThenBy(n => n.OrderNumber);
            return PartialView(selected_orders);
        }



        public PartialViewResult _LoginPartial(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout(Cart cart, User user)
        {
                     
            User addOrChangeUser = null;
            Order newOrder=null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addOrChangeUser  = _user.GetAll().FirstOrDefault(c=>c.UserEmail==HttpContext.User.Identity.Name);
                newOrder = CreateOrderForUser(addOrChangeUser.Id, cart);                
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // check if user exist
                   addOrChangeUser = _user.GetAll().FirstOrDefault(c=>c.UserEmail== user.UserEmail);
                   if (addOrChangeUser != null)
                   {
                        //if user found update adress
                        _user.Update(user);
                     
                    }
                   else
                   {
                        // if user not exist add new user
                        user.UserRoleId = 3;
                     addOrChangeUser = _user.Add(user);
                     _user.Save();
                   }
                    newOrder = CreateOrderForUser(addOrChangeUser.Id,  cart);
                                     
                }
                
            }
            if (HttpContext.User.Identity.IsAuthenticated||ModelState.IsValid)
            {
                
                
                _order.Add(newOrder);
                _order.Save();
                newOrder.User = addOrChangeUser ?? new User();
                cart.Clear();
            }

            
            return View(newOrder);
        }

        private Order CreateOrderForUser(int  user_id, Cart cart)
        {   

            Order newOrder = new Order
                  (DateTime.Now.Date,
                   GenerateOrderNumber(DateTime.Now.Date), 
                   user_id
                   , 1, cart.Lines.Sum(s => s.Quantity * s.Product.Price));

            newOrder.OrderDetail= cart.Lines.Select(line => new OrderDetail(
                     line.Product.Id,
                     line.Quantity,
                     line.Product.Price,
                     line.Quantity * line.Product.Price)).ToList();
            
            return newOrder;
           
        }


        private string GenerateOrderNumber(DateTime date)
        {
            int countOrderToday = _order.GetAll().Where(d => d.OrderDate == date.Date).Count() + 1;
            return DateTime.Now.ToString("ddMMyyyy") + "_" + countOrderToday.ToString();
        }
    }
}