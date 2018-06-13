using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.map.mapGenerators {
	public interface BasicSityPlacer {
		void PlaceSities(GameMap m, BasicCityId chooserId);
	}
}
