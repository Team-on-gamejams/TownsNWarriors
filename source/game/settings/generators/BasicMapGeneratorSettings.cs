using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.map.generators.map;

namespace taw.game.settings.generators {
	class BasicMapGeneratorSettings : SettinsSetter {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is BasicMapGenerator mapGenerator))
				throw new ApplicationException("Wrong generator in BasicMapGeneratorSettings.SetSettings");

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
