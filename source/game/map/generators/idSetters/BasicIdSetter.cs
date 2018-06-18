using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;

namespace taw.game.map.generators.idSetters {
	public abstract class BasicIdSetter : BasicGenerator, basicInterfaces.ISettingable {
		public byte bots;
		public List<byte> townsPerBot;
		public byte townsPerPlayer;

		public abstract void SetId();

		public BasicIdSetter() {
			this.SetSettings(this.CreateLinkedSetting());
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.BasicIdSetterSettings();
		}

		public void SetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
