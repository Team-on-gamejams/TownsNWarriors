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

using System.Windows.Media.Animation;

using System.Reflection;

namespace TownsAndWarriors.game.sity
{
	public partial class BasicSity
	{
		Label text;

		public static List<BasicSity> selected = new List<BasicSity>();

		public override void InitializeShape()
		{
			shape = new Grid();
			shape.Style = (Style)shape.FindResource("BasicCityStyle");
			FillShape();

			//Delegates
			settings.size.SizeChanged += () => {
				shape.Children.Clear();
				FillShape();
			};

			shape.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
			{
				if (playerId == 1)
					selected.Add(this);
				if (playerId != 1)
				{
					//MessageBox.Show(gameMap.ToString());
					gameMap.SendWarriors(selected, this);
					selected.Clear();
				}
			};
			//shape.Children.Add(newRec);

			//тут створювати всі собитія з городом

		}
		public override void UpdateValue()
		{
				text.Content = this.currWarriors.ToString() + '/' + maxWarriors.ToString();
		}

		void FillShape() {
			//Elipse
			double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;
			var elipse = new Ellipse() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Fill = settings.colors.neutralTownFill,
				Stroke = settings.colors.neutralTownStroke,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};

			if (playerId == 1) {
				elipse.Fill = settings.colors.playerTownFill;
				elipse.Stroke = settings.colors.playerTownStroke;
			}
			else if (playerId != 0) {
				if (settings.colors.TownFills.Count <= playerId - 2)
					elipse.Fill = settings.colors.TownFills[settings.colors.TownFills.Count - 1];
				else
					elipse.Fill = settings.colors.TownFills[playerId - 2];

				if (settings.colors.TownStrokes.Count <= playerId - 2)
					elipse.Stroke = settings.colors.TownStrokes[settings.colors.TownStrokes.Count - 1];
				else
					elipse.Stroke = settings.colors.TownStrokes[playerId - 2];
			}

			shape.Children.Add(elipse);

			text = new Label() {
				Foreground = Brushes.Black,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};
			shape.Children.Add(text);

			shape.MouseEnter += (a, b)=>{
				var anim = new System.Windows.Media.Animation.DoubleAnimation {
					From = min * settings.size.sitySizeMult,
					To = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				elipse.BeginAnimation(Ellipse.WidthProperty, anim);
				elipse.BeginAnimation(Ellipse.HeightProperty, anim);
			};
			shape.MouseLeave += (a, b) => {
				var anim = new System.Windows.Media.Animation.DoubleAnimation {
					To = min * settings.size.sitySizeMult,
					From  = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				elipse.BeginAnimation(Ellipse.WidthProperty, anim);
				elipse.BeginAnimation(Ellipse.HeightProperty, anim);
			};
		}
	}
}