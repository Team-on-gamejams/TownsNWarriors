using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.unit.events {
	public class UnitReachDestinationEvent : BasicUnitEvent {
		public city.BasicCity city;

		public UnitReachDestinationEvent(BasicUnitEvent basicUnitEvent, city.BasicCity city) : base(basicUnitEvent) {
			this.city = city;
		}
	}
}
