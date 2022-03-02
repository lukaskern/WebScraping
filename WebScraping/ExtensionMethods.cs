using System.Linq;
using HtmlAgilityPack;

namespace WebScraping
{
	public static class ExtensionMethods
	{	
		public static HtmlNode FindNodeByClass(this HtmlNode from, string name)
		{
			foreach(HtmlNode node in from.ChildNodes)
				foreach(HtmlAttribute attr in node.Attributes)
					if (attr.Name == "class" && attr.Value == name)
						return node;
			return null;
		}

		public static string GetNotransText(this HtmlNode coll)
		{
			foreach (HtmlNode node in coll.ChildNodes)
				if (node.Attributes.Contains("class"))
					return node.ChildNodes[1].InnerText;
			return "";
		}
	}
}
