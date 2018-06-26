using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.city;
using taw.game.settings;

namespace taw.game.map.generators.city {
	class CityPlacer14 : BasicCityPlacer {
		//---------------------------------------------- Fields ----------------------------------------------
		List<BasicCity> sities = new List<BasicCity>();
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
		public CityPlacer14() {
			this.SetSettings(this.CreateLinkedSetting());
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

			gameMap.Cities = GetSitiesOnMap();
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

			sitiesCnt = Rand.Next(citiesPerQuadMin * gameQuads,
				citiesPerQuadMax * gameQuads);

			for (int i = 0; i < sitiesCnt; ++i)
				sities.Add(new BasicCity());
		}

		void FormBestPosition() {
			for (int i = 0; i < gameMap.SizeY; ++i) {
				for (int j = 0; j < gameMap.SizeX; ++j) {
					int s = (gameMap.Map[i][j].IsOpenBottom ? 1 : 0) +
					(gameMap.Map[i][j].IsOpenTop ? 1 : 0) +
					(gameMap.Map[i][j].IsOpenLeft ? 1 : 0) +
					(gameMap.Map[i][j].IsOpenRight ? 1 : 0);
					if (s == 1 && (Rand.NextPersent() < chancePosWith1Road || alwaysFillWith1Road))
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 2 && Rand.NextPersent() < chancePosWith2Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 3 && Rand.NextPersent() < chancePosWith3Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
					else if (s == 4 && Rand.NextPersent() < chancePosWith4Road)
						bestSitiesPos.Add(new KeyValuePair<int, int>(i, j));
				}
			}

		}

		void MixSitiesAndPos() {
			int times = Rand.Next(bestSitiesPos.Count + 1, (bestSitiesPos.Count + 1) * 3);

			for (int i = 0; i < times; ++i) {
				int pos1, pos2;
				object tmp;

				if (bestSitiesPos.Count > 2) {
					pos1 = Rand.Next(0, bestSitiesPos.Count);
					do
						pos2 = Rand.Next(0, bestSitiesPos.Count);
					while (pos2 == pos1);

					tmp = bestSitiesPos[pos1];
					bestSitiesPos[pos1] = bestSitiesPos[pos2];
					bestSitiesPos[pos2] = (KeyValuePair<int, int>)tmp;
				}

				if (sities.Count > 2) {
					pos1 = Rand.Next(0, sities.Count);
					do
						pos2 = Rand.Next(0, sities.Count);
					while (pos2 == pos1);
					tmp = sities[pos1];
					sities[pos1] = sities[pos2];
					sities[pos2] = (BasicCity)tmp;
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
							InsertCity(k, 0);
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
					InsertCity(0, 0);
					bestSitiesPos.RemoveAt(0);
					sities.RemoveAt(0);
				}
				else
					bestSitiesPos.RemoveAt(0);
			}
		}

		void InsertCity(int idPos, int idCity) {
			gameMap.Map[bestSitiesPos[idPos].Key][bestSitiesPos[idPos].Value].City = sities[idCity];
			sities[idCity].X = bestSitiesPos[idPos].Value;
			sities[idCity].Y = bestSitiesPos[idPos].Key;
		}

		//-------------------------------------- Methods - Support --------------------------------------------

		bool IsFreeAround(int k) {
			return (bestSitiesPos[k].Key == 0 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenTop ||
					(bestSitiesPos[k].Key > 0 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenTop &&
												 gameMap.Map[bestSitiesPos[k].Key - 1][bestSitiesPos[k].Value].City == null)) &&

					(bestSitiesPos[k].Key == gameMap.Map.Count - 1 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenBottom ||
					(bestSitiesPos[k].Key < gameMap.Map.Count - 1 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenBottom &&
												 gameMap.Map[bestSitiesPos[k].Key + 1][bestSitiesPos[k].Value].City == null)) &&

					(bestSitiesPos[k].Value == 0 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenLeft ||
					(bestSitiesPos[k].Value > 0 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenLeft &&
												 gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value - 1].City == null)) &&

					(bestSitiesPos[k].Value == gameMap.Map[0].Count - 1 || !gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenRight ||
					(bestSitiesPos[k].Value < gameMap.Map[0].Count - 1 && gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value].IsOpenRight &&
												 gameMap.Map[bestSitiesPos[k].Key][bestSitiesPos[k].Value + 1].City == null));
		}

		int CntSities() {
			int rez = 0;
			for (int i = 0; i < gameMap.SizeY; ++i)
				for (int j = 0; j < gameMap.SizeX; ++j)
					if (gameMap.Map[i][j].City != null)
						++rez;
			return rez;
		}

		List<game.city.BasicCity> GetSitiesOnMap() {
			List<BasicCity> list = new List<BasicCity>();
			for (int i = 0; i < gameMap.SizeY; ++i)
				for (int j = 0; j < gameMap.SizeX; ++j)
					if (gameMap.Map[i][j].City != null)
						list.Add(gameMap.Map[i][j].City);
			return list;
		}

		public override SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.SityPlacer14Settings();
		}
	}
}
