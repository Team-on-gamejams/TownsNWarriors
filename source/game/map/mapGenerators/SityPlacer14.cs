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
			int gameQuads = 0, sitiesCnt;

			if (values.generator_SityPlacer14_QuadIsRoad) {
				for (int i = 0; i < m.SizeY; ++i) 
					for (int j = 0; j < m.SizeX; ++j) 
						if (m.Map[i][j].IsOpenBottom || m.Map[i][j].IsOpenTop || m.Map[i][j].IsOpenLeft || m.Map[i][j].IsOpenRight) 
							++gameQuads;
				gameQuads /= values.generator_SityPlacer14_Quad_Size;
			}
			else 
				gameQuads = m.SizeX * m.SizeY / values.generator_SityPlacer14_Quad_Size;
			

			if (gameQuads < values.generator_SityPlacer14_Quad_MinimimCnt) gameQuads = values.generator_SityPlacer14_Quad_MinimimCnt;
			sitiesCnt = rnd.Next(values.generator_SityPlacer14_Quad_Sities_Min * gameQuads,
				values.generator_SityPlacer14_Quad_Sities_Max * gameQuads);

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
					if		(s == 1 && rnd.Next(0, 100) < values.generator_SityPlacer14_Chance_PosWith1Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 2 && rnd.Next(0, 100) < values.generator_SityPlacer14_Chance_PosWith2Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 3 && rnd.Next(0, 100) < values.generator_SityPlacer14_Chance_PosWith3Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 4 && rnd.Next(0, 100) < values.generator_SityPlacer14_Chance_PosWith4Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
				}
			}

			//Перемішування масиву міст і оптимальних позицій
			for (int i = 0; i < bestSitiesPos.Count * (rnd.NextDouble() + 1); ++i) {
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


			if (values.generator_SityPlacer14_FillAllWith1Road) {
				for (int k = 0; k < bestSitiesPos.Count && sities.Count != 0; ++k) {
					if (IsFreeAround(k)) {
						int i = bestSitiesPos[k].Key, j = bestSitiesPos[k].Value;
						int s = (m.Map[i][j].IsOpenBottom ? 1 : 0) + (m.Map[i][j].IsOpenTop ? 1 : 0) +
								(m.Map[i][j].IsOpenLeft ? 1 : 0) + (m.Map[i][j].IsOpenRight ? 1 : 0);
						if(s == 1) {
							m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].Sity = sities[0];
							bestSitiesPos.RemoveAt(k);
							sities.RemoveAt(0);
							--k;
						}
					}
				}
			}

			//Вставка в карту
			for (int i = 0; i < bestSitiesPos.Count && i < sities.Count; ++i) 
				if(IsFreeAround(i))
					m.Map[bestSitiesPos[i].Key][bestSitiesPos[i].Value].Sity = sities[i];
			

			bool IsFreeAround(int k) {
				return (bestSitiesPos[k].Key == 0 || !m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenTop ||
						(bestSitiesPos[k].Key > 0 && m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenTop &&
													 m.Map[bestSitiesPos[k].Key - 1][bestSitiesPos[k].Value].Sity == null)) &&

						(bestSitiesPos[k].Key == m.Map.Count - 1 || !m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenBottom ||
						(bestSitiesPos[k].Key < m.Map.Count - 1 && m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenBottom &&
													 m.Map[bestSitiesPos[k].Key + 1][bestSitiesPos[k].Value].Sity == null)) &&

						(bestSitiesPos[k].Value == 0 || !m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenLeft ||
						(bestSitiesPos[k].Value > 0 && m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value - 1].IsOpenLeft &&
													 m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value - 1].Sity == null)) &&

						(bestSitiesPos[k].Value == m.Map[0].Count - 1 || !m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenRight ||
						(bestSitiesPos[k].Value < m.Map[0].Count - 1 && m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenRight &&
													 m.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value + 1].Sity == null));
			}
		}

	}
}
