
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
    public class OrderTypeController : Controller
    {

        private readonly IGenericRepository<OrderType, int> _orderType;

        public OrderTypeController(IGenericRepository<OrderType, int> orderTypeRepository)
        {
            _orderType = orderTypeRepository;
           
        }

        //list
        public ViewResult Index()
        {
            return View(_orderType.GetAll());
        }

        
        [HttpGet]
        public ViewResult Edit(int? Id)
        {

            OrderType orderType = _orderType.GetAll().FirstOrDefault(c => c.Id == Id);
            return View(orderType);
        }

        [HttpPost]
        public ActionResult Edit(OrderType orderType)
        {
            
            if (ModelState.IsValid)
            {
                OrderType foundOrderType= _orderType.GetAll().FirstOrDefault(c => c.Id == orderType.Id);
                if (foundOrderType!=null)
                {
                    _orderType.Update(orderType);
                    TempData["message"] = string.Format("Order type \"{0}\"uploaded", orderType.OrderTypeName);                    
                }
                else
                {
                    _orderType.Add(orderType);
                    TempData["message"] = string.Format("Order type\"{0}\"added", orderType.OrderTypeName);
                }
                _orderType.Save();
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(orderType);
            }
          
        }

        [HttpPost]
        public ViewResult Create()
        {
            return View("Edit", new OrderType());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {

            OrderType foundType = _orderType.GetAll().FirstOrDefault(c => c.Id == Id);

            if (foundType != null)
            {
                TempData["message"] = string.Format("Product group  was deleted");
                _orderType.Delete(foundType);
                _orderType.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("Product group was not found");
            }
            return RedirectToAction("Index");
        }
    }

}


   