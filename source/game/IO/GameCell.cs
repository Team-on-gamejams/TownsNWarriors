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

namespace TownsAndWarriors.game.map {
	partial class GameCell {
		public override void InitializeShape() {
			shape = new Grid();

			var rect = new Rectangle();
			if (IsOpenLeft && IsOpenBottom)
				rect.Fill = Brushes.Orange;
			else if(IsOpenLeft || IsOpenRight)
				rect.Fill = Brushes.Red;
			else if (IsOpenTop || IsOpenBottom)
				rect.Fill = Brushes.Blue;
			else
				rect.Fill = Brushes.Black;
			shape.Children.Add(rect);
		}
	}
}
