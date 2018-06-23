using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.map.generators.idSetters;

namespace taw.game.settings.generators {
	class BasicIdSetterSettings : SettinsSetter {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is BasicIdSetter idSetter))
				throw new ApplicationException("Wrong generator in BasicIdSetterSettings.SetSettings");

			idSetter.townsPerControl = new List<byte>() {1, 1, 1, 1};
		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
