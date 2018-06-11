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
		public List<BasicUnit> Units => units;
		public List<List<GameCell>> Map => map;

		//---------------------------------------------- Ctor ----------------------------------------------
		private GameMap(int SizeX, int SizeY) {
			sizeX = SizeX; sizeY = SizeY;
			map = new List<List<GameCell>>(sizeY);
			for (int i = 0; i < sizeY; ++i) {
				map.Add(new List<GameCell>(sizeX));
				for (int j = 0; j < sizeX; ++j)
					map[i].Add(new GameCell());
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

			bot.TickReact();
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

		static public GameMap GenerateRandomMap(int seed, int SizeX, int SizeY) {
			GameMap m = new GameMap(SizeX, SizeY);
			Random rnd = new Random(seed);

			for (int i = 0; i < m.sizeX; ++i)
				m.map[0][i].IsOpenLeft = m.map[0][i].IsOpenRight = true;
			for (int i = 0; i < m.sizeY; ++i)
				m.map[i][m.sizeX - 1].IsOpenTop = m.map[i][m.sizeX - 1].IsOpenBottom = true;

			for (int i = 0; i < m.sizeY; ++i)
				m.map[i][0].IsOpenTop = m.map[i][0].IsOpenBottom = true;

			m.map[0][0].IsOpenLeft = m.map[0][m.sizeX - 1].IsOpenRight = false;
			m.map[0][m.sizeX - 1].IsOpenTop = m.map[m.sizeY - 1][m.sizeX - 1].IsOpenBottom = false;

			m.map[0][0].IsOpenTop = m.map[m.sizeY - 1][0].IsOpenBottom = false;

			BasicSity.gameMap = m;
			m.sities.Add(new BasicSity());
			m.sities.Add(new BasicSity());
			m.sities.Add(new BasicSity());

			m.sities[0].playerId = 1;
			m.sities[1].playerId = 2;

			m.map[0][0].Sity = m.sities[0];
			m.map[m.sizeY - 1][m.sizeX - 1].Sity = m.sities[1];
			m.map[m.sizeY - 1][0].Sity = m.sities[2];


			return m;
		}
	}
}
