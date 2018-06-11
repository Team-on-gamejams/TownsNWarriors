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

namespace TownsAndWarriors.game.unit {
	public partial class BasicUnit {
		Label text;
		Canvas canvas;
		double cellSizeX, cellSizeY;
		double pixelPerTurnX, pixelPerTurnY;
		double shiftX, shiftY;

		public void SetCanvas(Canvas canvas) => this.canvas = canvas;
		public void SetOneCellSize(int X, int Y) { cellSizeX = X; cellSizeY = Y; }

		public override void InitializeShape() {
			text = new Label() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};

			shape = new Grid() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			shape.Width = cellSizeX / 2;
			shape.Height = cellSizeY / 2;
			shape.Children.Add(new Ellipse() {
				Fill = Brushes.Pink,
				Stroke = Brushes.Black,
				Width = shape.Width,
				Height = shape.Height,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			});
			shape.Children.Add(text);

			pixelPerTurnX = cellSizeX / tickPerTurn;
			pixelPerTurnY = cellSizeY / tickPerTurn;
			shiftX = cellSizeX / 2 - shape.Width / 2;
			shiftY = cellSizeY / 2 - shape.Height / 2;

			UpdateValue();
			canvas.Children.Add(shape);
		}

		public override void UpdateValue() {
			text.Content = this.warriorsCnt.ToString() + '\n' + this.playerId.ToString();

			if(path[currPathIndex].Key > path[currPathIndex + 1].Key)
				Canvas.SetLeft(shape, path[currPathIndex].Key * cellSizeX - currTickOnCell * pixelPerTurnX + shiftX);
			else if (path[currPathIndex].Key < path[currPathIndex + 1].Key)
				Canvas.SetLeft(shape, path[currPathIndex].Key * cellSizeX + currTickOnCell * pixelPerTurnX + shiftX);
			else
				Canvas.SetLeft(shape, path[currPathIndex].Key * cellSizeX + shiftX);

			if (path[currPathIndex].Value > path[currPathIndex + 1].Value)
				Canvas.SetTop(shape, path[currPathIndex].Value * cellSizeY - currTickOnCell * pixelPerTurnY + shiftY);
			else if (path[currPathIndex].Value < path[currPathIndex + 1].Value)
				Canvas.SetTop(shape, path[currPathIndex].Value * cellSizeY + currTickOnCell * pixelPerTurnY + shiftY);
			else
				Canvas.SetTop(shape, path[currPathIndex].Value * cellSizeY + shiftY);
		}
	}
}
