using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.sity {
	public partial class BasicSity : GameCellDrawableObj, tickable, withPlayerId {
		//---------------------------------------------- Fields ----------------------------------------------
		public static TownsAndWarriors.game.map.GameMap gameMap;

		protected ushort currWarriors, maxWarriors, ticksPerIncome;
		Dictionary<BasicSity, List<KeyValuePair<int, int>>> pathToSities;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicSity() {
			maxWarriors = settings.values.basicSity_MaxWarriors;
			currWarriors = settings.values.basicSity_StartWarriors;
			ticksPerIncome = settings.values.basicSity_ticks_NewWarrior;
			pathToSities = new Dictionary<BasicSity, List<KeyValuePair<int, int>>>(1);
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public void TickReact() {
			if (playerId != 0 && maxWarriors > currWarriors && game.globalGameInfo.tick % ticksPerIncome == 0)
				++currWarriors;
		}

		public BasicUnit SendUnit(BasicSity to) {
			ushort sendWarriors = (ushort) (currWarriors * settings.values.basicSity_sendWarriorsPersent);
			currWarriors -= sendWarriors;
			if (currWarriors < 0)
				currWarriors = 0;

			if (!pathToSities.ContainsKey(to)) 
				BuildPath();

			BasicUnit unit = new BasicUnit(sendWarriors, this.playerId, pathToSities[to], to);
			return unit;

			void BuildPath() {
				pathToSities.Add(to, new List<KeyValuePair<int, int>>() {
					new KeyValuePair<int, int>(0, 0),
					new KeyValuePair<int, int>(0, 1),
					new KeyValuePair<int, int>(0, 2),
					new KeyValuePair<int, int>(0, 3)
				});
			}
		}

		public void GetUnits(BasicUnit unit) {
			if(playerId == unit.playerId) {
				this.currWarriors += unit.warriorsCnt;
				if (!settings.values.gameplay_SaveWarriorsOverCap && currWarriors > maxWarriors)
					currWarriors = maxWarriors;
			}
			else {
				if (currWarriors > unit.warriorsCnt)
					currWarriors -= unit.warriorsCnt;
				else if (!settings.values.gameplay_EqualsMeansCapture && currWarriors == unit.warriorsCnt) {
					currWarriors = 0;
				}
				else {
					currWarriors = (ushort)(unit.warriorsCnt - currWarriors);
					playerId = unit.playerId;
				}

			}

			gameMap.Units.Remove(unit);
		}

	}
}
