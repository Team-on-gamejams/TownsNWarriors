using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.controlable.botControl.parts {
	public class PartWithPriority {
		public short Priority { get; set; }
		public BasicPart Part { get; set; }

		public PartWithPriority(short priority, BasicPart part) {
			Priority = priority;
			Part = part;
		}
	}
}
