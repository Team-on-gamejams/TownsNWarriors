using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.controlable.playerControl;

namespace TownsAndWarriors.game.settings.controlable.playerControl {
	class LocalPlayer1Settings : BasicPlayerSettigns {
		public override void SetSettings(Settingable obj) {
			LocalPlayer1 player = obj as LocalPlayer1;
			if (player == null)
				throw new ApplicationException("Wrong generator in LocalPlayer1Settings.SetSettings");

		}
	}
}
