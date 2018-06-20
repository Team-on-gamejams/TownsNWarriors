using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.controlable {
	public interface Controlable : taw.game.basicInterfaces.ITickable, taw.game.basicInterfaces.IWithPlayerId,
		taw.game.basicInterfaces.ISettingable {
	}
}
