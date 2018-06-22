using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.city.events{
    public class BasicCityEvent : game.basicInterfaces.IGameEvent {
		public city.BasicCity city;

		public BasicCityEvent(BasicCity City) {
			city = City;
		}

		public BasicCityEvent(BasicCityEvent basicCityEvent) {
			city = basicCityEvent.city;
		}
	}
}
