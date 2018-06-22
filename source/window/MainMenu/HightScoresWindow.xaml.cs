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
	/// Interaction logic for HightScores.xaml
	/// </summary>
	public partial class HightScoresWindow : Window {
		public HightScoresWindow() {
			InitializeComponent();

			this.MinHeight = 450;
			this.MinWidth = 800;
			this.MaxHeight = 450;
			this.MaxWidth = 800;

			double size = (this.Width - 20) / 3;
			Thickness e = new Thickness(0, 5, 5, 5);
			StackPanel d = new StackPanel {
				Orientation = Orientation.Horizontal
			};

			Label f = new Label();
			Label w = new Label();
			Label r = new Label();

			f.HorizontalContentAlignment = HorizontalAlignment.Center;
			r.HorizontalContentAlignment = HorizontalAlignment.Center;
			w.HorizontalContentAlignment = HorizontalAlignment.Center;

			f.Content = "456";
			r.Content = "123";
			w.Content = "789";

			r.Width = size;
			w.Width = size;
			f.Width = size;

			w.BorderThickness = e;
			f.BorderThickness = e;
			r.BorderThickness = e;

			d.Children.Add(w);
			d.Children.Add(f);
			d.Children.Add(r);

			list.Items.Add(d);
		}
	}
}
