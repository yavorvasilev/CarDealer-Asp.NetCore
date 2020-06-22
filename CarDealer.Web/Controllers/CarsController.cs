namespace CarDealer.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using CarDealer.Services;
    using CarDealer.Web.Models.Cars;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;

    [Route("cars")]
    public class CarsController : Controller
	{
		private readonly ICarService cars;
		private readonly IPartService parts;

		public CarsController(ICarService cars, IPartService parts)
		{
			this.cars = cars;
			this.parts = parts;
		}

		[Authorize]
		[Route(nameof(Create))]
		public IActionResult Create()
			=> View(new CarFormModel
			{
				AllParts = GetPartsSelectItems()
			});

		[Authorize]
		[HttpPost]
		[Route(nameof(Create))]
		public IActionResult Create(CarFormModel carModel) 
		{
			if (!ModelState.IsValid)
			{
				carModel.AllParts = GetPartsSelectItems();
				return View(carModel);
			}

			cars.Create(
				carModel.Make,
				carModel.Model,
				carModel.TravelledDistance,
				carModel.SelectedParts);

			return RedirectToAction(nameof(Parts));
		}

		[Route("{make}", Order = 2)]
		public IActionResult ByMake(string make) 
		{
			var cars = this.cars.ByMake(make);

			return View(new CarsByMakeModel 
			{
				Make = make,
				Cars = cars
			});
		}

		[Route("parts", Order = 1)]
		public IActionResult Parts()
		{
			return View(this.cars.WithParts());
		}

		private IEnumerable<SelectListItem> GetPartsSelectItems() 
		{
			return parts
				.All()
				.Select(p => new SelectListItem
				{
					Text = p.Name,
					Value = p.Id.ToString()
				});
		}
	}
}
