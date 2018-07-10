using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.city;
using taw.game.settings;
using taw.game.unit.events;

using lp = taw.game.controlable.botControl.support.LogicalPlayersSingletone;

namespace taw.game.unit {
	public partial class BasicUnit : ITickable, IWithPlayerId, ISettingable, IOutputable {
		//---------------------------------------------- Fields ----------------------------------------------
		protected List<KeyValuePair<int, int>> path;
		protected int currPathIndex;
		public ushort currTickOnCell;

		public ushort warriorsCnt;
		public BasicCity destination;
		public BasicCity planedDestination;

		//Load from settings
		public ushort tickPerTurn;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte PlayerId { get; set; }
		public object OutputInfo { get; set; }

		public int Y { get => path[currPathIndex].Value; }
		public int X { get => path[currPathIndex].Key; }

		public int NextY { get => path[currPathIndex + 1].Value; }
		public int NextX { get => path[currPathIndex + 1].Key; }

		//---------------------------------------------- Events ----------------------------------------------
		public delegate void UnitBasicDelegate(BasicUnitEvent cityEvent);
		public delegate void UnitMoveDelegate(UnitMoveEvent cityEvent);
		public delegate void UnitReachDestinationDelegate(UnitReachDestinationEvent cityEvent);

		BasicUnitEvent basicUnitEvent;

		public event UnitReachDestinationDelegate ReachDestination;

		public event UnitMoveDelegate Move;

		public event UnitBasicDelegate Tick;
		public event UnitBasicDelegate FirstTick;


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicUnit(ushort warriorsCnt, byte PlayerId, List<KeyValuePair<int, int>> Path, BasicCity destination, BasicCity PlanedDestination) {
			this.warriorsCnt = warriorsCnt;
			this.PlayerId = PlayerId;

			SetPath(Path, destination, PlanedDestination);

			if (path != null)
				BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

			SetSettings(CreateLinkedSetting());

			basicUnitEvent = new BasicUnitEvent(this);
		}

		public void SetPath(List<KeyValuePair<int, int>> Path, BasicCity destination, BasicCity PlanedDestination) {
			this.destination = destination;
			planedDestination = PlanedDestination;
			path = Path;
			currTickOnCell = 0;
			currPathIndex = 0;
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public bool TickReact() {
			if (currTickOnCell == 0 && currPathIndex == 0 && FirstTick != null)
				FirstTick(basicUnitEvent);

			Tick?.Invoke(basicUnitEvent);

			++currTickOnCell;
			if(currTickOnCell >= tickPerTurn) {
				currTickOnCell = 0;
				BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
				++currPathIndex;
				BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Add(this);

				Move?.Invoke(new UnitMoveEvent(basicUnitEvent, path[currPathIndex].Key, path[currPathIndex].Value,
					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key]
					));

				if ( (BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].City != null &&
					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].City.PlayerId != this.PlayerId) ||
					(currPathIndex == path.Count - 1)
					) {

					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].Units.Remove(this);
					ReachDestination?.Invoke(new UnitReachDestinationEvent(basicUnitEvent, BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].City));
					BasicCity.gameMap.Map[path[currPathIndex].Value][path[currPathIndex].Key].City.GetUnits(this);

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

		public void SetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public virtual settings.SettinsSetter CreateLinkedSetting() {
			return new settings.unit.BasicUnitSettings();
		}

		public void DestroyUnit() {
			if (destination.PlayerId == this.PlayerId)
				lp.ControlInfoForParts[this.destination.PlayerId][this.destination]
				.AllyUnitsMovingToCity.Remove(this);
			else
				lp.ControlInfoForParts[this.destination.PlayerId][this.destination]
				.EnemyUnitsMovingToCity.Remove(this);

			BasicCity.gameMap.Units.Remove(this);
			BasicCity.gameMap.Map[Y][X].Units.Remove(this);
		}
	}
}
