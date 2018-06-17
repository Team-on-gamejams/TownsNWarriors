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
using taw.game;
using taw.game.unit;
using taw.game.city;
using taw.game.map;

namespace taw.game.output {
	/*Z-Levels:
		Canvas: 100

	*/
	class WPFOutput : BasicOutput {
		//---------------------------------------------- Fields ----------------------------------------------
		Grid mainGrid;
		Canvas mainCanvas;
		window.GameWindow window;


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public WPFOutput(Game game, window.GameWindow gameWindow) : base(game) {
			window = gameWindow;

			InitWindow();
		}

		void InitWindow() {
			mainGrid = window.mainGameGrid;
			for (int i = 0; i < this.game.X; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < game.Y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainGrid.ShowGridLines = true;

			mainCanvas = new Canvas();
			Grid.SetZIndex(mainCanvas, 100);
			mainGrid.Children.Add(mainCanvas);
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
