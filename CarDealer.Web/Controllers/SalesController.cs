namespace CarDealer.Web.Controllers
{
	using Infrastructure.Extensions;
	using Microsoft.AspNetCore.Mvc;
	using Services;

	[Route("Sales")]
	public class SalesController : Controller
	{
		private readonly ISaleService sales;

		public SalesController(ISaleService sales)
		{
			this.sales = sales;
		}

		[Route("")]
		public IActionResult All()
			=> View(sales.All());

		[Route("{id}")]
		public IActionResult Details(int id)
			=> this.ViewOrNotFound(sales.Details(id));
	}
}
