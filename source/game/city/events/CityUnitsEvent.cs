using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.city.events {
	public class CityUnitsEvent : BasicCityEvent {
		public game.unit.BasicUnit unit;

		public CityUnitsEvent(BasicCityEvent basicCityEvent,
			unit.BasicUnit Unit) : base(basicCityEvent) {
			unit = Unit;
		}
	}
}
