using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;
using taw.game.basicInterfaces;
using taw.game.output;


namespace taw.game.output {
	abstract class BasicOutput  : basicInterfaces.Tickable,
		basicInterfaces.Settingable  {
		//---------------------------------------------- Fields ----------------------------------------------


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicOutput() {
			GetSettings(CreateLinkedSetting());
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public abstract bool TickReact();


		//---------------------------------------------- Settinggable ----------------------------------------------
		public abstract SettinsSetter CreateLinkedSetting();

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

	}
}
