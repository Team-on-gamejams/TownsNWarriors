﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;
using taw.game.basicInterfaces;
using taw.game.output;

namespace taw.game.settings.output {
	class WPFOutputSettings : BasicOutputSettings {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is WPFOutput output))
				throw new ApplicationException("Wrong generator in WPFOutputSettings.SetSettings");

			base.SetSettings(obj);

			output.cityIsSquere = true;
			output.citySizeMod = 0.50;
		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
