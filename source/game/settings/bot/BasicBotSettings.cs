using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.bot;


namespace TownsAndWarriors.game.settings.bot {
	class BasicBotSettings : settings.SettinsSetter {
		public override void SetSettings(Settingable obj) {
			BasicBot bot = obj as BasicBot;
			if (bot == null)
				throw new ApplicationException("Wrong generator in BasicIdSetterSettings.SetSettings");

			bot.ignoreFirstNTicks = 50;
			bot.tickReact = 50;
	}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
