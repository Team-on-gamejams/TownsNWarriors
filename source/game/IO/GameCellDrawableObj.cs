using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public void SetGrid(System.Windows.Controls.Grid a) => grid = a;
		//public void SetPoint(int X, int Y){x = X; y = Y; }

		//---------------------------------------------- Methods ----------------------------------------------
		public abstract void DrawOnGameCell(int x, int y);
	//	public void DrawOnGameCell() {
		//}
	}
}
