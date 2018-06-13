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

using TownsAndWarriors.game;


namespace TownsAndWarriors.window {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			this.KeyDown += (a, b) => {
				GameWindow gameWindow = new GameWindow();
				gameWindow.Show();
				this.Close();
				//gameWindow.Top = this.Top;
				//gameWindow.Left = this.Left;
				//gameWindow.Height = this.Height;
				//gameWindow.Width = this.Width;

				Game game = new Game(gameWindow, TownsAndWarriors.game.settings.values.fieldSizeX, TownsAndWarriors.game.settings.values.fieldSizeY);
				game.Play();
			};
		}
	}
}