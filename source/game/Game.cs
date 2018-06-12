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
		GameWindow IOWindow;

		bool isPlay;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public Game(GameWindow IOWindow, int x, int y) {
			this.IOWindow = IOWindow;
			isPlay = true;
			mainGrid = IOWindow.mainGameGrid;

			for(int i = 0; i < x; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainCanvas = new Canvas();
			Grid.SetZIndex(mainCanvas, 2);
			mainGrid.Children.Add(mainCanvas);

			//settings.size.oneCellSizeX = mainGrid.RenderSize.Width / x;
			//settings.size.oneCellSizeY = mainGrid.RenderSize.Height / y;
			settings.size.oneCellSizeX = 0;
			settings.size.oneCellSizeY = 0;

			gameMap = GameMap.GenerateRandomMap(
				(int)DateTime.Now.Ticks, 
				//1340092764,
				x, y, 
				new game.map.mapGenerators.TunnelMapGenerator(),
				new game.map.mapGenerators.SityPlacer14(),
				new game.map.mapGenerators.CityIdDiffCorners()
				);
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public void Play() {
			Init();

			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = 500;
			timer.Tick += (a, b) => {
				if (isPlay) Loop();
			};
			timer.Start();
		}

		void Init() {
			gameMap.SetCanvas(mainCanvas);
			gameMap.DrawStatic(mainGrid);

			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = 10;
			timer.Tick += (a, b) => {
				if (settings.size.oneCellSizeX != mainGrid.RenderSize.Width / gameMap.SizeX)
					++settings.size.oneCellSizeX;
				if (settings.size.oneCellSizeY != mainGrid.RenderSize.Height / gameMap.SizeY)
					++settings.size.oneCellSizeY;

				if ((int)settings.size.oneCellSizeX == (int)(mainGrid.RenderSize.Width / gameMap.SizeX) ||
					(int)settings.size.oneCellSizeY == (int)(mainGrid.RenderSize.Height / gameMap.SizeY)) {
					timer.Stop();
					mainGrid.SizeChanged += (c, d) => {
						settings.size.oneCellSizeX = d.NewSize.Width / gameMap.SizeX;
						settings.size.oneCellSizeY = d.NewSize.Height / gameMap.SizeY;
					};
				}

				IOWindow.ForceResize();
			};
			timer.Start();
		}

		void Loop() {
			gameMap.UpdateMap();
			gameMap.Tick();
			++game.globalGameInfo.tick;
		}

	}
}
