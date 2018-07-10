using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.city;
using lp = taw.game.controlable.botControl.support.LogicalPlayersSingletone;

namespace taw.game.controlable.botControl.parts.attack {
	class RushWeakestCity : BasicPart {
		public RushWeakestCity(ushort Priority) : base(Priority) {

		}

		public override bool TickReact() {
			command = new Command {
				fromType = Command.FromType.NearestCity,
				toType = Command.ToType.Direct,
				warriorsType = Command.WarriorsType.Count
			};

			BasicCity weakesCity = null;
			foreach (var control in lp.ControlInfoForParts) {
				foreach (var city in control) {
					if (city.Key.PlayerId == this.PlayerId)
						break;

					if (city.Value.AllyUnitsMovingToCity.Count == 0 && city.Value.EnemyUnitsMovingToCity.Count == 0 &&
						(weakesCity == null || weakesCity.GetDefWarriors() > city.Key.GetDefWarriors())
					) 
						weakesCity = city.Key;
					
				}
			}

			if(weakesCity != null) {
				command.to = weakesCity;
				command.warriors = (ushort)(weakesCity.GetDefWarriors() + 1);
				return true;
			}

			return false;
		}
	}
}
