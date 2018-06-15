using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.map.generators.map;

namespace TownsAndWarriors.game.settings.generators {
	class TunnelMapGeneratorSettings : BasicMapGeneratorSettings {
		public override void SetSettings(Settingable obj) {
			TunnelMapGenerator mapGenerator = obj as TunnelMapGenerator;
			if (mapGenerator == null)
				throw new ApplicationException("Wrong generator in TunnelMapGeneratorSettings.SetSettings");

			base.SetSettings(obj);

			mapGenerator.crossOnStart = true;
			mapGenerator.skipChance = 10;
			mapGenerator.ignoreSkipChanceForFirstNTitles = 15;
		}
	}
}
