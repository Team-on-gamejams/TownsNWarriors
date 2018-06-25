using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;

namespace taw.game.controlable.botControl.parts {
	public abstract class BasicPart : ITickable, IWithPlayerId {
		//------------------------------------------- Fields -------------------------------------------
		protected PartCommand TickReactResult;

		//------------------------------------------- Properties -------------------------------------------
		public byte PlayerId { get; set; }

		//------------------------------------------- Methods -------------------------------------------
		public abstract bool TickReact();
		public PartCommand GetTickReactResult() => TickReactResult;
	}
}
