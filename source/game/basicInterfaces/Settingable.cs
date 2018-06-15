using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.basicInterfaces {
	public interface Settingable {
		void GetSettings(game.settings.SettinsSetter settinsSetter);
		game.settings.SettinsSetter CreateLinkedSetting();
	}
}
