namespace FastFood.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using FastFood.Web.ViewModels.Employees;
    using FastFood.Web.ViewModels.Items;

    public class CreateOrderViewModel
    {
        public List<ItemViewModel> Items { get; set; }

        public List<EmployeeViewModel> Employees { get; set; }

    }
}
