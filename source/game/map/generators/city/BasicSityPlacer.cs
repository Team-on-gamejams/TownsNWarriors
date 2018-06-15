using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.map.generators.city {
	abstract class BasicSityPlacer : BasicGenerator, basicInterfaces.Settingable {
		public abstract void PlaceSities();

		public BasicSityPlacer() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new TownsAndWarriors.game.settings.generators.BasicSityPlaceSettings();
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
