using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownsAndWarriors.game.settings;

using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.settings.unit {
	public class BasicUnitSettings : UnitSettings {
		public override void SetSettings(game.basicInterfaces.Settingable obj) {
			BasicUnit unit = obj as BasicUnit;
			if (unit == null)
				throw new ApplicationException("Wrong unit in BasicUnitSettings.SetSettings");

			base.SetSettings(obj);

			unit.tickPerTurn = 10;
		}
	}
}
