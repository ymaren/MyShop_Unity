using MyShop.Models.interfaces.Repositories;
using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace StoreWeb.Controllers
{
    public class OrderController : Controller
    {


        private readonly IGenericRepository<OrderType, int> _orderType;
        private readonly IGenericRepository<User, int> _user;
        private readonly IGenericRepository<Order, int> _order;
        public OrderController(IGenericRepository<OrderType, int> orderTypeRepository, IGenericRepository<User, int> userRepository,
            IGenericRepository<Order, int> orderRepository
            )
        {
            
            _orderType = orderTypeRepository;
            _user =  userRepository;
            _order = orderRepository;
        }

        public ViewResult Index()
        {
            ViewBag.OrderTypes = new SelectList(_orderType.GetAll(), "Id", "OrderTypeName");
            ViewBag.Users = new SelectList(_user.GetAll(), "Id", "UserName");
            return View(_order.GetAll());
        }

        public ActionResult IndexSearch(DateTime? StartDate, DateTime? FinishDate, int? orderToUser, int? OrderTypeId)
        {
           
            var selected_orders = _order.GetAll().Where(u => orderToUser == null || u.UserId == orderToUser).
                Where(u => OrderTypeId == null || u.OrderTypeId == OrderTypeId).Where
                (d=> StartDate == null || d.OrderDate>=StartDate).Where(
                f => FinishDate == null || f.OrderDate <= FinishDate).OrderBy(o=>o.OrderDate).ThenBy(n=>n.OrderNumber);
            return PartialView(selected_orders);
        }



        [HttpGet]
        public ViewResult Edit(int? Id)
        {
           //bool load=  DbContext.Configuration.LazyLoadingEnabled
            ViewBag.OrderTypes = new SelectList(_orderType.GetAll(), "Id", "OrderTypeName");
            ViewBag.Users = new SelectList(_user.GetAll(), "Id", "UserName");
            Order order = _order.GetSingle(int.Parse(Id.ToString())); //Orders.Include(c => c.OrderType).Include(c=>c.OrderDetail.Select(p=>p.Product)).FirstOrDefault(c => c.Id == Id);
           
            
            return View(order??new Order(DateTime.Now, GenerateOrderNumber (DateTime.Now)));
        }

        private string GenerateOrderNumber (DateTime date)
        {
          int countOrderToday=  _order.GetAll().Where(d => d.OrderDate == date.Date).Count()+1;
            return DateTime.Now.ToString("ddMMyyyy") + "_" + countOrderToday.ToString();
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {

            if (ModelState.IsValid)
            {
                Order foundOrder = _order.GetSingle(order.Id);
                if (foundOrder!=null)
                {

                    _order.Update(order);
                    TempData["message"] = string.Format("Order \"{0}\"uploaded", order.OrderNumber);
                    
                }
                else
                {
                    _order.Add(order);
                    TempData["message"] = string.Format("Order\"{0}\"added", order.OrderNumber);
                }
                _order.Save();
                return RedirectToAction("Index");

            }
            else
            {
                return View(order);
            }

        }
        [HttpPost]
        public ViewResult Create()
        {
            return View("Edit", new Order());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Order foundOrder= _order.GetAll().FirstOrDefault(c => c.Id == Id);

            if (foundOrder != null)
            {
                TempData["message"] = string.Format("Order was deleted");
                _order.Delete(foundOrder);
                _order.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("User was not found");
            }

            return RedirectToAction("Index");
        }


        public ViewResult CreateOrderFromCart()
        {
            return View("Edit", new Order());
        }
    }

}


