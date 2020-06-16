namespace CarDealer.Services.Models.Sales
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class SaleListModel : SaleModel
	{
		public int Id { get; set; }

		public string CustomerName { get; set; }

		public bool IsYoungDriver { get; set; }

		public decimal DiscountedPrice => 
			this.Price * (1 - ((decimal)this.Discount + (this.IsYoungDriver ? 0.05m : 0)));
	}
}
