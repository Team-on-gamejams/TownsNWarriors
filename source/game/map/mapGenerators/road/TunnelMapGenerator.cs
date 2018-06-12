using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game;
using TownsAndWarriors.game.map;
using TownsAndWarriors.game.settings;

using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.map.mapGenerators {
	public class TunnelMapGenerator : BasicMapGenerator {
		public GameMap GenerateRandomMap(int seed, int sizeX, int sizeY, BasicSityPlacer sityPlacer) {
			Random rnd = new Random(seed);
			GameMap m = new GameMap(sizeX, sizeY);

			LaburintCell[,] map = new LaburintCell[sizeY, sizeX];
			for (int i = 0; i < sizeY; ++i)
				for (int j = 0; j < sizeX; ++j)
					map[i, j] = new LaburintCell();

			int digNum = 0;
			List<KeyValuePair<int, int>> digPos;

			if (values.generator_TunenelMapGenerator_CrossOnStart)
				digPos = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(rnd.Next(1, sizeX - 1), rnd.Next(1, sizeY - 1)) };
			else
				digPos = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(rnd.Next(0, sizeX), rnd.Next(0, sizeY)) };

			while (digPos.Count != 0) {
				Dig(digPos[0].Key, digPos[0].Value);
				digPos.RemoveAt(0);
			}

			for (int i = 0; i < sizeY; ++i) { 
				for (int j = 0; j < sizeX; ++j) {
					m.Map[i][j].IsOpenLeft = map[i, j].IsOpenLeft;
					m.Map[i][j].IsOpenRight = map[i, j].IsOpenRight;
					m.Map[i][j].IsOpenTop = map[i, j].IsOpenTop;
					m.Map[i][j].IsOpenBottom = map[i, j].IsOpenBottom;
				}
			}

			sityPlacer.PlaceSities(m, rnd);

					//for (int i = 0; i < sizeX; ++i)
					//	m.Map[0][i].IsOpenLeft = m.Map[0][i].IsOpenRight = true;
					//for (int i = 0; i < sizeY; ++i)
					//	m.Map[i][sizeX - 1].IsOpenTop = m.Map[i][sizeX - 1].IsOpenBottom = true;

			//for (int i = 0; i < sizeY; ++i)
			//	m.Map[i][0].IsOpenTop = m.Map[i][0].IsOpenBottom = true;

			//m.Map[0][0].IsOpenLeft = m.Map[0][sizeX - 1].IsOpenRight = false;
			//m.Map[0][sizeX - 1].IsOpenTop = m.Map[sizeY - 1][sizeX - 1].IsOpenBottom = false;

			//m.Map[0][0].IsOpenTop = m.Map[sizeY - 1][0].IsOpenBottom = false;

			//BasicSity.gameMap = m;
			//m.Sities.Add(new BasicSity());
			//m.Sities.Add(new BasicSity());
			//m.Sities.Add(new BasicSity());

			//m.Sities[0].playerId = 1;
			//m.Sities[1].playerId = 2;

			//m.Map[0][0].Sity = m.Sities[0];
			//m.Map[sizeY - 1][sizeX - 1].Sity = m.Sities[1];
			//m.Map[sizeY - 1][0].Sity = m.Sities[2];

			return m;

			void Dig(int x, int y) {
				++digNum;

				if (rnd.Next(0, 100) < values.generator_TunenelMapGenerator_SkipChance && digNum > values.generator_TunenelMapGenerator_IgnoreSkipChanceForFirstNTitles)
					return;

				map[y, x].isVisited = true;
				List<KeyValuePair<int, int>> jumpPos = new List<KeyValuePair<int, int>>();
				if (x != sizeX - 1 && !map[y, x + 1].isVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x + 1, y));
				if (x != 0 && !map[y, x - 1].isVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x - 1, y));
				if (y != sizeY - 1 && !map[y + 1, x].isVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x, y + 1));
				if (y != 0 && !map[y - 1, x].isVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x, y - 1));

				byte jumpCnt = (byte)(jumpPos.Count != 0 ? rnd.Next(1, jumpPos.Count) : 0);

				if (values.generator_TunenelMapGenerator_CrossOnStart && digNum == 1) 
					jumpCnt = 4;

				while (jumpCnt-- != 0) {
					var curr = jumpPos[rnd.Next(0, jumpPos.Count)];
					jumpPos.Remove(curr);

					if (curr.Key == x + 1) 
						map[y, x].IsOpenRight = map[y, x + 1].IsOpenLeft = true;
					if (curr.Key == x - 1)
						map[y, x].IsOpenLeft = map[y, x - 1].IsOpenRight = true;
					if (curr.Value == y - 1)
						map[y, x].IsOpenTop = map[y - 1, x].IsOpenBottom = true;
					if (curr.Value == y + 1)
						map[y, x].IsOpenBottom = map[y + 1, x].IsOpenTop = true;

					digPos.Add(curr);
				}
			}


			bool CellsLeftUnvisited() {
				for (int i = 0; i < sizeY; ++i)
					for (int j = 0; j < sizeX; ++j)
						if(!map[i, j].isVisited)
							return true;
				return false;
			}
		}

		class LaburintCell {
			public bool IsOpenLeft { get; set; }
			public bool IsOpenTop { get; set; }
			public bool IsOpenRight { get; set; }
			public bool IsOpenBottom { get; set; }

			public bool isVisited { get; set; }
		}
	}
}
