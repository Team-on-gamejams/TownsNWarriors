using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.map.generators.city;

namespace taw.game.settings.generators {
	class BasicSityPlaceSettings : SettinsSetter {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is BasicCityPlacer mapGenerator))
				throw new ApplicationException("Wrong generator in BasicSityPlaceSettings.SetSettings");

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
