using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.controlable.botControl.parts {
	class PartCommandWithPriority {
		public short Priority { get; set; }
		public PartCommand Command { get; set; }

		public PartCommandWithPriority(short priority, PartCommand command) {
			Priority = priority;
			Command = command;
		}
	}
}
