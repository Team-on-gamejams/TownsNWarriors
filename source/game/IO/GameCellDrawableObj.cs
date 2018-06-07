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
	public abstract class GameCellDrawableObj {
		//---------------------------------------------- Fields ----------------------------------------------
		protected System.Windows.Controls.Grid grid;
		protected System.Windows.Shapes.Shape shape;
		//protected int x, y;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public GameCellDrawableObj() {

		}

		public abstract void InitializeShape();

		public virtual void SetGrid(System.Windows.Controls.Grid a) => grid = a;
		//public void SetPoint(int X, int Y){x = X; y = Y; }

		//---------------------------------------------- Methods ----------------------------------------------
		public virtual void DrawOnGameCell(int x, int y) {
			if (shape == null)
				InitializeShape();
			Grid.SetRow(shape, y);
			Grid.SetColumn(shape, x);
			grid.Children.Remove(shape);
			grid.Children.Add(shape);
		}
		//public void DrawOnGameCell() {
		//}
	}
}
