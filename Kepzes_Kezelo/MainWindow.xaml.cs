using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using cnKepzesek;
using Microsoft.EntityFrameworkCore;
namespace Kepzes_Kezelo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{
		KepzesekTabla tabla;
		List<Grid> oldalak;

		public MainWindow()
		{

			InitializeComponent();
			tabla = new KepzesekTabla();
			//Oktato oktato = new Oktato { Nev ="Sanyi", Szakterulet = "Sebész"};
			Initdb();
			oldalak = new List<Grid>();
			oldalak.Add(gVisualize);
			oldalak.Add(gAddKepzes);
			oldalak.Add(gAddOktato);
			oldalak.Add(gAddResztvevo);
			oldalak.Add(gEdit);
			oldalak.Add(gVisualize);
			oldalak.Add(gSearchKepzes);
			oldalak.Add(gSearchOktato);
			oldalak.Add(gSearchResztvevo);
			oldalak.Add(gHome);
			oldalak.Add(gEditOktato);
		}

		private void Initdb()
		{
			tabla.Database.EnsureCreated();
			if (tabla.Kepzesek != null)
			{

				if (!tabla.Kepzesek.Any())
				{
					Populate();
				}
			}
		}

		private void Populate()
		{
			var k1 = new Kepzes { Hely = "Szeged", Nev = "ITBiz", BefejezesDatuma = DateTime.Now, KezdesDatuma = DateTime.Today };
			var k2 = new Kepzes { Hely = "Pécs", Nev = "asd", BefejezesDatuma = DateTime.Now, KezdesDatuma = DateTime.Today };
			var o1 = new Oktato { Kepzesek = [k1], Nev = "Sanyi", Szakterulet = "Otthon" };

			var r1 = new Resztvevo { Beosztas = "Kapu", Nev = "Péter", Kepzeseks = [k1, k2] };
			var r2 = new Resztvevo { Beosztas = "Ajtó", Nev = "Sándor", Kepzeseks = [k1] };

			k1.Oktatok.Add(o1);
			k2.Oktatok.Add(o1);


			tabla.Kepzesek.Add(k1);
			tabla.Kepzesek.Add(k2);


			tabla.Oktatok.Add(o1);
			tabla.Resztvevok.Add(r1);
			tabla.Resztvevok.Add(r2);
			tabla.SaveChanges();

		}



		/// <summary>
		/// Feltölti a Combo box-ok tartalmát az Add menünél
		/// </summary>
		private void FillLists()
		{
			cbOktatok.ItemsSource = tabla.Oktatok.Select(x => x.Nev).ToList();
			cbResztvevo.ItemsSource = tabla.Resztvevok.Select(x => x.Nev).ToList();
			cbKepzesek.ItemsSource = tabla.Kepzesek.Select(x => x.Nev).ToList();
			cbKepzesek2.ItemsSource = tabla.Kepzesek.Select(x => x.Nev).ToList();
		}

		private void cVisualize(object sender, RoutedEventArgs e)
		{

			//gVisualize.Visibility = Visibility.Visible;
			//gEdit.Visibility = Visibility.Collapsed;
			HideGrids(gVisualize);

			dgMegjelen.ItemsSource = tabla.Kepzesek.Include(p => p.Oktatok).Include(p => p.Resztvevoks).ToList();
			dgMgejelen2.ItemsSource = tabla.Oktatok.Include(p => p.Kepzesek).ToList();
			dgMegjelenResztvevok.ItemsSource = tabla.Resztvevok.Include(p => p.Kepzeseks).ToList();


		}

		

		private void ClickTodayStart(object sender, RoutedEventArgs e)
		{
			tbDateStart.Text = DateTime.Now.ToShortDateString();
			tbDateStart.DataContext = DateTime.Now;
		}

		private void ClickTodayEnd(object sender, RoutedEventArgs e)
		{
			tbDateEnd.Text = DateTime.Now.ToShortDateString();
			tbDateEnd.DataContext = DateTime.Now;
		}

		private void CliclSaveKepzesek(object sender, RoutedEventArgs e)
		{
			if (tbName.Text == "") { MessageBox.Show("Üres képzés név", "Hiba", MessageBoxButton.OK); return; }
			if (tbDateStart.Text == "") { MessageBox.Show("Üres képzés dátum", "Hiba", MessageBoxButton.OK); return; }
			if (tbDateEnd.Text == "") { MessageBox.Show("Üres képzés dátum", "Hiba", MessageBoxButton.OK); return; }
			if (tbPlace.Text == "") { MessageBox.Show("Üres képzés hely", "Hiba", MessageBoxButton.OK); return; }


			//az oktató már létezik
			if (tabla.Kepzesek.Any(x => x.Nev == tbName.Text)) { MessageBox.Show("Létező képzés név", "Hiba", MessageBoxButton.OK); return; }

			if (DateTime.TryParse(tbDateStart.Text, out DateTime start) && DateTime.TryParse(tbDateEnd.Text, out DateTime end))
			{

				List<Oktato> oktatok = tabla.Oktatok.ToList().Where(x => cbHozzaadotOktatok.Items.Contains(x.Nev)).ToList();
				List<Resztvevo> resztvevok = tabla.Resztvevok.ToList().Where(x => cbHozzaadotResztvevok.Items.Contains(x.Nev)).ToList();

				Kepzes newKepzes = new Kepzes { Nev = tbName.Text, KezdesDatuma = start, BefejezesDatuma = end, Hely = tbPlace.Text, Oktatok = oktatok, Resztvevoks = resztvevok };

				tabla.Kepzesek.Add(newKepzes);
				tabla.SaveChanges();

				MessageBox.Show("Mentés sikeres");
				FillLists();
				tbName.Text = "";
				tbDateStart.Text = "";
				tbDateEnd.Text = "";
				tbPlace.Text = "";
				cbHozzaadotOktatok.ItemsSource = null;
				cbHozzaadotResztvevok.ItemsSource = null;
			}
		}

		private void ClickOktatoHozzaadasa(object sender, RoutedEventArgs e)
		{
			if (cbOktatok.SelectedItem != null)
			{
				cbHozzaadotOktatok.Items.Add(cbOktatok.SelectedItem.ToString());
				cbHozzaadotOktatok.SelectedIndex = 0;
			}

		}

		private void ClickResztvevoHozzaadasa(object sender, RoutedEventArgs e)
		{
			if (cbResztvevo.SelectedItem != null)
			{
				cbHozzaadotResztvevok.SelectedIndex = 0;
				cbHozzaadotResztvevok.Items.Add(cbResztvevo.SelectedItem.ToString());
			}
		}



		private void ClickSaveOktato(object sender, RoutedEventArgs e)
		{
			if (tbOktatoNev.Text == "") { MessageBox.Show("Üres oktató név", "Hiba", MessageBoxButton.OK); return; }
			if (tbOktatoSzakterulet.Text == "") { MessageBox.Show("Üres oktató szakterület", "Hiba", MessageBoxButton.OK); return; }

			List<Kepzes> kepzesek = tabla.Kepzesek.ToList().Where(x => cbKivalasztottKepzes.Items.Contains(x.Nev)).ToList();

			Oktato newOktato = new Oktato { Nev = tbOktatoNev.Text, Szakterulet = tbOktatoSzakterulet.Text, Kepzesek = kepzesek };

			tabla.Oktatok.Add(newOktato);
			tabla.SaveChanges();

			MessageBox.Show("Mentés sikeres");
			FillLists();
			tbOktatoNev.Text = "";
			tbOktatoSzakterulet.Text = "";
			cbKivalasztottKepzes.ItemsSource = null;

		}

		private void ClickKivalasztottKepzesHozzaadasa(object sender, RoutedEventArgs e)
		{
			if (cbKepzesek.SelectedItem != null)
			{
				cbKivalasztottKepzes.Items.Add(cbKepzesek.SelectedItem.ToString());
				cbKivalasztottKepzes.SelectedIndex = 0;
			}
		}

		private void cAddKepzes(object sender, RoutedEventArgs e)
		{
			//gAddKepzes.Visibility = Visibility.Visible;
			//gAddOktato.Visibility = Visibility.Collapsed;
			//gAddResztvevo.Visibility = Visibility.Collapsed;

			//gVisualize.Visibility = Visibility.Collapsed;
			//gEdit.Visibility = Visibility.Collapsed;
			HideGrids(gAddKepzes);
			FillLists();
		}

		private void cAddOktato(object sender, RoutedEventArgs e)
		{
			//gAddKepzes.Visibility = Visibility.Collapsed;
			//gAddOktato.Visibility = Visibility.Visible;
			//gAddResztvevo.Visibility = Visibility.Collapsed;

			//gVisualize.Visibility = Visibility.Collapsed;
			//gEdit.Visibility = Visibility.Collapsed;
			HideGrids(gAddOktato);
			FillLists();
		}

		private void cAddResztvevo(object sender, RoutedEventArgs e)
		{
			//gAddKepzes.Visibility = Visibility.Collapsed;
			//gAddOktato.Visibility = Visibility.Collapsed;
			//gAddResztvevo.Visibility = Visibility.Visible;

			//gVisualize.Visibility = Visibility.Collapsed;
			//gEdit.Visibility = Visibility.Collapsed;

			HideGrids(gAddResztvevo);
			FillLists();
		}

		private void ClickSaveResztvevo(object sender, RoutedEventArgs e)
		{
			if (tbResztvevoNev.Text == "") { MessageBox.Show("Üres résztvevő név", "Hiba", MessageBoxButton.OK); return; }
			if (tbBeosztas.Text == "") { MessageBox.Show("Üres beosztás", "Hiba", MessageBoxButton.OK); return; }

			List<Kepzes> kepzesek = tabla.Kepzesek.ToList().Where(x => cbKivalasztottKepzes2.Items.Contains(x.Nev)).ToList();

			Resztvevo newResztvevo = new Resztvevo { Nev = tbResztvevoNev.Text, Beosztas = tbBeosztas.Text, Kepzeseks = kepzesek };

			tabla.Resztvevok.Add(newResztvevo);
			tabla.SaveChanges();

			MessageBox.Show("Mentés sikeres");
			FillLists();
			tbResztvevoNev.Text = "";
			tbBeosztas.Text = "";
			cbKivalasztottKepzes2.ItemsSource = null;
		}

		private void ClickKivalasztottKepzesHozzaadasa2(object sender, RoutedEventArgs e)
		{
			if (cbKepzesek2.SelectedItem != null)
			{
				cbKivalasztottKepzes2.Items.Add(cbKepzesek2.SelectedItem.ToString());
				cbKivalasztottKepzes2.SelectedIndex = 0;
			}
		}

		private void kepzesKerese(object sender, TextChangedEventArgs e)
		{
			TextBox searchBar = sender as TextBox;

			if (searchBar.Text == "")
			{
				dgKepzes.ItemsSource = tabla.Kepzesek.ToList();
			}
			else
			{
				List<Kepzes> eredmeny = new List<Kepzes>();


				foreach (Kepzes kepzes in tabla.Kepzesek.Include(x => x.Oktatok).Include(x => x.Resztvevoks))
				{
					if (kepzes.Nev.Contains(searchBar.Text)
						|| kepzes.ID.ToString().Contains(searchBar.Text)
						|| kepzes.KezdesDatuma.ToString().Contains(searchBar.Text)
						|| kepzes.BefejezesDatuma.ToString().Contains(searchBar.Text)
						|| kepzes.Hely.Contains(searchBar.Text)
						|| kepzes.OktatokLista.Contains(searchBar.Text)
						|| kepzes.ResztvevokLista.Contains(searchBar.Text))
					{
						eredmeny.Add(kepzes);
					}
				}


				dgKepzes.ItemsSource = eredmeny;
				KepzesTalalatok.Content = $"Találatok száma: {eredmeny.Count}";
			}
		}

		private void cSearchKepzes(object sender, RoutedEventArgs e)
		{
			dgKepzes.ItemsSource = tabla.Kepzesek.Include(x=> x.Oktatok).Include(x=> x.Resztvevoks).ToList();
			HideGrids(gSearchKepzes);
		}

		private void cSearchOktato(object sender, RoutedEventArgs e)
		{
			dgOktato.ItemsSource = tabla.Oktatok.Include(x=> x.Kepzesek).ToList();
			HideGrids(gSearchOktato);
		}

		private void cSearchResztvevo(object sender, RoutedEventArgs e)
		{
			dgResztvevo.ItemsSource = tabla.Resztvevok.Include(x=> x.Kepzeseks).ToList();
			HideGrids(gSearchResztvevo);
		}

		/// <summary>
		/// Iterálja a regisztrált oldalakat és megjeleníti a pereméterben megadott oldalt(Grid)
		/// </summary>
		/// <param name="kihagy"></param>
		private void HideGrids(Grid kihagy)
		{
			foreach (Grid g in oldalak)
			{
				if (g == kihagy)
				{
					g.Visibility = Visibility.Visible;
				}
				else
				{
					g.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void oktatoKerese(object sender, TextChangedEventArgs e)
		{
			TextBox searchBar = sender as TextBox;

			if (searchBar.Text == "")
			{
				dgOktato.ItemsSource = tabla.Oktatok.ToList();
			}
			else
			{
				List<Oktato> eredmeny = new List<Oktato>();


				foreach (Oktato oktato in tabla.Oktatok.Include(x => x.Kepzesek))
				{
					if (oktato.Nev.Contains(searchBar.Text)
						|| oktato.ID.ToString().Contains(searchBar.Text)
						|| oktato.Szakterulet.Contains(searchBar.Text)
						|| oktato.KepzesekLista.Contains(searchBar.Text))
					{
						eredmeny.Add(oktato);
					}
				}


				dgOktato.ItemsSource = eredmeny;
				OktatoTalalatok.Content = $"Találatok száma: {eredmeny.Count}";
			}
		}

		private void resztvevoKerese(object sender, TextChangedEventArgs e)
		{
			TextBox searchBar = sender as TextBox;

			if (searchBar.Text == "")
			{
				dgResztvevo.ItemsSource = tabla.Resztvevok.Include(x => x.Kepzeseks).ToList();
			}
			else
			{
				List<Resztvevo> eredmeny = new List<Resztvevo>();


				foreach (Resztvevo resztvevo in tabla.Resztvevok.Include(x => x.Kepzeseks))
				{
					if (resztvevo.Nev.Contains(searchBar.Text)
						|| resztvevo.ID.ToString().Contains(searchBar.Text)
						|| resztvevo.Beosztas.Contains(searchBar.Text)
						|| resztvevo.KepzesekLista.Contains(searchBar.Text))
					{
						eredmeny.Add(resztvevo);
					}
				}


				dgResztvevo.ItemsSource = eredmeny;
				ResztvevoTalalatok.Content = $"Találatok száma: {eredmeny.Count}";
			}
		}

		private void editKepzes(object sender, RoutedEventArgs e)
		{
			HideGrids(gEditOktato);
			cbEditOktatoID.ItemsSource = tabla.Oktatok.Select(x=> x.ID).ToList();
			cbEditKepzesek2.ItemsSource = tabla.Kepzesek.Select(x => x.Nev).ToList();
		}

		private void editOktato(object sender, RoutedEventArgs e)
		{

		}

		private void editresztvevo(object sender, RoutedEventArgs e)
		{

		}

		private void KivalasztottOktato(object sender, SelectionChangedEventArgs e)
		{
			
			Oktato oktato = tabla.Oktatok.Include(x=> x.Kepzesek).Where(x => x.ID == int.Parse(cbEditOktatoID.SelectedItem.ToString())).First();

			tbEditOktatoNev.Text = oktato.Nev;
			tbEditOktatoSzakterulet.Text = oktato.Szakterulet;

			cbEditKivalasztottKepzes2.ItemsSource = oktato.Kepzesek.Select(x=> x.Nev).ToList();
			

		}

		private void editKivalasztottKepzesHozzaadasa2(object sender, RoutedEventArgs e)
		{
			
			List<string> handler = (List<string>)cbEditKivalasztottKepzes2.ItemsSource;
			
			handler.Add(cbEditKepzesek2.SelectedItem.ToString());
			cbEditKivalasztottKepzes2.ItemsSource = null;
			cbEditKivalasztottKepzes2.ItemsSource = handler;
		}

		private void EditValszottKepzesTorles(object sender, RoutedEventArgs e)
		{

			List<string> handler = (List<string>)cbEditKivalasztottKepzes2.ItemsSource;

			if (handler.Contains(cbEditKepzesek2.SelectedItem.ToString()))
			{
				handler.Remove(cbEditKepzesek2.SelectedItem.ToString());
				
			}
			cbEditKivalasztottKepzes2.ItemsSource = null;
			cbEditKivalasztottKepzes2.ItemsSource = handler;
		}

		private void EditSaveOktato(object sender, RoutedEventArgs e)
		{


			Oktato oktato = tabla.Oktatok.Include(x => x.Kepzesek).Where(x => x.ID == int.Parse(cbEditOktatoID.SelectedItem.ToString())).First();
			if(oktato.Nev == "") { MessageBox.Show("Üres név mező","Hiba", MessageBoxButton.OK); return;  };
			if (oktato.Szakterulet == "") { MessageBox.Show("Üres név mező", "Hiba", MessageBoxButton.OK); return; };
			oktato.Nev = tbEditOktatoNev.Text;
			oktato.Szakterulet=tbEditOktatoSzakterulet.Text;

			
			oktato.Kepzesek = tabla.Kepzesek.ToList().Where(x=> cbEditKivalasztottKepzes2.Items.Contains(x.Nev)).ToList();

			tabla.SaveChanges();
			MessageBox.Show("Adatok mentve", "Info", MessageBoxButton.OK);
		}
	}
}