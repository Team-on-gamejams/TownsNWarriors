using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.controlable.playerControl;

namespace taw.game.settings.controlable.playerControl {
	class BasicPlayerSettigns : settings.SettinsSetter {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is BasicPlayer player))
				throw new ApplicationException("Wrong BasicPlayer in BasicPlayerSettigns.SetSettings");

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
