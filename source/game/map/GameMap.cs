using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors {
	public class GameMap {
		//---------------------------------------------- Fields ----------------------------------------------
		List<List<GameCell>> map;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public GameMap(int sizeX, int sizeY) {
			map = new List<List<GameCell>>(sizeY);
			for (int i = 0; i < sizeY; ++i) {
				map[i] = new List<GameCell>(sizeX);
				for (int j = 0; j < sizeX; ++j)
					map[i][j] = new GameCell();
			}
		}


		//---------------------------------------------- Methods ----------------------------------------------
		void GenerateRandomMap(int seed) {
			Random rnd = new Random(seed);
		}


	}
}
