using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.map.mapGenerators {
	public interface BasicMapGenerator {
		GameMap GenerateRandomMap(int seed, int SizeX, int SizeY, BasicSityPlacer sityPlacer);
	}
}
