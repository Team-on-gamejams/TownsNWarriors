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

namespace TownsAndWarriors {
	partial class GameCell {
		public override void InitializeShape() {
			shape = new Rectangle();

			if (IsOpenLeft && IsOpenBottom)
				shape.Fill = Brushes.Orange;
			else if(IsOpenLeft || IsOpenRight)
				shape.Fill = Brushes.Red;
			else if (IsOpenTop || IsOpenBottom)
				shape.Fill = Brushes.Blue;
			else
				shape.Fill = Brushes.Black;
		}

		public override void DrawOnGameCell(int x, int y) {
			if (shape == null)
				InitializeShape();
			Grid.SetRow(shape, y);
			Grid.SetColumn(shape, x);
			grid.Children.Add(shape);
		}
	}
}
