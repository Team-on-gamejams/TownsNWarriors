using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.city.events {
	public class CityCaptureEvent : BasicCityEvent {
		public byte prevPlayerId;
		public byte newPlayerId;

		public CityCaptureEvent(BasicCityEvent basicCityEvent,
			byte PrevPlayerId,
			byte NewPlayerId) : base(basicCityEvent) {
			prevPlayerId = PrevPlayerId;
			newPlayerId = NewPlayerId;
		}
	}
}
