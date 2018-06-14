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

			loopTimer.Interval = settings.values.milisecondsPerTick;
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

		void InitGameMap() {
			gameMap.SetCanvas(mainCanvas);
			gameMap.DrawStatic(mainGrid);
			SetGrowTimer();
		}

		void Loop() {
			++game.globalGameInfo.tick;
			//MessageBox.Show("game" + game.globalGameInfo.tick.ToString());
			gameMap.UpdateMap();
			gameMap.Tick();

			WinProcess();
		}

		void WinProcess() {
			int id = 0;
			if (IsWin()) {
				isPlay = false;
				loopTimer.Stop();

				string winner = "";
				if (id == 1)
					winner = "You win!";
				else
					winner = "Bot win! Seems like you looser";


				if (MessageBox.Show("Do you want to play again?", winner, MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
					this.Play();
				}
				else {
					IOWindow.Close();
				}
			}

			bool IsWin() {
				foreach (var sity in gameMap.Sities) {
					if (sity.playerId != 0) {
						id = sity.playerId;
						break;
					}
				}

				foreach (var sity in gameMap.Sities)
					if (sity.playerId != 0 && sity.playerId != id)
						return false;

				return true;
			}
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
