using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.unit.events {
	public class UnitMoveEvent : BasicUnitEvent {
		public int x, y;
		public map.GameCell gameCell;

		public UnitMoveEvent(BasicUnitEvent basicUnitEvent, int X, int Y, map.GameCell GameCell) : base(basicUnitEvent) {
			this.x = X;
			this.y = Y;
			gameCell = GameCell;
		}
	}
}
