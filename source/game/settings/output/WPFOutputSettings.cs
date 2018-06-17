using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;
using taw.game.basicInterfaces;
using taw.game.output;

namespace taw.game.settings.output {
	class WPFOutputSettings : BasicOutputSettings {
		public override void SetSettings(Settingable obj) {
			if (!(obj is WPFOutput output))
				throw new ApplicationException("Wrong generator in WPFOutputSettings.SetSettings");

			base.SetSettings(obj);

		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
