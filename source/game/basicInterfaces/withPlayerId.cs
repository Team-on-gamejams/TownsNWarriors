using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.basicInterfaces {
	interface withPlayerId {
		/// <summary>
		/// 0  - нейтральний
		/// 1  - игрок
		/// 2+ - боти
		/// </summary>
		byte playerId { get; set; }
	}
}
