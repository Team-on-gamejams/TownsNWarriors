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

			settings.size.OneCellSizeX = 0;
			settings.size.OneCellSizeY = 0;

			settings.values.seed = (int)
				DateTime.Now.Ticks;
				//1340092764;

			gameMap = GameMap.GenerateRandomMap(
				x, y, 
				new game.map.mapGenerators.TunnelMapGenerator(),
				new game.map.mapGenerators.SityPlacer14(),
				new game.map.mapGenerators.CityIdDiffCorners()
				);

			for (int i = 0; i < settings.values.generator_CityId_Bots; ++i)
				gameMap.SetBot(i, new bot.RushBot(gameMap, gameMap.Sities, gameMap.Units, (byte)(i + 2)));
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public void Play() {
			Init();

			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = settings.values.milisecondsPerTick;
			timer.Tick += (a, b) => {
				if (isPlay) {
					Loop();
				}
			};
			timer.Start();
		}

		void Init() {
			gameMap.SetCanvas(mainCanvas);
			gameMap.DrawStatic(mainGrid);
			SetGrowTimer();
		}

		void Loop() {
			++game.globalGameInfo.tick;
			//MessageBox.Show("game" + game.globalGameInfo.tick.ToString());
			gameMap.UpdateMap();
			gameMap.Tick();

		}

		void SetGrowTimer() {
			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = 10;

			timer.Tick += (a, b) => {
				if ((int)settings.size.OneCellSizeX < (int)(mainGrid.RenderSize.Width / gameMap.SizeX))
					settings.size.OneCellSizeX += settings.size.growIncr;
				if ((int)settings.size.OneCellSizeY < (int)(mainGrid.RenderSize.Height / gameMap.SizeY))
					settings.size.OneCellSizeY += settings.size.growIncr;

				if ((int)settings.size.OneCellSizeX >= (int)(mainGrid.RenderSize.Width / gameMap.SizeX) &&
					(int)settings.size.OneCellSizeY >= (int)(mainGrid.RenderSize.Height / gameMap.SizeY)) {
					timer.Stop();
					mainGrid.SizeChanged += (c, d) => {
						settings.size.OneCellSizeX = d.NewSize.Width / gameMap.SizeX;
						settings.size.OneCellSizeY = d.NewSize.Height / gameMap.SizeY;
					};
					IOWindow.Width++;
				}
			};

			timer.Start();
		}
	}
}
