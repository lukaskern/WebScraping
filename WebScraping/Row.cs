namespace WebScraping
{
	public class Row
	{
		public string ArticleName;
		public string Price;
		public string ProductLink;

		public override bool Equals(object obj)
		{
			Row r = (Row) obj;
			return ArticleName == r.ArticleName && Price == r.Price && ProductLink == r.ProductLink;
		}
	}
}
