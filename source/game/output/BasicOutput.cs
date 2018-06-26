using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.settings;
using taw.game.basicInterfaces;
using taw.game.output;


namespace taw.game.output {
	public abstract class BasicOutput  : basicInterfaces.ITickable,
		basicInterfaces.ISettingable  {
		//---------------------------------------------- Fields ----------------------------------------------
		protected readonly Game game;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicOutput(Game Game) {
			game = Game;

			SetSettings(CreateLinkedSetting());
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public abstract bool TickReact();

		//---------------------------------------------- Settinggable ----------------------------------------------
		public abstract SettinsSetter CreateLinkedSetting();

		public void SetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}
	}
}
