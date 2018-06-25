using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.controlable.botControl.parts;

namespace taw.game.controlable.botControl.parts.attack {
	class CaptureNeutral : BasicPart {
		public override bool TickReact() {
			TickReactResult = new PartCommand(null, null, -1, 0, false);
			return false;
		}
	}
}
