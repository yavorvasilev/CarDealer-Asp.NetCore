﻿using CarDealer.Data;
using CarDealer.Data.Models;
using CarDealer.Services.Models.Parts;
using System.Collections.Generic;
using System.Linq;

namespace CarDealer.Services.Implementations
{
	public class PartService : IPartService
	{
		private readonly CarDealerDbContext db;

		public PartService(CarDealerDbContext db)
		{
			this.db = db;
		}

		public IEnumerable<PartBasicModel> All()
			=> db
			.Parts
			.OrderBy(p => p.Id)
			.Select(p => new PartBasicModel
			{
				Id = p.Id,
				Name = p.Name
			})
			.ToList();

		public IEnumerable<PartListingModel> AllListings(int page = 1, int pageSize = 10)
			 => db
			.Parts
			.OrderByDescending(p => p.Id)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Select(p => new PartListingModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Quantity = p.Quantity,
				SupplierName = p.Supplier.Name
			})
			.ToList();

		public PartDetailsModel ById(int id)
			=> db
			.Parts
			.Where(p => p.Id == id)
			.Select(p => new PartDetailsModel
			{
				Name = p.Name,
				Price = p.Price,
				Quantity = p.Quantity
			})
			.FirstOrDefault();

		public void Create(string name, decimal price, int quantity, int supplierId)
		{
			var part = new Part
			{
				Name = name,
				Price = price,
				Quantity = quantity > 0 ? quantity : 1,
				SupplierId = supplierId
			};

			db.Add(part);
			db.SaveChanges();
		}

		public void Delete(int id)
		{
			var part = db.Parts.Find(id);

			if (part == null)
			{
				return;
			}

			db.Parts.Remove(part);
			db.SaveChanges();
		}

		public void Edit(int id, decimal price, int quantity)
		{
			var part = db.Parts.Find(id);

			if (part == null)
			{
				return;
			}

			part.Price = price;
			part.Quantity = quantity;

			db.SaveChanges();
		}

		public int Total() => db.Parts.Count();
	}
}
