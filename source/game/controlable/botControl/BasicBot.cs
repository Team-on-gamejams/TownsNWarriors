using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.controlable.botControl {
	public abstract class BasicBot : taw.game.controlable.Controlable {
		//---------------------------------------------- Fields ----------------------------------------------
		//Map
		protected game.map.GameMap map;

		public byte ignoreFirstNTicks;
		public byte tickReact;


		//---------------------------------------------- Properties ----------------------------------------------
		public byte PlayerId { get; set; }

		public abstract bool TickReact();

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicBot(game.map.GameMap Map,
			byte botId) {
			map = Map;
			PlayerId = botId;

			this.SetSettings(this.CreateLinkedSetting());
		}

		//---------------------------------------------- Settingable ----------------------------------------------

		public void SetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new settings.controlable.botControl.BasicBotSettings();
		}
	}
}
