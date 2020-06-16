namespace CarDealer.Web.Controllers
{
    using CarDealer.Web.Models.Suppliers;
    using Microsoft.AspNetCore.Mvc;
	using Services;
    using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class SuppliersController : Controller
	{
		private const string SuppliersView = "Suppliers";

		private readonly ISupplierService suppliers;

		public SuppliersController(ISupplierService suppliers)
		{
			this.suppliers = suppliers;
		}

		public IActionResult Local() 
		{
			return this.View(SuppliersView, this.GetSuppliersModel(false));
		}

		public IActionResult Importers() 
		{
			return this.View(SuppliersView, this.GetSuppliersModel(true));
		}

		private SuppliersModel GetSuppliersModel(bool importers) 
		{
			var type = importers ? "Importer" : "Local";

			return new SuppliersModel
			{
				Type = type,
				Suppliers = suppliers.AllListings(importers)
			};
		}
	}
}
