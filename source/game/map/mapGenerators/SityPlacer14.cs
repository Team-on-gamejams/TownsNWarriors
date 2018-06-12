using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.map.mapGenerators {
	public class SityPlacer14 : BasicSityPlacer {

		public void PlaceSities(GameMap m, Random rnd) {
			//Вичислення оптимальної кількості міст
			int gameQuads = m.SizeX * m.SizeY / 25;
			if (gameQuads < 2) gameQuads = 2;
			int sitiesCnt = rnd.Next(2 * gameQuads, 4 * gameQuads);
			List<BasicSity> sities = new List<BasicSity>(sitiesCnt);
			for (int i = 0; i < sitiesCnt; ++i)
				sities.Add(new BasicSity());

			//Знаходження оптимальних позицій
			List<KeyValuePair<int, int>> bestSitiesPos = new List<KeyValuePair<int, int>>();

			for (int i = 0; i < m.SizeY; ++i) {
				for (int j = 0; j < m.SizeX; ++j) {
					int s = (m.Map[i][j].IsOpenBottom ? 1 : 0) +
					(m.Map[i][j].IsOpenTop ? 1 : 0) +
					(m.Map[i][j].IsOpenLeft ? 1 : 0) +
					(m.Map[i][j].IsOpenRight ? 1 : 0);
					if (s == 1 || s == 4)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					if(s == 3 && rnd.Next(0, 100) < 15)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					if (s == 2 && rnd.Next(0, 100) < 5)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
				}
			}

			//Перемішування масиву міст і оптимальних позицій
			for (int i = 0; i < sitiesCnt * (rnd.NextDouble() + 1); ++i) {
				int pos1 = rnd.Next(0, bestSitiesPos.Count), pos2;
				do 
					pos2 = rnd.Next(0, bestSitiesPos.Count);
				while (pos2 == pos1);

				object tmp = bestSitiesPos[pos1];
				bestSitiesPos[pos1] = bestSitiesPos[pos2];
				bestSitiesPos[pos2] = (KeyValuePair<int, int>) tmp;


				pos1 = rnd.Next(0, sities.Count);
				do
					pos2 = rnd.Next(0, sities.Count);
				while (pos2 == pos1);
				tmp = sities[pos1];
				sities[pos1] = sities[pos2];
				sities[pos2] = (BasicSity)tmp;
			}

			//Вставка в карту
			for (int i = 0; i < bestSitiesPos.Count && i < sities.Count; ++i) {
				if( (bestSitiesPos[i].Key == 0 || !m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenTop ||
					(bestSitiesPos[i].Key > 0 && m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenTop &&
					                             m.Map[bestSitiesPos[i].Key - 1][bestSitiesPos[i].Value].Sity == null)) &&

					(bestSitiesPos[i].Key == m.Map.Count - 1 || !m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenBottom ||
					(bestSitiesPos[i].Key < m.Map.Count - 1 && m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenBottom &&
												 m.Map[bestSitiesPos[i].Key + 1][bestSitiesPos[i].Value].Sity == null)) &&

					(bestSitiesPos[i].Value == 0 || !m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenLeft ||
					(bestSitiesPos[i].Value > 0 && m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value - 1].IsOpenLeft &&
												 m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value - 1].Sity == null)) &&

					(bestSitiesPos[i].Value == m.Map[0].Count - 1 || !m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenRight ||
					(bestSitiesPos[i].Value < m.Map[0].Count - 1 && m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].IsOpenRight &&
												 m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value + 1].Sity == null) )
					)
				m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].Sity = sities[i];
			}

		}

	}
}
