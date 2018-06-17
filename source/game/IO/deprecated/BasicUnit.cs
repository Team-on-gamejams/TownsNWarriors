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

using taw.game.basicInterfaces;

namespace taw.game.unit {
	public partial class BasicUnit {
		protected Label text;
		protected Shape rectangle;
		Canvas canvas;
		double pixelPerTurnX, pixelPerTurnY;
		double shiftX, shiftY;

		public void SetCanvas(Canvas c) => canvas = c;

		public override void InitializeShape() {
			shape = new Grid() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			settings.size.SizeChanged += () => {
				RecalcGeometrySize();
				rectangle.Width = shape.Width;
				rectangle.Height = shape.Height;
			};


			FillShape();

			canvas.Children.Add(shape);
		}

		public override void UpdateValue() {
			text.Content = this.warriorsCnt.ToString();

			if(path[currPathIndex].Key > path[currPathIndex + 1].Key)
				Canvas.SetLeft(shape, path[currPathIndex].Key * settings.size.OneCellSizeX - currTickOnCell * pixelPerTurnX + shiftX);
			else if (path[currPathIndex].Key < path[currPathIndex + 1].Key)
				Canvas.SetLeft(shape, path[currPathIndex].Key * settings.size.OneCellSizeX + currTickOnCell * pixelPerTurnX + shiftX);
			else
				Canvas.SetLeft(shape, path[currPathIndex].Key * settings.size.OneCellSizeX + shiftX);

			if (path[currPathIndex].Value > path[currPathIndex + 1].Value)
				Canvas.SetTop(shape, path[currPathIndex].Value * settings.size.OneCellSizeY - currTickOnCell * pixelPerTurnY + shiftY);
			else if (path[currPathIndex].Value < path[currPathIndex + 1].Value)
				Canvas.SetTop(shape, path[currPathIndex].Value * settings.size.OneCellSizeY + currTickOnCell * pixelPerTurnY + shiftY);
			else
				Canvas.SetTop(shape, path[currPathIndex].Value * settings.size.OneCellSizeY + shiftY);
		}

		protected virtual void FillShape() {
			RecalcGeometrySize();

			rectangle = new Rectangle() {
				Fill = settings.colors.neutralTownFill,
				Stroke = settings.colors.neutralTownStroke,
				Width = shape.Width,
				Height = shape.Height,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			sity.BasicSity.SetElipseColor(rectangle, this.playerId);
			shape.Children.Add(rectangle);

			text = new Label() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			shape.Children.Add(text);

			UpdateValue();
		}

		protected void RecalcGeometrySize() {
			shape.Width = settings.size.OneCellSizeX * settings.size.unitSizeMult;
			shape.Height = settings.size.OneCellSizeY * settings.size.unitSizeMult;

			pixelPerTurnX = settings.size.OneCellSizeX / tickPerTurn;
			pixelPerTurnY = settings.size.OneCellSizeY / tickPerTurn;
			shiftX = settings.size.OneCellSizeX / 2 - shape.Width / 2;
			shiftY = settings.size.OneCellSizeY / 2 - shape.Height / 2;
		}
	}
}
