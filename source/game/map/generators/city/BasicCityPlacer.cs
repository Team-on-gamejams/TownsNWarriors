using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.map.generators.city {
	public abstract class BasicCityPlacer : BasicGenerator, basicInterfaces.ISettingable {
		public abstract void PlaceSities();

		public BasicCityPlacer() {
			this.SetSettings(this.CreateLinkedSetting());
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.BasicSityPlaceSettings();
		}

		public void SetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
