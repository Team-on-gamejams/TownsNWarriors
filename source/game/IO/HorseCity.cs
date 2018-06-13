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
	public partial class HorseCity
	{
		Label label;
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

			shape.MouseEnter += (a, b) => {
				double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;

				var anim = new System.Windows.Media.Animation.DoubleAnimation
				{
					From = min * settings.size.sitySizeMult,
					To = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				label.BeginAnimation(Ellipse.WidthProperty, anim);
				label.BeginAnimation(Ellipse.HeightProperty, anim);
			};
			shape.MouseLeave += (a, b) => {
				double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;

				var anim = new System.Windows.Media.Animation.DoubleAnimation
				{
					To = min * settings.size.sitySizeMult,
					From = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				label.BeginAnimation(Ellipse.WidthProperty, anim);
				label.BeginAnimation(Ellipse.HeightProperty, anim);
			};

			shape.MouseRightButtonDown += delegate (object sender, MouseButtonEventArgs e)
			{
				selected.Clear();
			};

			shape.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e) {
				if (playerId == 1)
				{
					if (selected.Contains(this) == false)
					{
						selected.Add(this);
						label.BorderBrush = settings.colors.citySelectedStroke;
						label.BorderThickness = new Thickness(settings.colors.cityPassiveStrokeThickness);
					}
				}
				else if (playerId != 1)
				{
					gameMap.SendWarriors(selected, this);
					foreach (var x in selected)
					{
						if (x is BasicSity)
						SetElipseColor(x.CityModel, x.playerId);
						else
						{
							SetUiColor(((HorseCity)x).label, x.playerId);
						}
					}
					selected.Clear();
				}
			};
		}
		protected override void FillShape()
		{
			//Shape
			double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;
			cityModel = new Rectangle()
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Fill = settings.colors.neutralTownFill,
				Stroke = settings.colors.neutralTownStroke,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};
			label = new Label();
			label.Style = (Style)label.FindResource("HorseCityStyle");
			label.VerticalAlignment = VerticalAlignment.Center;
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.Background = settings.colors.neutralTownFill;
			//Stroke = settings.colors.neutralTownStroke;
			label.Width = min * settings.size.sitySizeMult;
			label.Height = min * settings.size.sitySizeMult;

			//SetElipseColor(this.cityModel, this.playerId);
			SetUiColor(this.label, this.playerId);
			//shape.Children.Add(cityModel);
			shape.Children.Add(label);

			text = new Label()
			{
				Foreground = Brushes.Black,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};
			shape.Children.Add(text);
		}

		void SetUiColor(Label label, byte playerId)
		{
			if (playerId == 1)
			{
				label.Background = settings.colors.playerTownFill;
				label.BorderBrush = settings.colors.playerTownStroke;
			}
			else if (playerId != 0)
			{
				if (settings.colors.TownFills.Count <= playerId - 2)
					label.Background = settings.colors.TownFills[settings.colors.TownFills.Count - 1];
				else
					label.Background = settings.colors.TownFills[playerId - 2];
			}
			if (settings.colors.TownStrokes.Count <= playerId - 2)
				label.BorderBrush = settings.colors.TownStrokes[settings.colors.TownStrokes.Count - 1];
			else
				//label.BorderBrush = settings.colors.TownStrokes[playerId - 2];

			label.BorderThickness = new Thickness(settings.colors.cityPassiveStrokeThickness);
		}
	}
}
