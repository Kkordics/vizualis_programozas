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
	}
}