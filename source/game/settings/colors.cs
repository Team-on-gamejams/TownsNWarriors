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

namespace TownsAndWarriors.game.settings {
	static class colors {
		public static Brush roadBackground = Brushes.Black;
		public static Brush roadStroke = Brushes.White;
		public static double roadStrokeThickness = 0.3;

		public static Brush neutralTownFill = Brushes.Wheat;
		public static Brush neutralTownStroke = Brushes.Black;

		public static Brush playerTownFill = Brushes.Red;
		public static Brush playerTownStroke = Brushes.Black;

		public static Brush citySelectedStroke = Brushes.Yellow;
		public static double citySelectedStrokeThickness = 2;

		public static List<Brush> TownFills = new List<Brush>() {
			Brushes.Blue,
			Brushes.Teal,
			Brushes.Purple,
			Brushes.Yellow,
			Brushes.Orange,
			Brushes.Green,
			Brushes.Pink,
			Brushes.Gray,
			Brushes.LightBlue,
			Brushes.DarkGreen,
			Brushes.Brown,
			Brushes.Maroon,
			Brushes.Navy,
			Brushes.Navy,
			Brushes.Turquoise,
			Brushes.Violet,
			Brushes.Wheat,
			Brushes.PeachPuff,
			Brushes.MintCream,
			Brushes.Lavender,
			Brushes.DarkGray,
			Brushes.Snow,
			Brushes.DarkGreen,
			Brushes.Peru,
		};
		public static List<Brush> TownStrokes = new List<Brush>() {
			Brushes.Black
		};
	}
}
