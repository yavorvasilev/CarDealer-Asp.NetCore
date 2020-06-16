namespace CarDealer.Web.Controllers
{
	using Infrastructure.Extensions;
	using CarDealer.Services.Models;
	using CarDealer.Web.Models.Customers;
	using Microsoft.AspNetCore.Mvc;
	using Services;

	[Route("customers")]
	public class CustomersController : Controller
	{
		private readonly ICustomerService customers;

		public CustomersController(ICustomerService customers)
		{
			this.customers = customers;
		}

		[Route(nameof(Create))]
		public IActionResult Create() => View();

		[HttpPost]
		[Route(nameof(Create))]
		public IActionResult Create(CustomerFormModel model) 
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			customers.Create(
				model.Name,
				model.Birthday,
				model.IsYoungDriver);

			return RedirectToAction(nameof(All), new { order = OrderDirection.Ascendiung});
		}

		[Route(nameof(Edit) + "/{id}")]
		public IActionResult Edit(int id)
		{
			var customer = customers.ById(id);

			if (customer == null)
			{
				return NotFound();
			}

			return View(new CustomerFormModel
			{
				Name = customer.Name,
				Birthday = customer.BirthDay,
				IsYoungDriver = customer.IsYoungDriver
			});
		}

		[HttpPost]
		[Route(nameof(Edit) + "/{id}")]
		public IActionResult Edit(int id, CustomerFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var customerExists = customers.Exists(id);

			if (!customerExists)
			{
				return NotFound();
			}

			customers.Edit(
				id,
				model.Name,
				model.Birthday,
				model.IsYoungDriver);

			return RedirectToAction(nameof(All), new { order = OrderDirection.Ascendiung });
		}

		[Route("all/{order}")]
		public IActionResult All(string order) 
		{
			var orderDirection = order.ToLower() == "descending" 
				? OrderDirection.Descending 
				: OrderDirection.Ascendiung;

			var customers = this.customers.Ordered(orderDirection);

			return View(new AllCustomersModel
			{ 
				Customers = customers,
				OrderDirection = orderDirection
			});
		}

		[Route("{id}")]
		public IActionResult TotalSales(int id)
		{
			var customerWithSales = this.customers.TotalSalesById(id);

			return this.ViewOrNotFound(customerWithSales);
		}
	}
}
