using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.controlable {
	public interface Controlable : TownsAndWarriors.game.basicInterfaces.Tickable, TownsAndWarriors.game.basicInterfaces.WithPlayerId,
		TownsAndWarriors.game.basicInterfaces.Settingable {
	}
}
