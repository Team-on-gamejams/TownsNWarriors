using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.basicInterfaces;

namespace taw.game.settings.game {
	class GameSettings : taw.game.settings.SettinsSetter {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is Game game))
				throw new ApplicationException("Wrong game in GameSettings.SetSettings");

			game.X = 10;
			game.Y = 10;
		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
