using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.settings;

using static TownsAndWarriors.game.settings.values;

namespace TownsAndWarriors.game.map.generators.city {
	class SityPlacer14 : BasicSityPlacer {
		//---------------------------------------------- Fields ----------------------------------------------
		List<BasicSity> sities = new List<BasicSity>();
		List<KeyValuePair<int, int>> bestSitiesPos = new List<KeyValuePair<int, int>>();

		public bool quadIsRoad;
		public ushort quadCells;
		public byte minQuads;
		public byte citiesPerQuadMin;
		public byte citiesPerQuadMax;

		public byte maxPlaceRepeats;

		public bool alwaysFillWith1Road = true;
		public byte chancePosWith1Road = 100;
		public byte chancePosWith2Road = 10;
		public byte chancePosWith3Road = 25;
		public byte chancePosWith4Road = 100;


		//---------------------------------------------- Ctor ----------------------------------------------
		public SityPlacer14() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		//---------------------------------------------- Methods - main ----------------------------------------------
		public override void PlaceSities() {
			FormSitiesList();

			int cnt = maxPlaceRepeats;
			do {
				if (cnt-- == 0)
					break;

				FormBestPosition();

				MixSitiesAndPos();

				SpecialInsertWith1Road();
				InsertIntoMap();
			} while (sities.Count != 0);

			gameMap.Sities = GetSitiesOnMap();
		}

		//-------------------------------------------- Methods - parts --------------------------------------------

		void FormSitiesList() {
			int gameQuads = 0, sitiesCnt;

			if (quadIsRoad) {
				for (int i = 0; i < gameMap.SizeY; ++i)
					for (int j = 0; j < gameMap.SizeX; ++j)
						if (gameMap.Map[i][j].IsOpenBottom || gameMap.Map[i][j].IsOpenTop || gameMap.Map[i][j].IsOpenLeft || gameMap.Map[i][j].IsOpenRight)
							++gameQuads;
				gameQuads /= quadCells;
			}
			else
				gameQuads = gameMap.SizeX * gameMap.SizeY / quadCells;
			if (gameQuads < minQuads)
				gameQuads = minQuads;

			sitiesCnt = rnd.Next(citiesPerQuadMin * gameQuads,
				citiesPerQuadMax * gameQuads);

			for (int i = 0; i < sitiesCnt; ++i)
				sities.Add(new BasicSity());
		}

		void FormBestPosition() {
			for (int i = 0; i < gameMap.SizeY; ++i) {
				for (int j = 0; j < gameMap.SizeX; ++j) {
					int s = (gameMap.Map[i][j].IsOpenBottom ? 1 : 0) +
					(gameMap.Map[i][j].IsOpenTop ? 1 : 0) +
					(gameMap.Map[i][j].IsOpenLeft ? 1 : 0) +
					(gameMap.Map[i][j].IsOpenRight ? 1 : 0);
					if (s == 1 && (rnd.Next(0, 100) < chancePosWith1Road || alwaysFillWith1Road))
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 2 && rnd.Next(0, 100) < chancePosWith2Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 3 && rnd.Next(0, 100) < chancePosWith3Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 4 && rnd.Next(0, 100) < chancePosWith4Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
				}
			}

		}

		void MixSitiesAndPos() {
			int times = rnd.Next(bestSitiesPos.Count + 1, (bestSitiesPos.Count + 1) * 3);

			for (int i = 0; i < times; ++i) {
				int pos1, pos2;
				object tmp;

				if (bestSitiesPos.Count > 2) {
					pos1 = rnd.Next(0, bestSitiesPos.Count);
					do
						pos2 = rnd.Next(0, bestSitiesPos.Count);
					while (pos2 == pos1);

					tmp = bestSitiesPos[pos1];
					bestSitiesPos[pos1] = bestSitiesPos[pos2];
					bestSitiesPos[pos2] = (KeyValuePair<int, int>)tmp;
				}

				if (sities.Count > 2) {
					pos1 = rnd.Next(0, sities.Count);
					do
						pos2 = rnd.Next(0, sities.Count);
					while (pos2 == pos1);
					tmp = sities[pos1];
					sities[pos1] = sities[pos2];
					sities[pos2] = (BasicSity)tmp;
				}
			}

		}

		void SpecialInsertWith1Road() {
			if (alwaysFillWith1Road) {
				for (int k = 0; k < bestSitiesPos.Count && sities.Count != 0; ++k) {
					if (IsFreeAround(k)) {
						int i = bestSitiesPos[k].Key, j = bestSitiesPos[k].Value;
						int s = (gameMap.Map[i][j].IsOpenBottom ? 1 : 0) + (gameMap.Map[i][j].IsOpenTop ? 1 : 0) +
								(gameMap.Map[i][j].IsOpenLeft ? 1 : 0) + (gameMap.Map[i][j].IsOpenRight ? 1 : 0);
						if (s == 1) {
							gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].Sity = sities[0];
							bestSitiesPos.RemoveAt(k);
							sities.RemoveAt(0);
							--k;
						}
					}
				}
			}
		}

		void InsertIntoMap() {
			while (sities.Count != 0 && bestSitiesPos.Count != 0) {
				if (IsFreeAround(0)) {
					gameMap.Map[bestSitiesPos[0].Key][bestSitiesPos[0].Value].Sity = sities[0];
					bestSitiesPos.RemoveAt(0);
					sities.RemoveAt(0);
				}
				else
					bestSitiesPos.RemoveAt(0);
			}
		}

		//-------------------------------------- Methods - Support --------------------------------------------

		bool IsFreeAround(int k) {
			return (bestSitiesPos[k].Key == 0 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenTop ||
					(bestSitiesPos[k].Key > 0 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenTop &&
												 gameMap.Map[bestSitiesPos[k].Key - 1][bestSitiesPos[k].Value].Sity == null)) &&

					(bestSitiesPos[k].Key == gameMap.Map.Count - 1 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenBottom ||
					(bestSitiesPos[k].Key < gameMap.Map.Count - 1 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenBottom &&
												 gameMap.Map[bestSitiesPos[k].Key + 1][bestSitiesPos[k].Value].Sity == null)) &&

					(bestSitiesPos[k].Value == 0 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenLeft ||
					(bestSitiesPos[k].Value > 0 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenLeft &&
												 gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value - 1].Sity == null)) &&

					(bestSitiesPos[k].Value == gameMap.Map[0].Count - 1 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenRight ||
					(bestSitiesPos[k].Value < gameMap.Map[0].Count - 1 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenRight &&
												 gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value + 1].Sity == null));
		}

		int CntSities() {
			int rez = 0;
			for (int i = 0; i < gameMap.SizeY; ++i)
				for (int j = 0; j < gameMap.SizeX; ++j)
					if (gameMap.Map[i][j].Sity != null)
						++rez;
			return rez;
		}

		List<sity.BasicSity> GetSitiesOnMap() {
			List<sity.BasicSity> list = new List<BasicSity>();
			for (int i = 0; i < gameMap.SizeY; ++i)
				for (int j = 0; j < gameMap.SizeX; ++j)
					if (gameMap.Map[i][j].Sity != null)
						list.Add(gameMap.Map[i][j].Sity);
			return list;
		}

		public override SettinsSetter CreateLinkedSetting() {
			return new TownsAndWarriors.game.settings.generators.SityPlacer14Settings();
		}
	}
}
