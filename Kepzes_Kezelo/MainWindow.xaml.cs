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
namespace Kepzes_Kezelo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{
		KepzesekTabla kepzesek;

		public MainWindow()
		{

			InitializeComponent();
			kepzesek = new KepzesekTabla();
			Oktato oktato = new Oktato { Nev ="Sanyi", Szakterulet = "Sebész"};
			
			
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
            //dgMegjelen.ItemsSource = cnKepzesek.Include(p => p.Oktatók).Include(p => p.Résztvevők).ToList();
        }

		private void cEdit(object sender, RoutedEventArgs e)
		{
			gAdd.Visibility = Visibility.Collapsed;
			gVisualize.Visibility = Visibility.Collapsed;
			gEdit.Visibility = Visibility.Visible;
		}

        private void mi_KilépésClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}