using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game;
using TownsAndWarriors.game.settings;

using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.map {
	public partial class GameMap {
		//---------------------------------------------- Fields ----------------------------------------------
		int sizeX, sizeY;
		int cellSizeX, cellSizeY;

		List<List<GameCell>> map;
		List<BasicSity> sities;
		List<BasicUnit> units;

		game.bot.BasicBot bot;

		//---------------------------------------------- Properties ----------------------------------------------
		public List<BasicSity> Sities => sities;
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
				for (int j = 0; j < sizeX; ++j) {
					map[i].Add(new GameCell());
				}
			}

			sities = new List<BasicSity>(values.locateMemory_SizeForTowns);
			units = new List<BasicUnit>(values.locateMemory_SizeForUnits);
			bot = new game.bot.SimpleBot(this, sities, units, 2);
		}


		//---------------------------------------------- Methods ----------------------------------------------
		public void Tick() {
			foreach (var sity in sities)
				sity.TickReact();

			REPEAT_UNITS_TURN:
			foreach (var unit in units) {
				if (unit.TickReact())
					goto REPEAT_UNITS_TURN;
			}

			//bot.TickReact();
		}

		public void SendWarriors(List<BasicSity> from, BasicSity to) {
            foreach (var i in from)
                SendWarriors(i, to);
        }

    	public void SendWarriors(BasicSity from, BasicSity to) {
			var unit = from.SendUnit(to);

			unit.SetCanvas(canvas);

			cellSizeX = (int)canvas.RenderSize.Width;
			cellSizeY = (int)canvas.RenderSize.Height;
			unit.SetOneCellSize(cellSizeX, cellSizeY);
			unit.InitializeShape();

			units.Add(unit);
		}

		static public GameMap GenerateRandomMap(int seed, int SizeX, int SizeY, 
			game.map.mapGenerators.BasicMapGenerator mapGenerator, game.map.mapGenerators.BasicSityPlacer sityPlacer,
			game.map.mapGenerators.BasicCityId basicCityId
			) {
			return mapGenerator.GenerateRandomMap(seed, SizeX, SizeY, sityPlacer, basicCityId);
		}
	}
}
