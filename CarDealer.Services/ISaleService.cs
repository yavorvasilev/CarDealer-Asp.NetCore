namespace CarDealer.Services
{
	using System.Collections.Generic;
	using Models.Sales;

	public interface ISaleService
	{
		IEnumerable<SaleListModel> All();

		SaleDetailsModel Details(int id);
	}
}
