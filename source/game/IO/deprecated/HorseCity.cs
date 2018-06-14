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
	
		protected override void FillShape()
		{
			if (!shape.Children.Contains(units))
				shape.Children.Add(units);
			//Shape
			double min = settings.size.OneCellSizeX < settings.size.OneCellSizeY ? settings.size.OneCellSizeX : settings.size.OneCellSizeY;
			label = new Label();
			label.Width = min * settings.size.sitySizeMult;
			label.Height = min * settings.size.sitySizeMult;
			switch (settings.values.style_Num)
			{
				case 0:
					label.Background = settings.colors.neutralTownFill;
					label.Style = (Style)label.FindResource("HorseCityStyle");
					SetUiColor(this.label, this.playerId);
					break;
				case 1:
					label.Style = (Style)label.FindResource("ColorCityStyle1");
					label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\stable_p0_s4_l5.png", UriKind.Relative)) };
					SetImgColor(label, playerId);
					break;
			}
			
			shape.Children.Add(label);

			text = new Label()
			{
				Foreground = Brushes.Black,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Width = min * settings.size.sitySizeMult,
				Height = min * settings.size.sitySizeMult,
			};
			label.Content = text.Content;
		}

		public override void SetImgColor(Label label, byte playerId)
		{
			if (playerId == 1)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\stable_p1_s4_l5.png", UriKind.Relative)) };
			}
			else if (playerId == 2)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\stable_p2_s4_l5.png", UriKind.Relative)) };
			}
			else if (playerId == 4)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\stable_p3_s4_l5.png", UriKind.Relative)) };
			}
			else if (playerId == 3)
			{
				label.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(@"..\..\img\cities\stable_p4_s4_l5.png", UriKind.Relative)) };
			}
		}
	}
}
