using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.controlable.playerControl;

namespace TownsAndWarriors.game.settings.controlable.playerControl {
	class BasicPlayerSettigns : settings.SettinsSetter {
		public override void SetSettings(Settingable obj) {
			BasicPlayer player = obj as BasicPlayer;
			if (player == null)
				throw new ApplicationException("Wrong generator in BasicIdSetterSettings.SetSettings");

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
