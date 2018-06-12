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
			//settings.values.generator_CityChooserIdDiffCorners_Players;
			//settings.values.generator_CityChooserIdDiffCorners_TownsPerPlayer;
			//settings.values.generator_CityChooserIdDiffCorners_TownsPerBot;
			int playerTowns = settings.values.generator_CityChooserIdDiffCorners_TownsPerPlayer;

			while (playerTowns-- != 0) {
				int minLen = 0;
				for (int i = 0; i < m.SizeY; ++i) {
					for (int j = 0; j < m.SizeX; ++j) {
						if (m.Map[i][j].Sity != null && m.Map[i][j].Sity.playerId == 0) {

						}
					}
				}
			}
		}
		//-------------------------------------------- Methods - parts --------------------------------------------



		//-------------------------------------- Methods - Support --------------------------------------------
	}
}
