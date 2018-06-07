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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TownsAndWarriors {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			this.Background = new SolidColorBrush(new Color() { B = f, R = f, G = f, A = 255 });
			this.KeyDown += MainWindow_Initialized;
		}

		Random r = new Random();
		byte f => (byte)r.Next(0, 256);

		static int a = 0;
		private void MainWindow_Initialized(object sender, EventArgs e) {
			if (++a != 10) {
				System.Threading.Thread.Sleep(100);
				var a = new MainWindow();
				a.Show();
				a.Top = this.Top;
				a.Left = this.Left;
				a.Height = this.Height;
				a.Width = this.Width;
				this.Close();
			}
		}
	}
}