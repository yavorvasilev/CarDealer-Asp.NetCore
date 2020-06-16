namespace CarDealer.Services.Implementations
{
	using CarDealer.Data;
	using CarDealer.Services.Models.Parts;
	using CarDealer.Services.Models.Cars;
	using System.Collections.Generic;
	using System.Linq;
    using CarDealer.Data.Models;

    public class CarService : ICarService
	{
		private readonly CarDealerDbContext db;

		public CarService(CarDealerDbContext db)
		{
			this.db = db;
		}

		public IEnumerable<CarModel> ByMake(string make)
			=> db
				.Cars
				.Where(c => c.Make.ToLower() == make.ToLower())
				.OrderBy(c => c.Model)
				.ThenBy(c => c.TravelledDistance)
				.Select(c => new CarModel
				{
					Make = c.Make,
					Model = c.Model,
					TravelledDistance = c.TravelledDistance
				})
				.ToList();

		public void Create(string make, string model, long travelledDistance, IEnumerable<int> parts)
		{

			var existingPartIds = db
				.Parts
				.Where(p => parts.Contains(p.Id))
				.Select(p => p.Id)
				.ToList();

			var car = new Car
			{
				Make = make,
				Model = model,
				TravelledDistance = travelledDistance
			};

			foreach (var partId in existingPartIds)
			{
				car.Parts.Add(new PartCars 
				{
					PartId = partId
				});
			}

			db.Add(car);
			db.SaveChanges();
		}

		public IEnumerable<CarWithPartsModel> WithParts()
			=> db
			.Cars
			.OrderByDescending(c => c.Id)
			.Select(c => new CarWithPartsModel
			{
				Make = c.Make,
				Model = c.Model,
				TravelledDistance = c.TravelledDistance,
				Parts = c.Parts.Select(p => new PartModel
				{
					Name = p.Part.Name,
					Price = p.Part.Price
				})
			})
			.ToList();
	}
}
