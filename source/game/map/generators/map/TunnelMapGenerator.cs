using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game;
using taw.game.map;
using taw.game.settings;

using taw.game.city;
using taw.game.unit;

namespace taw.game.map.generators.map {
	class TunnelMapGenerator : BasicMapGenerator {
		//------------------------------------------ Fields ------------------------------------------
		public byte skipChance;
		public byte ignoreSkipChanceForFirstNTitles;
		public bool crossOnStart;

		protected LaburintCell[,] map;

		//------------------------------------------ Ctor ------------------------------------------
		public TunnelMapGenerator() {
			this.SetSettings(this.CreateLinkedSetting());
		}

		//------------------------------------------ Inharitated methods ------------------------------------------

		public override void GenerateRandomMap() {
			FormLogicMap();

			int digNum = 0;
			List<KeyValuePair<int, int>> digPos;

			if (crossOnStart)
				digPos = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(Rand.Next(1, gameMap.SizeX - 1), Rand.Next(1, gameMap.SizeY - 1)) };
			else
				digPos = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(Rand.Next(0, gameMap.SizeX), Rand.Next(0, gameMap.SizeY)) };

			while (digPos.Count != 0) {
				Dig(digPos[0].Key, digPos[0].Value);
				digPos.RemoveAt(0);
			}

			FillMapBack();

			void Dig(int x, int y) {
				++digNum;

				if (digNum > ignoreSkipChanceForFirstNTitles && Rand.NextPersent() < skipChance)
					return;

				map[y, x].IsVisited = true;
				List<KeyValuePair<int, int>> jumpPos = new List<KeyValuePair<int, int>>();
				if (x != gameMap.SizeX - 1 && !map[y, x + 1].IsVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x + 1, y));
				if (x != 0 && !map[y, x - 1].IsVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x - 1, y));
				if (y != gameMap.SizeY - 1 && !map[y + 1, x].IsVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x, y + 1));
				if (y != 0 && !map[y - 1, x].IsVisited)
					jumpPos.Add(new KeyValuePair<int, int>(x, y - 1));

				byte jumpCnt = (byte)(jumpPos.Count != 0 ? Rand.Next(1, jumpPos.Count) : 0);

				if (crossOnStart && digNum == 1) 
					jumpCnt = 4;

				while (jumpCnt-- != 0) {
					var curr = jumpPos[Rand.Next(0, jumpPos.Count)];
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
		}

		public override SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.TunnelMapGeneratorSettings();
		}

		//------------------------------------------ Support methods ------------------------------------------

		void FormLogicMap() {
			map = new LaburintCell[gameMap.SizeY, gameMap.SizeX];
			for (int i = 0; i < gameMap.SizeY; ++i)
				for (int j = 0; j < gameMap.SizeX; ++j)
					map[i, j] = new LaburintCell();
		}

		void FillMapBack() {
			for (int i = 0; i < gameMap.SizeY; ++i) {
				for (int j = 0; j < gameMap.SizeX; ++j) {
					gameMap.Map[i][j].IsOpenLeft = map[i, j].IsOpenLeft;
					gameMap.Map[i][j].IsOpenRight = map[i, j].IsOpenRight;
					gameMap.Map[i][j].IsOpenTop = map[i, j].IsOpenTop;
					gameMap.Map[i][j].IsOpenBottom = map[i, j].IsOpenBottom;
				}
			}

		}

		//------------------------------------------ Add classes ------------------------------------------
		protected class LaburintCell {
			public bool IsOpenLeft { get; set; }
			public bool IsOpenTop { get; set; }
			public bool IsOpenRight { get; set; }
			public bool IsOpenBottom { get; set; }

			public bool IsVisited { get; set; }
		}
	}
}
