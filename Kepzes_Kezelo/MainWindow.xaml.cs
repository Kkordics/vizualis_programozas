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

		public MainWindow()
		{

			InitializeComponent();
			tabla = new KepzesekTabla();
			//Oktato oktato = new Oktato { Nev ="Sanyi", Szakterulet = "Sebész"};
			Initdb();
			
		}

		private void Initdb()
		{
			tabla.Database.EnsureCreated();
			if(tabla.Kepzesek != null)
			{
				
				if (!tabla.Kepzesek.Any())
				{
					Populate();
				}
			}
		}

		private void Populate()
		{
			var k1 = new Kepzes { Hely = "Szeged", Nev = "ITBiz", BefejezesDatuma = DateTime.Now , KezdesDatuma = DateTime.Today};
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

		private void cAdd(object sender, RoutedEventArgs e)
		{
			gAdd.Visibility = Visibility.Visible;
			gVisualize.Visibility = Visibility.Collapsed;
			gEdit.Visibility = Visibility.Collapsed;
			cbOktatok.ItemsSource = tabla.Oktatok.Select(x => x.Nev).ToList();
			cbResztvevo.ItemsSource = tabla.Resztvevok.Select(x => x.Nev).ToList();
		}

		private void cVisualize(object sender, RoutedEventArgs e)
		{
			gAdd.Visibility = Visibility.Collapsed;
			gVisualize.Visibility = Visibility.Visible;
			gEdit.Visibility = Visibility.Collapsed;
            dgMegjelen.ItemsSource = tabla.Kepzesek.Include(p => p.Oktatok).Include(p => p.Resztvevoks).ToList();
			
		}

		private void cEdit(object sender, RoutedEventArgs e)
		{
			gAdd.Visibility = Visibility.Collapsed;
			gVisualize.Visibility = Visibility.Collapsed;
			gEdit.Visibility = Visibility.Visible;
		}

		private void ClickTodayStart(object sender, RoutedEventArgs e)
		{
			tbDateStart.Text = DateTime.Now.ToShortDateString();
			
		}

		private void ClickTodayEnd(object sender, RoutedEventArgs e)
		{
			tbDateEnd.Text = DateTime.Now.ToShortDateString();
		}

		private void CliclSaveKepzesek(object sender, RoutedEventArgs e)
		{
			if(tbName.Text == "") { MessageBox.Show("Üres név", "Hiba", MessageBoxButton.OK); return; }
			if(tbDateStart.Text == "") { MessageBox.Show("Üres dátum", "Hiba", MessageBoxButton.OK); return; }
			if(tbDateEnd.Text == "") { MessageBox.Show("Üres dátum", "Hiba", MessageBoxButton.OK); return; }
			if(tbPlace.Text == "") { MessageBox.Show("Üres hely", "Hiba", MessageBoxButton.OK); return; }

			
			//az oktató már létezik
			if(tabla.Kepzesek.Any(x=> x.Nev == tbName.Text)) { MessageBox.Show("Létező név", "Hiba", MessageBoxButton.OK); return; }

			if(DateTime.TryParse(tbDateStart.Text, out DateTime start) && DateTime.TryParse(tbDateEnd.Text, out DateTime end))
			{
				
				List<Oktato> oktatok = tabla.Oktatok.ToList().Where(x=> cbHozzaadotOktatok.Items.Contains(x.Nev)).ToList();
				List<Resztvevo> resztvevok = tabla.Resztvevok.ToList().Where(x=> cbHozzaadotResztvevok.Items.Contains(x.Nev)).ToList();

				Kepzes newKepzes = new Kepzes { Nev = tbName.Text, KezdesDatuma = start, BefejezesDatuma = end, Hely = tbPlace.Text, Oktatok = oktatok, Resztvevoks = resztvevok};

				tabla.Kepzesek.Add(newKepzes);
				tabla.SaveChanges();
				
				MessageBox.Show("Mentés sikeres");
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
			if(cbOktatok.SelectedItem != null)
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

		
	}
}