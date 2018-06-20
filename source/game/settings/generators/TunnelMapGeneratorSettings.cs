using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.map.generators.map;

namespace taw.game.settings.generators {
	class TunnelMapGeneratorSettings : BasicMapGeneratorSettings {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is TunnelMapGenerator mapGenerator))
				throw new ApplicationException("Wrong generator in TunnelMapGeneratorSettings.SetSettings");

			base.SetSettings(obj);

			mapGenerator.crossOnStart = true;
			mapGenerator.skipChance = 10;
			mapGenerator.ignoreSkipChanceForFirstNTitles = 15;
		}
	}
}
