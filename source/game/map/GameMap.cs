using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game;
using taw.game.settings;

using taw.game.city;
using taw.game.unit;
using taw.game.controlable.botControl;

namespace taw.game.map {
	public partial class GameMap {
		//---------------------------------------------- Fields ----------------------------------------------
		int sizeX, sizeY;

		List<List<GameCell>> map;
		List<BasicCity> cities;
		List<BasicUnit> units;

		//---------------------------------------------- Properties ----------------------------------------------
		public List<BasicCity> Cities { get => cities; set => cities = value; }
		public List<BasicUnit> Units => units;
		public List<List<GameCell>> Map => map;
		public int SizeX => sizeX;
		public int SizeY => sizeY;

		//---------------------------------------------- Ctor ----------------------------------------------
		public GameMap(int SizeX, int SizeY) {
			sizeX = SizeX; sizeY = SizeY;
			map = new List<List<GameCell>>(sizeY);
			for (int i = 0; i < sizeY; ++i) {
				map.Add(new List<GameCell>(sizeX));
				for (int j = 0; j < sizeX; ++j) 
					map[i].Add(new GameCell());
			}

			cities = new List<BasicCity>();
			units = new List<BasicUnit>();

			BasicCity.gameMap = this;
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public void Tick() {
			foreach (var sity in cities)
				sity.TickReact();

			REPEAT_UNITS_TURN:
			foreach (var unit in units) {
				if (unit.TickReact())
					goto REPEAT_UNITS_TURN;
			}
		}
	}
}
