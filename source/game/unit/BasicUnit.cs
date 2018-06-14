using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.unit {
	public partial class BasicUnit : GameCellDrawableObj, tickable, withPlayerId, Settingable {
		//---------------------------------------------- Fields ----------------------------------------------
		protected List<KeyValuePair<int, int>> path;
		protected int currPathIndex;
		protected ushort currTickOnCell;

		public ushort warriorsCnt;
		public ushort tickPerTurn;
		public BasicSity destination;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicUnit(ushort warriorsCnt, byte PlayerId, List<KeyValuePair<int, int>> Path, BasicSity destination) {
			this.warriorsCnt = warriorsCnt;
			playerId = PlayerId;
			path = Path;
			this.destination = destination;

			currTickOnCell = 1;
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

				if( (BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity != null &&
					BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity.playerId != this.playerId) ||
					(currPathIndex == path.Count - 1)
					) {
					BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
					BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity.GetUnits(this);
					canvas.Children.Remove(shape);
					return true;
				}
			}
			return false;
		}

		public ushort TicksLeftToDestination() {
			return (ushort)((path.Count - 1 - currPathIndex) * tickPerTurn - currTickOnCell);
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
