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

		bool isPlay, isNeedToExit;
		int x, y;

		System.Windows.Forms.Timer loopTimer = new System.Windows.Forms.Timer();

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public Game(GameWindow IOWindow, int X, int Y) {
			this.IOWindow = IOWindow;
			isPlay = true;
			isNeedToExit = false;
			mainGrid = IOWindow.mainGameGrid;

			x = X;
			y = Y;

			FillIOWindow();

			loopTimer.Interval = globalGameInfo.milisecondsPerTick;
			loopTimer.Tick += (a, b) => {
				if (isPlay) {
					Loop();
				}
			};
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public void Play() {
			isPlay = true;
			game.globalGameInfo.tick = 1;

			CreateGameMap();
			InitGameMap();

			loopTimer.Start();
		}

		void FillIOWindow() {
			for (int i = 0; i < x; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainCanvas = new Canvas();
			Grid.SetZIndex(mainCanvas, 2);
			mainGrid.Children.Add(mainCanvas);
		}

		void CreateGameMap() {
			settings.size.OneCellSizeX = 0;
			settings.size.OneCellSizeY = 0;

			settings.values.seed = (int) DateTime.Now.Ticks;

			gameMap = new GameMap(x, y);

			var mapGen = new game.map.generators.map.TunnelMapGenerator();
			mapGen.SetGameMap(gameMap);
			mapGen.GenerateRandomMap();
			var cityGen = new game.map.generators.city.SityPlacer14();
			cityGen.SetGameMap(gameMap);
			cityGen.PlaceSities();
			var idGen = new game.map.generators.idSetters.IdSetterDiffCorners();
			idGen.SetGameMap(gameMap);
			idGen.SetId();

			gameMap.SetBotsSize(idGen.bots);
			for (int i = 0; i < idGen.bots; ++i)
				gameMap.SetBot(i, new bot.RushBot(gameMap, gameMap.Sities, gameMap.Units, (byte)(i + 2)));
		}

		void InitGameMap() {
			gameMap.SetCanvas(mainCanvas);
			gameMap.DrawStatic(mainGrid);

			settings.size.OneCellSizeX = mainGrid.RenderSize.Width / gameMap.SizeX;
			settings.size.OneCellSizeY = mainGrid.RenderSize.Height / gameMap.SizeY;
			mainGrid.SizeChanged += (c, d) => {
				settings.size.OneCellSizeX = d.NewSize.Width / gameMap.SizeX;
				settings.size.OneCellSizeY = d.NewSize.Height / gameMap.SizeY;
			};
		}

		void Loop() {
			++game.globalGameInfo.tick;
			gameMap.UpdateMap();
			gameMap.Tick();
		}
	}
}
