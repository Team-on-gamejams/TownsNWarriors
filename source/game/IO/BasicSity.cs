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
		protected Label label;
		public Label Label
		{
			get
			{
				return label;
			}
		}
		protected Label text;
		protected Shape cityModel;

		protected Label selection;

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

			selection = new Label();
			selection.Background = new ImageBrush(new BitmapImage(new Uri(@"..\..\img\war\our_selector.png", UriKind.Relative)));
			selection.Opacity = 0;
			Grid.SetZIndex(selection, 10);
			shape.Children.Add(selection);

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
				label.BeginAnimation(Label.WidthProperty, anim);
				label.BeginAnimation(Label.HeightProperty, anim);
			};
			shape.MouseLeave += (a, b) => {
				double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;

				var anim = new System.Windows.Media.Animation.DoubleAnimation {
					To = min * settings.size.sitySizeMult,
					From = min,
					Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100)),
				};
				label.BeginAnimation(Label.WidthProperty, anim);
				label.BeginAnimation(Label.HeightProperty, anim);
			};

			grid.MouseRightButtonDown += delegate (object sender, MouseButtonEventArgs e)
			{
				foreach (var x in selected)
				{
					switch (settings.values.style_Num)
					{
						case 0:
							SetUiColor(this.label, this.playerId);
							break;
						case 1:
							SetImgColor(this.label, this.playerId);
							break;
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
							selection.Opacity = 1;
							label.BorderBrush = settings.colors.citySelectedStroke;
							label.BorderThickness = new Thickness(settings.colors.citySelectedStrokeThickness);

						}
					}
					else if (playerId != 1)
					{
						gameMap.SendWarriors(selected, this);
						foreach (var x in selected)
						{
							x.selection.Opacity = 0;
							switch (settings.values.style_Num)
							{
								case 0:
									SetUiColor(this.label, this.playerId);
									break;
								case 1:
									SetImgColor(this.label, this.playerId);
									break;
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
							switch (settings.values.style_Num)
							{
								case 0:
									SetUiColor(this.label, this.playerId);
									break;
								case 1:
									SetImgColor(this.label, this.playerId);
									break;
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

			label = new Label();
			label.Width = min * settings.size.sitySizeMult;
			label.Height = min * settings.size.sitySizeMult;
			switch (settings.values.style_Num)
			{
				case 0:
					label.Background = settings.colors.neutralTownFill;
					label.Style = (Style)label.FindResource("BaseCityStyle");
					SetUiColor(this.label, this.playerId);
					break;
				case 1:
					label.Style = (Style)label.FindResource("ColorCityStyle1");
					label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\city_p0_s4_l5.png", UriKind.Relative)) };
					SetImgColor(this.label, this.playerId);
					break;
			}

			shape.Children.Add(label);

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

			label.BorderThickness = new Thickness(settings.colors.cityPassiveStrokeThickness);
		}

		public virtual void SetImgColor(Label label, byte playerId)
		{
			if (playerId == 1)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\city_p1_s4_l5.png", UriKind.Relative)) };
			}
			else if (playerId == 2)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\city_p2_s4_l5.png", UriKind.Relative)) };
			}
			else if (playerId == 4)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\city_p3_s4_l5.png", UriKind.Relative)) };
			}
			else if (playerId == 3)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\city_p4_s4_l5.png", UriKind.Relative)) };
			}
		}
	}
}