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
			var o1 = new Oktato { Kepzesek = [k1], Nev = "Saniy", Szakterulet = "Otthon" };

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
		}

		private void cVisualize(object sender, RoutedEventArgs e)
		{
			gAdd.Visibility = Visibility.Collapsed;
			gVisualize.Visibility = Visibility.Visible;
			gEdit.Visibility = Visibility.Collapsed;
            dgMegjelen.ItemsSource = tabla.Kepzesek.Include(p => p.Oktatok).Include(p => p.Resztvevoks).ToList();
			//dgMegjelen.ItemsSource = tabla.Kepzesek.ToList();
			//KepzesOktato
			Trace.WriteLine("asd"+string.Join(", ", tabla.Kepzesek.ToList()));

		}

		private void cEdit(object sender, RoutedEventArgs e)
		{
			gAdd.Visibility = Visibility.Collapsed;
			gVisualize.Visibility = Visibility.Collapsed;
			gEdit.Visibility = Visibility.Visible;
		}
    }
}