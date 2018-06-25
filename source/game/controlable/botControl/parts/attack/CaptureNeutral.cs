using System;

using taw.game.controlable.botControl.parts;
using taw.game.controlable.botControl.support;

namespace taw.game.controlable.botControl.parts.attack {
	class CaptureNeutral : BasicPart {
		public override bool TickReact() {
			//LogicalPlayersSingletone.ControlInfoForParts[PlayerId];

			TickReactResult = new PartCommand(null, null, -1, 0, false);
			return false;
		}
	}
}
