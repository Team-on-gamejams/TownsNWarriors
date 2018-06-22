using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.controlable.botControl;

namespace taw.game.settings.controlable.botControl {
	class BasicBotSettings : settings.SettinsSetter {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is BasicBot bot))
				throw new ApplicationException("Wrong bot in BasicBotSettings.SetSettings");

			bot.ignoreFirstNTicks = 50;
			bot.tickReact = 50;
		}
	
		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
