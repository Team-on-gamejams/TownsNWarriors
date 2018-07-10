using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lp=taw.game.controlable.botControl.support.LogicalPlayersSingletone;
using taw.game.city;

namespace taw.game.controlable.botControl.parts.attack {
	class CaptureNeutral : BasicPart {
		public CaptureNeutral(ushort Priority) : base(Priority) {

		}

		public override bool TickReact() {
			if (lp.ControlInfoForParts[0].Count != 0) {
				command = new Command {
					fromType = Command.FromType.Direct,
					toType = Command.ToType.Direct,
				};
				BasicCity from = null;
				BasicCity to = null;

				bool findFirst = false;
				foreach (var fromCity in lp.ControlInfoForParts[this.PlayerId].Keys) {
					from = fromCity;
					foreach (var city in lp.ControlInfoForParts[0].Keys) {
						from.BuildOptimalPath(city, out BasicCity real);
						if (real == city && from.currWarriors * from.sendPersent * 3 * from.atkPersent > city.GetDefWarriors()) {
							to = city;
							findFirst = true;
							break;
						}
					}
					if (findFirst)
						break;
				}

				if(to != null) {
					command.to = to;
					command.from = from;
					command.warriorsType = Command.WarriorsType.Count;
					command.warriors = (ushort)(to.currWarriors);
					return true;
				}

			}
			return false;
		}
	}
}
