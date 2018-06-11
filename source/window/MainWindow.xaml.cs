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

				Game game = new Game(gameWindow, 5, 4);
				game.Play();
			};

			var g = new Grid();
			this.Content = g;
			g.RowDefinitions.Add(new RowDefinition());
			g.RowDefinitions.Add(new RowDefinition());
			g.RowDefinitions.Add(new RowDefinition());

			g.ColumnDefinitions.Add(new ColumnDefinition());
			g.ColumnDefinitions.Add(new ColumnDefinition());
			g.ColumnDefinitions.Add(new ColumnDefinition());

			var c = new Canvas();
			g.Children.Add(c);

			var r = new Grid() { Height = 100, Width = 150};
			r.Children.Add(new Ellipse() { Fill = Brushes.Aqua, Stroke = Brushes.Red });
			r.Children.Add(new Label() { Content="142" });
			Canvas.SetZIndex(r, 3);
			Canvas.SetTop(r, 100);
			Canvas.SetLeft(r, 100);
			c.Children.Add(r);
		}
	}
}