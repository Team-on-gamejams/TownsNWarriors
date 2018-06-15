using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.map.generators.city;

namespace TownsAndWarriors.game.settings.generators {
	class BasicSityPlaceSettings : SettinsSetter {
		public override void SetSettings(Settingable obj) {
			BasicSityPlacer mapGenerator = obj as BasicSityPlacer;
			if (mapGenerator == null)
				throw new ApplicationException("Wrong generator in BasicSityPlaceSettings.SetSettings");

	}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
