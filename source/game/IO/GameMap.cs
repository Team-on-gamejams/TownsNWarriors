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
	public partial class GameMap {
		Canvas canvas;

		public void SetCanvas(Canvas Canvas) => canvas = Canvas;

		public void DrawStatic(Grid grid) {
			for (int i = 0; i < sizeY; ++i)
				for (int j = 0; j < sizeX; ++j) {
					map[i][j].SetGrid(grid);
					map[i][j].DrawOnGameCell(j, i);
					if(map[i][j].Sity != null) {
						map[i][j].Sity.SetGrid(grid);
						map[i][j].Sity.DrawOnGameCell(j, i);
					}
				}
		}

		public void UpdateMap() {
			foreach (var sity in sities) 
				sity.UpdateValue();
			foreach (var unit in units)
				unit.UpdateValue();
		}
	}
}
