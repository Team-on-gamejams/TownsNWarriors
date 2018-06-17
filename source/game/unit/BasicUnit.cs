using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.city;
using taw.game.settings;

namespace taw.game.unit {
	public partial class BasicUnit : ITickable, IWithPlayerId, ISettingable {
		//---------------------------------------------- Fields ----------------------------------------------
		protected List<KeyValuePair<int, int>> path;
		protected int currPathIndex;
		protected ushort currTickOnCell;

		public ushort warriorsCnt;
		public BasicCity destination;

		//Load from settings
		public ushort tickPerTurn;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte PlayerId { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicUnit(ushort warriorsCnt, byte PlayerId, List<KeyValuePair<int, int>> Path, BasicCity destination) {
			this.warriorsCnt = warriorsCnt;
			this.PlayerId = PlayerId;
			path = Path;
			this.destination = destination;

			currTickOnCell = 0;
			currPathIndex = 0;

			if(path != null)
				BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

			GetSettings(CreateLinkedSetting());
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public bool TickReact() {
			++currTickOnCell;
			if(currTickOnCell >= tickPerTurn) {
				currTickOnCell = 0;
				BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
				++currPathIndex;
				BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

				if( (BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity != null &&
					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity.PlayerId != this.PlayerId) ||
					(currPathIndex == path.Count - 1)
					) {
					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Sity.GetUnits(this);
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
