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

using taw.window;
using taw.game.map;

using taw.game.output;
using taw.game.controlable.botControl;
using taw.game.controlable.playerControl;
using taw.game.settings;

namespace taw.game {
	public class Game : basicInterfaces.Settingable {
		//---------------------------------------------- Fields ----------------------------------------------
		//Deprecated
		GameMap gameMap;
		Grid mainGrid;
		Canvas mainCanvas;
		GameWindow IOWindow;

		bool isPlay;

		System.Windows.Forms.Timer loopTimer = new System.Windows.Forms.Timer();

		//New
		private int y;
		private int x;

		List<controlable.Controlable> controlsInput;
		BasicOutput output;

		//---------------------------------------------- Properties ----------------------------------------------
		public int X { get => x; set => x = value; }
		public int Y { get => y; set => y = value; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public Game(GameWindow IOWindow) {
			GetSettings(CreateLinkedSetting());

			this.IOWindow = IOWindow;
			isPlay = true;
			mainGrid = IOWindow.mainGameGrid;

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
			for (int i = 0; i < X; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < Y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainCanvas = new Canvas();
			Grid.SetZIndex(mainCanvas, 2);
			mainGrid.Children.Add(mainCanvas);
		}

		void CreateGameMap() {
			settings.size.OneCellSizeX = 0;
			settings.size.OneCellSizeY = 0;

			settings.values.seed = (int) DateTime.Now.Ticks;

			gameMap = new GameMap(X, Y);

			var mapGen = new game.map.generators.map.TunnelMapGenerator();
			mapGen.SetGameMap(gameMap);
			mapGen.GenerateRandomMap();
			var cityGen = new game.map.generators.city.SityPlacer14();
			cityGen.SetGameMap(gameMap);
			cityGen.PlaceSities();
			var idGen = new game.map.generators.idSetters.IdSetterDiffCorners();
			idGen.SetGameMap(gameMap);
			idGen.SetId();

			output = new WPFOutput();

			controlsInput = new List<controlable.Controlable>(idGen.bots + 1);
			controlsInput.Add(new game.controlable.playerControl.WPFLocalPlayer(1));
			for (int i = 0; i < idGen.bots; ++i)
				controlsInput.Add(new game.controlable.botControl.RushBot(gameMap, gameMap.Sities, gameMap.Units, (byte)(i + 2)));
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
			gameMap.Tick();

			foreach (var control in controlsInput)
				if (control != null)
					control.TickReact();

			++game.globalGameInfo.tick;
			gameMap.UpdateMap();
		}
		//---------------------------------------------- Settingable ----------------------------------------------
		public void GetSettings(SettinsSetter settingsSetter) {
			settingsSetter.SetSettings(this);
		}

		public SettinsSetter CreateLinkedSetting() {
			return new settings.game.GameSettings();
		}
	}
}
