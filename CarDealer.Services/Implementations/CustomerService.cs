namespace CarDealer.Services.Implementations
{
	using Models;
	using Models.Sales;
	using CarDealer.Services.Models.Customers;
	using Data;
	using Data.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class CustomerService : ICustomerService
	{
		private readonly CarDealerDbContext db;

		public CustomerService(CarDealerDbContext db)
		{
			this.db = db;
		}

		public CustomerModel ById(int id)
			=> db
			.Customers
			.Where(c => c.Id == id)
			.Select(c => new CustomerModel
			{
				Id = c.Id,
				Name = c.Name,
				BirthDay = c.BirthDay,
				IsYoungDriver = c.IsYoungDriver
			})
			.FirstOrDefault();

		public void Create(string name, DateTime birthday, bool isYoungDriver)
		{
			var customer = new Customer
			{
				Name = name,
				BirthDay = birthday,
				IsYoungDriver = isYoungDriver
			};

			db.Add(customer);
			db.SaveChanges();
		}

		public void Edit(int id, string name, DateTime birthday, bool isYoungDriver)
		{
			var existingCustomer = db.Customers.Find(id);

			if (existingCustomer == null)
			{
				return;
			}

			existingCustomer.Name = name;
			existingCustomer.BirthDay = birthday;
			existingCustomer.IsYoungDriver = isYoungDriver;

			db.SaveChanges();
		}

		public bool Exists(int id)
			=> db.Customers.Any(c => c.Id == id);

		public IEnumerable<CustomerModel> Ordered(OrderDirection order)
		{
			var customersQuery = db.Customers.AsQueryable();

			switch (order)
			{
				case OrderDirection.Ascendiung:
					customersQuery = customersQuery
						.OrderBy(c => c.BirthDay)
						.ThenBy(c => c.Name);

					break;
				case OrderDirection.Descending:
					customersQuery = customersQuery
						.OrderByDescending(c => c.BirthDay)
						.ThenBy(c => c.Name);

					break;
				default:
					throw new InvalidOperationException($"Invalid order direction: {order}");
			}

			return customersQuery
				.Select(c => new CustomerModel
				{
					Id = c.Id,
					Name = c.Name,
					BirthDay = c.BirthDay,
					IsYoungDriver = c.IsYoungDriver
				})
				.ToList();
		}

		public CustomerTotalSalesModel TotalSalesById(int id)
		{
			return db
				.Customers
				.Where(c => c.Id == id)
				.Select(c => new CustomerTotalSalesModel
				{
					Name = c.Name,
					IsYoungDriver = c.IsYoungDriver,
					BoughtCars = c.Sales.Select(s => new SaleModel
					{
						Price = s.Car.Parts.Sum(p => p.Part.Price),
						Discount = s.Discount
					})
				})
				.FirstOrDefault();
		}
	}
}
