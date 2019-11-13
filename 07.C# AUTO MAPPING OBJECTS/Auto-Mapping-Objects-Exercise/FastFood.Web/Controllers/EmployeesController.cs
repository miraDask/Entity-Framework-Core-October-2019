﻿namespace FastFood.Web.Controllers
{
    using System.Linq;
    
    using Data;
    using ViewModels.Employees;
    using AutoMapper;
    using FastFood.Models;
    
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper.QueryableExtensions;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            var positions = this.context
                .Positions
                .ProjectTo<RegisterEmployeeViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return this.View(positions);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var employee = this.mapper.Map<Employee>(model);
            this.context.Employees.Add(employee);
            this.context.SaveChanges();

            return RedirectToAction("All", "Employees");
        }

        public IActionResult All()
        {
            var employees = this.context
                .Employees
                .ProjectTo<EmployeesAllViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return this.View(employees);
        }

        public IActionResult Delete(int id)
        {
            var employee = this.context.Employees.FirstOrDefault(o => o.Id == id);
            this.context.Employees.Remove(employee);
            this.context.SaveChanges();

            return this.RedirectToAction("All", "Employees");
        }
    }
}
