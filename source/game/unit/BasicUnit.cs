using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.sity;


namespace TownsAndWarriors.game.unit {
	public partial class BasicUnit : GameCellDrawableObj, tickable, withPlayerId {
		//---------------------------------------------- Fields ----------------------------------------------
		public ushort warriorsCnt;

		protected List<KeyValuePair<int, int>> path;
		protected int currPathIndex;
		protected ushort currTickOnCell, tickPerTurn;

		public BasicSity destination;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicUnit(ushort warriorsCnt, byte PlayerId, List<KeyValuePair<int, int>> Path, BasicSity destination) {
			this.warriorsCnt = warriorsCnt;
			path = Path;
			currTickOnCell = 1;
			tickPerTurn = TownsAndWarriors.game.settings.values.basicUnit_ticks_MoveWarrior;

			playerId = PlayerId;
			this.destination = destination;

			currPathIndex = 0;
			BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public bool TickReact() {
			++currTickOnCell;
			if(currTickOnCell >= tickPerTurn) {
				currTickOnCell = 1;
				BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
				++currPathIndex;
				BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

				if (currPathIndex == path.Count - 1) {
					destination.GetUnits(this);
					canvas.Children.Remove(shape);
					return true;
				}
			}
			return false;
		}

	}
}
