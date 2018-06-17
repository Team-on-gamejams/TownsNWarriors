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

namespace taw.game.map {
	partial class GameCell {
		public override void InitializeShape() {
			shape = new Grid();
			FillShape();

			//Delegates
			settings.size.SizeChanged += () => {
				shape.Children.Clear();
				FillShape();
			};
		}

		void FillShape() {
			shape.Background = settings.colors.roadBackground;
			shape.Children.Add(new Rectangle() {
				Fill = settings.colors.roadBackground,
				Stroke = settings.colors.roadStroke,
				StrokeThickness = settings.colors.roadStrokeThickness
			});

			Rectangle rect;
			if (IsOpenTop) {
				rect = FormRect();
				rect.Width = settings.size.roadWidth * settings.size.OneCellSizeX;
				rect.VerticalAlignment = VerticalAlignment.Top;
				shape.Children.Add(rect);
			}
			if (IsOpenBottom) {
				rect = FormRect();
				rect.Width = settings.size.roadWidth * settings.size.OneCellSizeX;
				rect.VerticalAlignment = VerticalAlignment.Bottom;
				shape.Children.Add(rect);
			}
			if (IsOpenLeft) {
				rect = FormRect();
				rect.Height = settings.size.roadHeight * settings.size.OneCellSizeY;
				rect.HorizontalAlignment = HorizontalAlignment.Left;
				shape.Children.Add(rect);
			}
			if (IsOpenRight) {
				rect = FormRect();
				rect.Height = settings.size.roadHeight * settings.size.OneCellSizeY;
				rect.HorizontalAlignment = HorizontalAlignment.Right;
				shape.Children.Add(rect);
			}
			if((IsOpenTop || IsOpenBottom) && (IsOpenLeft || IsOpenRight)) {
				rect = FormRect();
				rect.HorizontalAlignment = HorizontalAlignment.Center;
				rect.VerticalAlignment = VerticalAlignment.Center;
				rect.Height = settings.size.roadHeight * settings.size.OneCellSizeY;
				rect.Width = settings.size.roadWidth * settings.size.OneCellSizeX;
				shape.Children.Add(rect);
			}
		}

		Rectangle FormRect() {
			return new Rectangle() {
				Fill = Brushes.LightGray,
				Height = settings.size.OneCellSizeY / 2,
				Width = settings.size.OneCellSizeX / 2
			};
		}
	}
}
