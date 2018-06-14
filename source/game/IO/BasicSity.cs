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
		protected Label text;
		protected Shape cityModel;

		public Shape CityModel
		{
			get { return cityModel; }
		}

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

			shape.MouseEnter += (a, b) => {
				double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;

				var anim = new System.Windows.Media.Animation.DoubleAnimation {
					From = min * settings.size.sitySizeMult,
					To = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				cityModel.BeginAnimation(Ellipse.WidthProperty, anim);
				cityModel.BeginAnimation(Ellipse.HeightProperty, anim);
			};
			shape.MouseLeave += (a, b) => {
				double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;

				var anim = new System.Windows.Media.Animation.DoubleAnimation {
					To = min * settings.size.sitySizeMult,
					From = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				cityModel.BeginAnimation(Ellipse.WidthProperty, anim);
				cityModel.BeginAnimation(Ellipse.HeightProperty, anim);
			};

			grid.MouseRightButtonDown += delegate (object sender, MouseButtonEventArgs e)
			{
				foreach (var x in selected)
				{
					if (x is HorseCity)
					{
						SetUiColor(((HorseCity)x).Label, x.playerId);
					}
					else
					{
						SetElipseColor(x.CityModel, x.playerId);
					}
				}
				selected.Clear();
			};

			shape.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e) {
				if (e.ClickCount == 1)
				{
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
							if (x is HorseCity)
							{
								SetUiColor(((HorseCity)x).Label, x.playerId);
							}
							else
							{
								SetElipseColor(x.CityModel, x.playerId);
							}
						}
						selected.Clear();
					}
				}
				else
				{
					if (playerId == 1)
					{
						gameMap.SendWarriors(selected, this);
						foreach (var x in selected)
						{
							if (x is HorseCity)
							{
								SetUiColor(((HorseCity)x).Label, x.playerId);
							}
							else
							{
								SetElipseColor(x.CityModel, x.playerId);
							}
						}
						selected.Clear();
					}
				}
			};
		}

		public override void UpdateValue()
		{
			text.Content = this.currWarriors.ToString() + '/' + maxWarriors.ToString();
		}

		protected virtual void FillShape() {
			//Elipse
			double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;
			cityModel = new Ellipse() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Fill = settings.colors.neutralTownFill,
				Stroke = settings.colors.neutralTownStroke,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};

			SetElipseColor(this.cityModel, this.playerId);
			shape.Children.Add(cityModel);

			text = new Label() {
				Foreground = Brushes.Black,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};
			shape.Children.Add(text);
		}

		static public void SetElipseColor(Shape elipse, byte playerId) {
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
			elipse.StrokeThickness = settings.colors.cityPassiveStrokeThickness;
		}

		static public void SetUiColor(Label label, byte playerId)
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
			//else
			//	label.BorderBrush = settings.colors.TownStrokes[playerId - 2];

				label.BorderThickness = new Thickness(settings.colors.cityPassiveStrokeThickness);
		}

		static public void SetImgColor(Label label, byte playerId)
		{
			if (playerId == 1)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage() { UriSource = new Uri(@"..\..\img\cities\stable_p1_s4_l5.png", UriKind.Relative) } };
			}
		}
	}
}