using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.controlable.botControl.support {
	static class LogicalPlayersSingletone {
		public static List<Dictionary<city.BasicCity, UnitsMovingToCity>> ControlInfoForParts { get; private set; }

		public static void Init(game.map.GameMap map, byte controlsCnt) {
			ControlInfoForParts = new List<Dictionary<city.BasicCity, UnitsMovingToCity>>(controlsCnt + 1);
			for(int i = 0; i < controlsCnt + 1; ++i) 
				ControlInfoForParts.Add(new Dictionary<city.BasicCity, UnitsMovingToCity>());

			foreach (var city in map.Cities) {
				//if (city.PlayerId != 0)
					ControlInfoForParts[city.PlayerId].Add(city, new UnitsMovingToCity());


				city.Captured += (args) => {
					ControlInfoForParts[args.newPlayerId].Add(args.city, ControlInfoForParts[args.prevPlayerId][args.city]);
					ControlInfoForParts[args.prevPlayerId].Remove(args.city);

					foreach (var unit in ControlInfoForParts[args.newPlayerId][args.city].AllyUnitsMovingToCity) 
						ControlInfoForParts[args.newPlayerId][args.city].EnemyUnitsMovingToCity.Add(unit);
					ControlInfoForParts[args.newPlayerId][args.city].AllyUnitsMovingToCity.Clear();

					REPEAT_ALLY_FILL:
					foreach (var unit in ControlInfoForParts[args.newPlayerId][args.city].EnemyUnitsMovingToCity) {
						if(unit.PlayerId == args.city.PlayerId) {
							ControlInfoForParts[args.newPlayerId][args.city].AllyUnitsMovingToCity.Add(unit);
							ControlInfoForParts[args.newPlayerId][args.city].EnemyUnitsMovingToCity.Remove(unit);
							goto REPEAT_ALLY_FILL;
						}
					}
				};

				city.UnitSend += (args) => {
					if (args.unit.destination.PlayerId == args.unit.PlayerId)
						ControlInfoForParts[args.unit.destination.PlayerId][args.unit.destination]
						.AllyUnitsMovingToCity.Add(args.unit);
					else
						ControlInfoForParts[args.unit.destination.PlayerId][args.unit.destination]
						.EnemyUnitsMovingToCity.Add(args.unit);
				};

				city.UnitGet += (args) => {
					if (args.unit.destination.PlayerId == args.unit.PlayerId)
						ControlInfoForParts[args.unit.destination.PlayerId][args.unit.destination]
						.AllyUnitsMovingToCity.Remove(args.unit);
					else
						ControlInfoForParts[args.unit.destination.PlayerId][args.unit.destination]
						.EnemyUnitsMovingToCity.Remove(args.unit);
				};

				city.Captured += (args) => {
					foreach (var i in ControlInfoForParts[args.prevPlayerId]) 
						i.Key.ClearHashedPath();
					foreach (var i in ControlInfoForParts[args.newPlayerId]) 
						i.Key.ClearHashedPath();
				};
			}
		}

	}
}
