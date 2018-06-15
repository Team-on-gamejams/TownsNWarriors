using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.bot {
	public abstract class BasicBot : TownsAndWarriors.game.basicInterfaces.Tickable, TownsAndWarriors.game.basicInterfaces.withPlayerId {
		//---------------------------------------------- Fields ----------------------------------------------
		protected game.map.GameMap map;
		protected List<game.sity.BasicSity> sities;
		protected List<game.unit.BasicUnit> units;


		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		public abstract bool TickReact();
	}
}
