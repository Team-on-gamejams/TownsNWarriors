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

				BasicCity from;

				BasicCity to;
				foreach (var city in lp.ControlInfoForParts[0].Keys) {
				}
			}
			return false;
		}
	}
}
