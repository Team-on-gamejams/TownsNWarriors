using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.map.generators.idSetters {
	class IdSetterDiffCorners : BasicIdSetter {
		//---------------------------------------------- Fields ----------------------------------------------

		//---------------------------------------------- Ctor ----------------------------------------------
		public IdSetterDiffCorners() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		//------------------------------------------ Inharitated methods ------------------------------------------
		public override void SetId() {
			int playerTowns = townsPerPlayer;
			while (playerTowns-- != 0) {
				int mini = gameMap.SizeY, minj = gameMap.SizeX;
				for (int i = 0; i < gameMap.SizeY; ++i) 
					for (int j = 0; j < gameMap.SizeX; ++j) 
						if (gameMap.Map[i][j].Sity != null && gameMap.Map[i][j].Sity.playerId == 0 && mini + minj > i + j) {
							mini = i;
							minj = j;
						}
				gameMap.Map[mini][minj].Sity.playerId = 1;
			}

			bool end = false;
			for(int botNum = 0; botNum < bots; ++botNum) {
				int BotsTowns = townsPerBot[botNum];
				while (BotsTowns-- != 0) {
					int maxi = 0, maxj = 0;
					for (int i = gameMap.SizeY - 1; i >= 0; --i)
						for (int j = gameMap.SizeX - 1; j >= 0; --j)
							if (gameMap.Map[i][j].Sity != null && gameMap.Map[i][j].Sity.playerId == 0 && maxi + maxj < i + j) {
								maxi = i;
								maxj = j;
							}
					if (maxi == 0 && maxj == 0 && gameMap.Map[maxi][maxj].Sity == null) {
						end = true;
						break;
					}
					gameMap.Map[maxi][maxj].Sity.playerId = (byte)(botNum + 2);
				}
				if (end)
					break;
			}
		}

		public override SettinsSetter CreateLinkedSetting() {
			return new TownsAndWarriors.game.settings.generators.BasicIdSetterSettings();
		}

		//------------------------------------------ Support methods ------------------------------------------


	}
}
