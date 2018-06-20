using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.map.generators {
	public abstract class BasicGenerator {
		protected GameMap gameMap;

		public void SetGameMap(GameMap map) => this.gameMap = map;
	}
}
