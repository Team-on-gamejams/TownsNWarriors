using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.basicInterfaces {
	public interface IWithPlayerId {
		/// <summary>
		/// 0  - нейтральний
		/// 1  - игрок
		/// 2+ - боти
		/// </summary>
		byte PlayerId { get; set; }
	}
}
