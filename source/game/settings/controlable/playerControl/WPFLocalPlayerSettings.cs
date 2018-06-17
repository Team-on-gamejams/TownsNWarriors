using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.controlable.playerControl;

namespace taw.game.settings.controlable.playerControl {
	class WPFLocalPlayerSettings : BasicPlayerSettigns {
		public override void SetSettings(Settingable obj) {
			WPFLocalPlayer player = obj as WPFLocalPlayer;
			if (player == null)
				throw new ApplicationException("Wrong generator in LocalPlayer1Settings.SetSettings");

			base.SetSettings(obj);

		}
	}
}
