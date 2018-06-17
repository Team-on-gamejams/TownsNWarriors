using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.controlable.playerControl {
 	abstract class BasicPlayer : Controlable {
		//---------------------------------------------- Fields ----------------------------------------------


		//---------------------------------------------- Properties ----------------------------------------------
		public byte PlayerId { get; set; }


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicPlayer(byte PlayerId) {
			this.PlayerId = PlayerId;
			GetSettings(CreateLinkedSetting());
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public abstract bool TickReact();

		//---------------------------------------------- Settinggable ----------------------------------------------

		public abstract SettinsSetter CreateLinkedSetting();

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
