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

using taw.game.basicInterfaces;
using taw.game.controlable.playerControl;

namespace taw.game.settings.controlable.playerControl {
	class WPFLocalPlayerSettings : BasicPlayerSettigns {
		public override void SetSettings(ISettingable obj) {
			if (!(obj is WPFLocalPlayer player))
				throw new ApplicationException("Wrong wpfplayer in LocalPlayer1Settings.SetSettings");

			base.SetSettings(obj);

			player.citySelectedStrokeThickness = 3;
			player.citySelectedStrokeColor = Brushes.Yellow;

	}

		protected override void LoadSettingsFromFile() {
			throw new NotImplementedException();
		}
	}
}
