using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

using taw.game.unit;

namespace taw.game.settings.unit {
	public class BasicUnitSettings : UnitSettings {
		public override void SetSettings(taw.game.basicInterfaces.ISettingable obj) {
			if (!(obj is BasicUnit unit))
				throw new ApplicationException("Wrong unit in BasicUnitSettings.SetSettings");

			base.SetSettings(obj);

			unit.tickPerTurn = 10;
		}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
