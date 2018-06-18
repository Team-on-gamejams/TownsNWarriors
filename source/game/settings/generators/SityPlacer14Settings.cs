using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.basicInterfaces;
using taw.game.map.generators.city;

namespace taw.game.settings.generators {
	class SityPlacer14Settings : BasicSityPlaceSettings {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is CityPlacer14 sityPlacer))
				throw new ApplicationException("Wrong generator in TunnelMapGeneratorSettings.SetSettings");

			base.SetSettings(obj);

			sityPlacer.maxPlaceRepeats = 3;

			sityPlacer.quadIsRoad = false;
			sityPlacer.quadCells = 25;
			sityPlacer.minQuads = 2;
			sityPlacer.citiesPerQuadMin = 2;
			sityPlacer.citiesPerQuadMax = 4;

		}
	}
}
