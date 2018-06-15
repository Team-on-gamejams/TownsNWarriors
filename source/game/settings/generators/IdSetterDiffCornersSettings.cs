using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.map.generators.idSetters;

namespace TownsAndWarriors.game.settings.generators {
	class IdSetterDiffCornersSettings : BasicIdSetterSettings {
		public override void SetSettings(Settingable obj) {
			IdSetterDiffCorners idSetter = obj as IdSetterDiffCorners;
			if (idSetter == null)
				throw new ApplicationException("Wrong generator in TunnelMapGeneratorSettings.SetSettings");

			base.SetSettings(obj);
		}
	}
}
