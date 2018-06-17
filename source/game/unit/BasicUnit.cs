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
	public partial class BasicUnit : GameCellDrawableObj, Tickable, WithPlayerId, Settingable {
		//---------------------------------------------- Fields ----------------------------------------------
		protected List<KeyValuePair<int, int>> path;
		protected int currPathIndex;
		protected ushort currTickOnCell;

		public ushort warriorsCnt;
		public BasicSity destination;

		//Load from settings
		public ushort tickPerTurn;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicUnit(ushort warriorsCnt, byte PlayerId, List<KeyValuePair<int, int>> Path, BasicSity destination) {
			this.warriorsCnt = warriorsCnt;
			playerId = PlayerId;
			path = Path;
			this.destination = destination;

			currTickOnCell = 0;
			currPathIndex = 0;

			if(path != null)
				BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

			GetSettings(CreateLinkedSetting());
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public bool TickReact() {
			++currTickOnCell;
			if(currTickOnCell >= tickPerTurn) {
				currTickOnCell = 0;
				BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
				++currPathIndex;
				BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

				if( (BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity != null &&
					BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity.playerId != this.playerId) ||
					(currPathIndex == path.Count - 1)
					) {
					BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
					BasicSity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity.GetUnits(this);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Кількість тіків, через яку юнит зайде в місто</returns>
		public ushort TicksLeftToDestination() {
			return (ushort)((path.Count - 1 - currPathIndex) * tickPerTurn - currTickOnCell);
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public virtual settings.SettinsSetter CreateLinkedSetting() {
			return new settings.unit.BasicUnitSettings();
		}
	}
}
