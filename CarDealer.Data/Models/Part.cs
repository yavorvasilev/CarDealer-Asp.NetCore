namespace CarDealer.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Part
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Range(0, double.MaxValue)]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[Range(0, int.MaxValue)]
		public int Quantity { get; set; }

		public int SupplierId { get; set; }

		public Supplier Supplier { get; set; }

		public List<PartCars> Cars { get; set; } = new List<PartCars>();

	}
}
