using System.Diagnostics;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace WebScraping
{
	public partial class WebScraper : Form
	{
		private string Address = "https://www.mydealz.de/";
		private string HTML;
		private int TimeSeconds = 60;

		public WebScraper()
		{
			InitializeComponent();
			AppDomain.CurrentDomain.UnhandledException += LogAllExceptions;
			MyDealzScraper.MainLoop();
		}

		#region Sync
		//public void MainLoop()
		//{
		//	using (StreamWriter sw = new StreamWriter("out.txt") { AutoFlush = true })
		//	{
		//		List<Row> rows = GetRows();
		//		string newArticles = "";
		//		//ni.Visible = true;
		//		//ni.Icon = SystemIcons.Application;
		//		while (true)
		//		{
		//			List<Row> newRows = GetRows();

		//			newArticles = newRows.Where(e => !rows.Any(x => x.Equals(e)))
		//				.Aggregate("", (x, y) => x + y.ArticleName + ", " + y.ProductLink + ", " + y.Price + "\n");

		//			if (!string.IsNullOrEmpty(newArticles))
		//			{
		//				//ni.ShowBalloonTip(TimeSeconds * 1000, "New Articles", newArticles, ToolTipIcon.None);
		//				sw.WriteLine(newArticles);
		//			}

		//			newArticles = "";
		//			rows = newRows;
		//			//Task.Delay(TimeSeconds * 1000);
		//			Thread.Sleep(TimeSeconds * 1000);
		//		}
		//	}
		//}

		//public List<Row> GetRows()
		//{
		//	HTML = GetHTML();

		//	HtmlDocument d = new HtmlDocument();
		//	d.LoadHtml(HTML);

		//	HtmlNode articlesNode = d.GetElementbyId("toc-target-deals").ChildNodes[0];
		//	List<HtmlNode> nodes = articlesNode.ChildNodes.Where(e => e.Name == "article").ToList();

		//	List<Row> rows = new List<Row>();
		//	foreach (HtmlNode n in nodes)
		//	{
		//		List<HtmlNode> allChildren = n.DescendantsAndSelf().ToList();
		//		Row r = new Row
		//		{
		//			ArticleName = allChildren.FirstOrDefault(e => e.Name == "a" && e.HasAttributes && e.Attributes["class"].Value.Contains("cept-tt")).InnerText,
		//			Price = allChildren.FirstOrDefault(e => e.Name == "span" && e.HasAttributes && e.Attributes["class"].Value.Contains("thread-price"))?.InnerText,
		//			ProductLink = allChildren.FirstOrDefault(e => e.Name == "a" && e.HasAttributes && e.Attributes["class"].Value.Contains("cept-dealBtn")).Attributes["href"].Value
		//		};
		//		rows.Add(r);
		//	}

		//	return rows;
		//}

		//public string GetHTML()
		//{
		//	using (HttpClient client = new HttpClient())
		//	{
		//		HttpResponseMessage resp = client.GetAsync(Address).Result;
		//		return resp.Content.ReadAsStringAsync().Result;
		//	}
		//}
		#endregion

		#region Async
		public async void MainLoop()
		{
			using (StreamWriter sw = new StreamWriter("out.txt") { AutoFlush = true })
			{
				List<Row> rows = await GetRows();
				string newArticles = "";
				//ni.Visible = true;
				//ni.Icon = SystemIcons.Application;
				while (true)
				{
					List<Row> newRows = await GetRows();

					newArticles = newRows.Where(e => !rows.Any(x => x.Equals(e)))
						.Aggregate("", (x, y) => x + y.ArticleName + ", " + y.ProductLink + ", " + y.Price + "\n");

					if (!string.IsNullOrEmpty(newArticles))
					{
						//ni.ShowBalloonTip(TimeSeconds * 1000, "New Articles", newArticles, ToolTipIcon.None);
						sw.WriteLine(newArticles);
					}

					newArticles = "";
					rows = newRows;
					await Task.Delay(TimeSeconds * 1000);
				}
			}
		}

		public async Task<List<Row>> GetRows()
		{
			HTML = await GetHTML();

			HtmlDocument d = new HtmlDocument();
			d.LoadHtml(HTML);

			HtmlNode articlesNode = d.GetElementbyId("toc-target-deals").ChildNodes[0];
			List<HtmlNode> nodes = articlesNode.ChildNodes.Where(e => e.Name == "article").ToList();

			List<Row> rows = new List<Row>();
			foreach (HtmlNode n in nodes)
			{
				List<HtmlNode> allChildren = n.DescendantsAndSelf().ToList();
				Row r = new Row
				{
					ArticleName = allChildren.FirstOrDefault(e => e.Name == "a" && e.HasAttributes && e.Attributes["class"].Value.Contains("cept-tt")).InnerText,
					Price = allChildren.FirstOrDefault(e => e.Name == "span" && e.HasAttributes && e.Attributes["class"].Value.Contains("thread-price"))?.InnerText,
					ProductLink = allChildren.FirstOrDefault(e => e.Name == "a" && e.HasAttributes && e.Attributes["class"].Value.Contains("cept-dealBtn")).Attributes["href"].Value
				};
				rows.Add(r);
			}

			return rows;
		}

		public async Task<string> GetHTML()
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage resp = await client.GetAsync(Address);
				return resp.Content.ReadAsStringAsync().Result;
			}
		}
		#endregion

		private void NotificationClicked(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				Process.Start(Address);
			if (e.Button == MouseButtons.Right)
				Environment.Exit(0);
		}

		private void LogAllExceptions(object sender, UnhandledExceptionEventArgs e)
		{
			using (StreamWriter sw = new StreamWriter(Path.Combine(@"C:\Users\lk3\Desktop", "Exception.txt")) { AutoFlush = true })
			{
				Exception ex = e.ExceptionObject as Exception;
				sw.WriteLine(ex.Message);
				sw.WriteLine(ex.StackTrace);
				sw.Flush();
			}
		}
	}
}
