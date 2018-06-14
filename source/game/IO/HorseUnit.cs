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

using TownsAndWarriors.game.basicInterfaces;

namespace TownsAndWarriors.game.unit
{
	public partial class HorseUnit
	{
		protected override void FillShape()
		{
			RecalcGeometrySize();

			rectangle = new Ellipse()
			{
				Fill = settings.colors.neutralTownFill,
				Stroke = settings.colors.neutralTownStroke,
				Width = shape.Width,
				Height = shape.Height,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			sity.BasicSity.SetElipseColor(rectangle, this.playerId);
			shape.Children.Add(rectangle);

			text = new Label()
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			shape.Children.Add(text);

			UpdateValue();
		}
	}
}
