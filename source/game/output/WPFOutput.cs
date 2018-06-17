using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;


using taw.game.settings;

namespace taw.game.output {
	class WPFOutput : BasicOutput {
		//---------------------------------------------- Fields ----------------------------------------------


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public WPFOutput() {

		}

		void CreateWindow() {

		}

		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			return false;
		}


		//---------------------------------------------- Settinggable ----------------------------------------------
		public override SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.output.WPFOutputSettings();
		}
	}
}
