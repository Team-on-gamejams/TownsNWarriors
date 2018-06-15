using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.map.generators.map {
	abstract class BasicMapGenerator : BasicGenerator, basicInterfaces.Settingable {
		public abstract void GenerateRandomMap();

		public BasicMapGenerator() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new TownsAndWarriors.game.settings.generators.BasicMapGeneratorSettings();
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
