using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.map.generators.idSetters;

namespace TownsAndWarriors.game.settings.generators {
	class BasicIdSetterSettings : SettinsSetter {
		public override void SetSettings(Settingable obj) {
			BasicIdSetter idSetter = obj as BasicIdSetter;
			if (idSetter == null)
				throw new ApplicationException("Wrong generator in BasicIdSetterSettings.SetSettings");

			idSetter.bots = 3;
			idSetter.townsPerBot = new List<byte>() {1, 2, 2};
			idSetter.townsPerPlayer = 1;

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
