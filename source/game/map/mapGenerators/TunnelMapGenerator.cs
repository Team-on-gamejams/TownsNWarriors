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
		public GameMap GenerateRandomMap(int seed, int sizeX, int sizeY) {
			GameMap m = new GameMap(sizeX, sizeY);
			Random rnd = new Random(seed);

			for (int i = 0; i < sizeX; ++i)
				m.Map[0][i].IsOpenLeft = m.Map[0][i].IsOpenRight = true;
			for (int i = 0; i < sizeY; ++i)
				m.Map[i][sizeX - 1].IsOpenTop = m.Map[i][sizeX - 1].IsOpenBottom = true;

			for (int i = 0; i < sizeY; ++i)
				m.Map[i][0].IsOpenTop = m.Map[i][0].IsOpenBottom = true;

			m.Map[0][0].IsOpenLeft = m.Map[0][sizeX - 1].IsOpenRight = false;
			m.Map[0][sizeX - 1].IsOpenTop = m.Map[sizeY - 1][sizeX - 1].IsOpenBottom = false;

			m.Map[0][0].IsOpenTop = m.Map[sizeY - 1][0].IsOpenBottom = false;

			BasicSity.gameMap = m;
			m.Sities.Add(new BasicSity());
			m.Sities.Add(new BasicSity());
			m.Sities.Add(new BasicSity());

			m.Sities[0].playerId = 1;
			m.Sities[1].playerId = 2;

			m.Map[0][0].Sity = m.Sities[0];
			m.Map[sizeY - 1][sizeX - 1].Sity = m.Sities[1];
			m.Map[sizeY - 1][0].Sity = m.Sities[2];

			return m;
		}
	}
}
