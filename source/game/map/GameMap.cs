using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game;
using TownsAndWarriors.game.settings;

using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.unit;
using TownsAndWarriors.game.bot;

namespace TownsAndWarriors.game.map {
	public partial class GameMap {
		//---------------------------------------------- Fields ----------------------------------------------
		int sizeX, sizeY;
		int cellSizeX, cellSizeY;

		List<List<GameCell>> map;
		List<BasicSity> sities;
		List<BasicUnit> units;

		List<BasicBot> bots;

		//---------------------------------------------- Properties ----------------------------------------------
		public List<BasicSity> Sities { get => sities; set => sities = value; }
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
			bots = new List<BasicBot>(settings.values.generator_CityId_Bots);
			for (int i = 0; i < settings.values.generator_CityId_Bots; ++i)
				bots.Add(null);

			BasicSity.gameMap = this;
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

			foreach (var bot in bots) 
				if(bot != null)
					bot.TickReact();
			//System.Windows.MessageBox.Show("map:" + game.globalGameInfo.tick.ToString());
		}

		public void SendWarriors(List<BasicSity> from, BasicSity to) {
            foreach (var i in from)
			{
				if (i.currWarriors > 1)
				SendWarriors(i, to);
			}
		}

    	public void SendWarriors(BasicSity from, BasicSity to) {
			if (from == to)
				return;

			var unit = from.SendUnit(to);

			if (unit== null)
				return;

			unit.SetCanvas(canvas);

			cellSizeX = (int)canvas.RenderSize.Width;
			cellSizeY = (int)canvas.RenderSize.Height;
			unit.InitializeShape();

			units.Add(unit);
		}

		public void SetBot(int id, BasicBot type) {
			bots[id] = type;
		}

		static public GameMap GenerateRandomMap(int SizeX, int SizeY, 
			game.map.mapGenerators.BasicMapGenerator mapGenerator, game.map.mapGenerators.BasicSityPlacer sityPlacer,
			game.map.mapGenerators.BasicCityId basicCityId
			) {
			return mapGenerator.GenerateRandomMap(SizeX, SizeY, sityPlacer, basicCityId);
		}
	}
}
