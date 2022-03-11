namespace TaskTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			//Asynchron wenn Methode nicht async sein kann
			Task.Run(() =>
			{
				HttpResponseMessage resp = APIInterface.Get("/file/scryfall-bulk/default-cards/default-cards-20220301100243.json");
				string json = resp.Content.ReadAsStringAsync().Result;
			});
		}

		//https://c2.scryfall.com/file/scryfall-bulk/default-cards/default-cards-20220301100243.json
		//250MB, 67000 Zeilen, ~300 Mio. Zeichen
		private async void ButtonClicked(object sender, EventArgs e)
		{
			//Läuft Synchron, freezt UI
			//HttpResponseMessage resp = APIInterface.Get("/file/scryfall-bulk/default-cards/default-cards-20220301100243.json");
			//string json = resp.Content.ReadAsStringAsync().Result;

			//Asynchron durch await
			HttpResponseMessage resp = await APIInterface.GetAsync("/file/scryfall-bulk/default-cards/default-cards-20220301100243.json");
			string json = await resp.Content.ReadAsStringAsync();
		}
	}
}