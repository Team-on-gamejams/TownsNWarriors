using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.map.mapGenerators {
	public class CityIdDiffCorners : BasicCityId {
		//---------------------------------------------- Fields ----------------------------------------------

		//---------------------------------------------- Methods - main ----------------------------------------------
		public void PlaceId(GameMap m, Random rnd) {

			int playerTowns = settings.values.generator_CityId_TownsPerPlayer;
			while (playerTowns-- != 0) {
				int mini = m.SizeY, minj = m.SizeX;
				for (int i = 0; i < m.SizeY; ++i) 
					for (int j = 0; j < m.SizeX; ++j) 
						if (m.Map[i][j].Sity != null && m.Map[i][j].Sity.playerId == 0 && mini + minj > i + j) {
							mini = i;
							minj = j;
						}
				m.Map[mini][minj].Sity.playerId = 1;
			}

			bool end = false;
			int BotsCnt = settings.values.generator_CityId_Bots;
			while (BotsCnt-- != 0) {
				int BotsTowns = settings.values.generator_CityId_TownsPerBot;
				while (BotsTowns-- != 0) {
					int maxi = 0, maxj = 0;
					for (int i = m.SizeY - 1; i >= 0; --i)
						for (int j = m.SizeX - 1; j >= 0; --j)
							if (m.Map[i][j].Sity != null && m.Map[i][j].Sity.playerId == 0 && maxi + maxj < i + j) {
								maxi = i;
								maxj = j;
							}
					if (maxi == 0 && maxj == 0 && m.Map[maxi][maxj].Sity == null) {
						end = true;
						break;
					}
					m.Map[maxi][maxj].Sity.playerId = (byte)(BotsCnt + 2);
				}
				if (end)
					break;
			}
		}
		//-------------------------------------------- Methods - parts --------------------------------------------



		//-------------------------------------- Methods - Support --------------------------------------------
	}
}
