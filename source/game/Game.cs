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
	public class Game : basicInterfaces.ISettingable {
		//---------------------------------------------- Fields ----------------------------------------------
		GameMap gameMap;

		System.Windows.Forms.Timer loopTimer = new System.Windows.Forms.Timer();

		bool isPlay;

		private int y;
		private int x;

		List<controlable.Controlable> controlsInput;
		BasicOutput output;

		//---------------------------------------------- Properties ----------------------------------------------
		public int X { get => x; set => x = value; }
		public int Y { get => y; set => y = value; }
		public GameMap GameMap { get => gameMap; set => gameMap = value; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public Game() {
			SetSettings(CreateLinkedSetting());

			isPlay = true;

			loopTimer.Interval = GlobalGameInfo.milisecondsPerTick;
			loopTimer.Tick += (a, b) => {
				if (isPlay) 
					Loop();
			};
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public void Play(output.BasicOutput output, List<controlable.Controlable> controlables) {
			isPlay = true;
			game.GlobalGameInfo.tick = 1;
			this.output = output;
			controlsInput = controlables;

			loopTimer.Start();
		}

		public void CreateGameMap(game.map.generators.map.BasicMapGenerator mapGenerator,
			game.map.generators.city.BasicCityPlacer sityPlacer,
			game.map.generators.idSetters.BasicIdSetter idSetter
			) {
			GameMap = new GameMap(X, Y);

			mapGenerator.SetGameMap(GameMap);
			mapGenerator.GenerateRandomMap();

			sityPlacer.SetGameMap(GameMap);
			sityPlacer.PlaceSities();

			idSetter.SetGameMap(GameMap);
			idSetter.SetId();
		}

		void Loop() {
			GameMap.Tick();

			foreach (var control in controlsInput)
				if (control != null)
					control.TickReact();

			++game.GlobalGameInfo.tick;
			output.TickReact();
		}
		//---------------------------------------------- Settingable ----------------------------------------------
		public void SetSettings(SettinsSetter settingsSetter) {
			settingsSetter.SetSettings(this);
		}

		public SettinsSetter CreateLinkedSetting() {
			return new settings.game.GameSettings();
		}
	}
}
