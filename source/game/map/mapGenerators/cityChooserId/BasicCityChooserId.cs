using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.map.mapGenerators {
	public interface BasicCityChooserId {
		void PlaceId(GameMap m, Random rnd);
	}
}
