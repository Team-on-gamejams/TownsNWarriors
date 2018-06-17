using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.map.generators.city {
	public abstract class BasicSityPlacer : BasicGenerator, basicInterfaces.ISettingable {
		public abstract void PlaceSities();

		public BasicSityPlacer() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.BasicSityPlaceSettings();
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
