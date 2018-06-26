using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.city.events {
	public class CityIncomeEvent : BasicCityEvent {
		public bool isIncreace;

		public CityIncomeEvent(BasicCityEvent basicCityEvent, bool IsIncreace) : base(basicCityEvent) {
			isIncreace = IsIncreace;
		}
	}
}
