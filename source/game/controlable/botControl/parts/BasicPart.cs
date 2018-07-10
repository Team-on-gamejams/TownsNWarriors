using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.city;
using taw.game.map;
using taw.game.unit;
using taw.game.controlable.botControl.parts;
using taw.game.controlable.botControl.support;

namespace taw.game.controlable.botControl.parts {
	abstract class BasicPart : ITickable, IWithPlayerId {
		protected Command command;
		public ushort priority;

		public byte PlayerId { get; set; }

		public BasicPart(ushort Priority) {
			priority = Priority;
		}

		public abstract bool TickReact();
		public Command GetRezult() => command;
	}
}
