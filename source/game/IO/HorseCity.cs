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
				cityModel.BeginAnimation(Ellipse.WidthProperty, anim);
				cityModel.BeginAnimation(Ellipse.HeightProperty, anim);
			};
			shape.MouseLeave += (a, b) => {
				double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;

				var anim = new System.Windows.Media.Animation.DoubleAnimation
				{
					To = min * settings.size.sitySizeMult,
					From = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				cityModel.BeginAnimation(Ellipse.WidthProperty, anim);
				cityModel.BeginAnimation(Ellipse.HeightProperty, anim);
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
						cityModel.Stroke = settings.colors.citySelectedStroke;
						cityModel.StrokeThickness = settings.colors.citySelectedStrokeThickness;
					}
				}
				else if (playerId != 1)
				{
					gameMap.SendWarriors(selected, this);
					foreach (var x in selected)
					{
						SetElipseColor(x.CityModel, x.playerId);
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
			Style newStyle = new Style(typeof(Rectangle));
			//Polygon polygon = new Polygon();
			//polygon.Points = new PointCollection();
			//polygon.Points.Add(new Point(0, 20));
			//polygon.Points.Add(new Point(20, 0));
			//polygon.Points.Add(new Point(20, 20));
			//polygon.Height = min * settings.size.sitySizeMult;
			//polygon.Width = min * settings.size.sitySizeMult;
			//polygon.VerticalAlignment = VerticalAlignment.Center;
			//polygon.HorizontalAlignment = HorizontalAlignment.Center;
			//polygon.Fill = settings.colors.neutralTownFill;
			//polygon.Stroke = settings.colors.neutralTownStroke;

			//cityModel = polygon;

			SetElipseColor(this.cityModel, this.playerId);
			shape.Children.Add(cityModel);

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
	}
}
