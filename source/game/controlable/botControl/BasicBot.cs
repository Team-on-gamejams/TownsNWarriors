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
		//All cities on map
		protected List<game.city.BasicCity> sities;
		//All existing units
		protected List<game.unit.BasicUnit> units;

		public byte ignoreFirstNTicks;
		public byte tickReact;


		//---------------------------------------------- Properties ----------------------------------------------
		public byte PlayerId { get; set; }

		public abstract bool TickReact();

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicBot(game.map.GameMap Map,
			List<game.city.BasicCity> Sities,
			List<game.unit.BasicUnit> Units,
			byte botId) {

			map = Map;
			sities = Sities;
			units = Units;
			PlayerId = botId;

			this.GetSettings(this.CreateLinkedSetting());
		}

		//---------------------------------------------- Settingable ----------------------------------------------

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new settings.controlable.botControl.BasicBotSettings();
		}
	}
}
