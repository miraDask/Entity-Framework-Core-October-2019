namespace FastFood.Web.Controllers
{
    using System;
    using System.Linq;
    
    using Data;
    using ViewModels.Orders;
    using FastFood.Models;
    
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using FastFood.Web.ViewModels.Items;
    using FastFood.Web.ViewModels.Employees;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {

            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context
                .Items
                .ProjectTo<ItemViewModel>(this.mapper.ConfigurationProvider)
                .ToList(),
                Employees = this.context
                .Employees
                .ProjectTo<EmployeeViewModel>(this.mapper.ConfigurationProvider)
                .ToList()
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var order = this.mapper.Map<Order>(model);
            order.DateTime = DateTime.Now;

            var orderItem = new OrderItem()
            {
                ItemId = model.ItemId,
                Order = order,
                Quantity = model.Quantity
            };

            order.OrderItems.Add(orderItem);

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = this.context
                .Orders
                .ProjectTo<OrderAllViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return this.View(orders);
        }

        public IActionResult Delete(int id)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == id);
            this.context.Orders.Remove(order);
            this.context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }
    }
}
