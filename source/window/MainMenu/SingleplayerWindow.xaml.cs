using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace taw.window {
	/// <summary>
	/// Interaction logic for SingleplayerWindow.xaml
	/// </summary>
	public partial class SingleplayerWindow : Window {
		//---------------------------- Fields ----------------------------
		public bool IsBack = false;
		public bool IsExit = false;

		//---------------------------- Init ----------------------------
		public SingleplayerWindow() {
			InitializeComponent();
		}


		//---------------------------- Click events ----------------------------
		private void Button_Click_Campaign(object sender, RoutedEventArgs e) {

		}

		private void Button_Click_Load_Game(object sender, RoutedEventArgs e) {

		}

		private void Button_Click_Select_Map(object sender, RoutedEventArgs e) {

		}

		private void Button_Click_Random_Map(object sender, RoutedEventArgs e) {
			
		}

		private void Button_Click_Back(object sender, RoutedEventArgs e) {
			IsBack = true;
			this.Close();
		}

		private void Button_Click_Exit(object sender, RoutedEventArgs e) {
			IsExit = true;
			Button_Click_Back(sender, e);
		}

		//---------------------------- Support ----------------------------

	}
}
