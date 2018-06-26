using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.unit.events {
	public class BasicUnitEvent : game.basicInterfaces.IGameEvent {
		public BasicUnit unit;

		public BasicUnitEvent(BasicUnit Unit) {
			unit = Unit;
		}

		public BasicUnitEvent(BasicUnitEvent @event) {
			unit = @event.unit;
		}
	}
}
