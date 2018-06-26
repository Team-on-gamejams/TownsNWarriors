using System;

using taw.game.controlable.botControl.parts;
using taw.game.controlable.botControl.support;

namespace taw.game.controlable.botControl.parts.attack {
	class CaptureNeutral : BasicPart {
		public override bool TickReact() {
			if (LogicalPlayersSingletone.ControlInfoForParts[0].Count != 0) {
				REPEAT_CITY_CHOOSE:

				int randPos = Rand.Next(0, LogicalPlayersSingletone.ControlInfoForParts[0].Count);
				int i = 0;
				city.BasicCity city = null;
				foreach (var cityNUnit in LogicalPlayersSingletone.ControlInfoForParts[0]) {
					if(i == randPos) {
						if (cityNUnit.Value.AllyUnitsMovingToCity.Count == 0 &&
							cityNUnit.Value.EnemyUnitsMovingToCity.Count == 0) {
							city = cityNUnit.Key;
							break;
						}
						else 
							goto REPEAT_CITY_CHOOSE;
					}
					++i;
				}

				TickReactResult = new PartCommand(null, city, 0, 0, false);
				return true;
			}
			return false;
		}
	}
}
