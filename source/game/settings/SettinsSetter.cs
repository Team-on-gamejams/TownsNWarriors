using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.settings {
	abstract class SettinsSetter {
		abstract public void SetSettings(object obj);
		abstract protected void LoadSettingsFromFile();
	}
}
