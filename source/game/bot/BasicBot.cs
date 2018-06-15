using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.bot {
	public abstract class BasicBot : TownsAndWarriors.game.basicInterfaces.Tickable, TownsAndWarriors.game.basicInterfaces.withPlayerId,
		TownsAndWarriors.game.basicInterfaces.Settingable {
		//---------------------------------------------- Fields ----------------------------------------------
		protected game.map.GameMap map;
		protected List<game.sity.BasicSity> sities;
		protected List<game.unit.BasicUnit> units;

		public byte ignoreFirstNTicks;
		public byte tickReact;


		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		public abstract bool TickReact();

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicBot() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		//---------------------------------------------- Settingable ----------------------------------------------

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new settings.bot.BasicBotSettings();
		}
	}
}
