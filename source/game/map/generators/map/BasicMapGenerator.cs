using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.map.generators.map {
	public abstract class BasicMapGenerator : BasicGenerator, basicInterfaces.ISettingable {
		public abstract void GenerateRandomMap();

		public BasicMapGenerator() {
			this.GetSettings(this.CreateLinkedSetting());
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.BasicMapGeneratorSettings();
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
