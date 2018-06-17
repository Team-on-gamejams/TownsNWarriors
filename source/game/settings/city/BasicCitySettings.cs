using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;
using taw.game.sity;

namespace taw.game.settings.city {
	public class BasicCitySettings : CitySettings {
		public override void SetSettings(taw.game.basicInterfaces.Settingable obj) {
			BasicSity city = obj as BasicSity;
			if (city == null)
				throw new ApplicationException("Wrong city in BasicCitySettings.SetSettings");

			base.SetSettings(obj);

			city.maxWarriors = 20;
			city.currWarriors = 10;
			city.sendPersent = 0.5f;
			city.atkPersent = 1.0f;
			city.defPersent = 1.0f;
			city.ticksPerIncome = 50;

			city.saveOvercapedUnits = true;
			city.removeOvercapedUnits = true;

			city.equalsMeanCapturedForNeutral = true;
			city.equalsMeanCaptured = false;
		}
	}
}
