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

using TownsAndWarriors.window;
using TownsAndWarriors.game.map;


namespace TownsAndWarriors.game {
	public class Game {
		//---------------------------------------------- Fields ----------------------------------------------
		GameMap gameMap;
		Grid mainGrid;
		Canvas mainCanvas;

		bool isPlay;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public Game(GameWindow IOWindow, int x, int y) {
			isPlay = true;
			mainGrid = IOWindow.mainGameGrid;

			for(int i = 0; i < x; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainCanvas = new Canvas();
			Grid.SetZIndex(mainCanvas, 2);
			mainGrid.Children.Add(mainCanvas);

			gameMap = GameMap.GenerateRandomMap((int)DateTime.Now.Ticks, x, y, 
				new game.map.mapGenerators.TunnelMapGenerator(),
				new game.map.mapGenerators.SityPlacer14()
				);
		}


		//---------------------------------------------- Methods ----------------------------------------------
		public void Play() {
			Init();

			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = 10;
			timer.Tick += (a, b) => {
				if (isPlay) Loop();
			};
			timer.Start();
		}

		void Init() {
			gameMap.SetCanvas(mainCanvas);
			gameMap.DrawStatic(mainGrid);
		}

		void Loop() {
			gameMap.UpdateMap();
			gameMap.Tick();
			++game.globalGameInfo.tick;
		}

	}
}
