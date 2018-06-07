using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game;
using TownsAndWarriors.game.settings;

using TownsAndWarriors.game.sity;


namespace TownsAndWarriors.game.map {
	public partial class GameMap {
		//---------------------------------------------- Fields ----------------------------------------------
		int sizeX, sizeY;
		List<List<GameCell>> map;
		List<BasicSity> sities;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		private GameMap(int SizeX, int SizeY) {
			sizeX = SizeX; sizeY = SizeY;
			map = new List<List<GameCell>>(sizeY);
			for (int i = 0; i < sizeY; ++i) {
				map.Add(new List<GameCell>(sizeX));
				for (int j = 0; j < sizeX; ++j)
					map[i].Add(new GameCell());
			}

			sities = new List<BasicSity>(values.locateMemorySizeForTowns);
		}


		//---------------------------------------------- Methods ----------------------------------------------
		static public GameMap GenerateRandomMap(int seed, int SizeX, int SizeY) {
			GameMap m = new GameMap(SizeX, SizeY);
			Random rnd = new Random(seed);

			for (int i = 0; i < m.sizeX; ++i)
				m.map[0][i].IsOpenLeft = m.map[0][i].IsOpenRight = true;
			for (int i = 0; i < m.sizeY; ++i)
				m.map[i][m.sizeX - 1].IsOpenTop = m.map[i][m.sizeX - 1].IsOpenBottom = true;

			m.map[0][0].IsOpenLeft = m.map[0][m.sizeX - 1].IsOpenRight = false;
			m.map[0][0].IsOpenTop = m.map[m.sizeY - 1][m.sizeX - 1].IsOpenBottom = false;

			m.sities.Add(new BasicSity());
			m.sities.Add(new BasicSity());

			m.map[0][0].Sity = m.sities[0];
			m.map[m.sizeY - 1][m.sizeX - 1].Sity = m.sities[1];


			return m;
		}
	}
}
