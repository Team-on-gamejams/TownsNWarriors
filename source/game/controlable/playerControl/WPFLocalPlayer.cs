using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.controlable.playerControl {
	class WPFLocalPlayer : BasicPlayer {
		//---------------------------------------------- Fields ----------------------------------------------


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public WPFLocalPlayer(byte PlayerId) : base(PlayerId) {
		}


		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			return false;
		}

		//---------------------------------------------- Settinggable ----------------------------------------------
		public override SettinsSetter CreateLinkedSetting() {
			return new settings.controlable.playerControl.WPFLocalPlayerSettings();
		}
	}
}
