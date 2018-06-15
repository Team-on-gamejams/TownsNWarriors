using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.map.generators.map;

namespace TownsAndWarriors.game.settings.generators {
	class BasicMapGeneratorSettings : SettinsSetter {
		public override void SetSettings(Settingable obj) {
			BasicMapGenerator mapGenerator = obj as BasicMapGenerator;
			if (mapGenerator == null)
				throw new ApplicationException("Wrong generator in BasicMapGeneratorSettings.SetSettings");

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
