namespace CarDealer.Web.Infrastructure.Extensions
{
	public static class StringExtensions
	{
		private const string NumberFormat = "F2";

		public static string ToPrice(this decimal priceText)
			=> $"${priceText.ToString(NumberFormat)}";

		public static string ToPercentage(this double number)
			=> $"{number.ToString(NumberFormat)}%";
	}
}
