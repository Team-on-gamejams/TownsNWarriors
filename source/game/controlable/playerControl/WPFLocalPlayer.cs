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

using taw.game;
using taw.game.settings;
using taw.game.unit;
using taw.game.city;
using taw.game.map;
using taw.game.city.events;

using taw.game.output.wpf;

namespace taw.game.controlable.playerControl {
	class WPFLocalPlayer : BasicPlayer {
		//---------------------------------------------- Fields ----------------------------------------------
		static List<city.BasicCity> selectedCity;

		window.GameWindow window;


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public WPFLocalPlayer(byte PlayerId, Game game, window.GameWindow gameWindow) : base(PlayerId, game) {
			selectedCity = new List<BasicCity>(game.GameMap.Cities.Count);

			InitCityEvents();
		}

		void InitCityEvents() {
			foreach (var city in game.GameMap.Cities) {
				city.InputInfo = null;
				city.FirstTick += City_InitLMBPress;
			}
			
		}

		//---------------------------------------------- Events - city ----------------------------------------------
		private void City_InitLMBPress(BasicCityEvent cityEvent) {
			if (!(cityEvent.city.OutputInfo is OutputInfoWPF outInfo))
				throw new ApplicationException("Wrong OutputInfo in taw.game.controlable.playerControl.WPFLocalPlayer.City_InitLMBPress(BasicCityEvent cityEvent). Must be CityOutputInfoWPF");

			outInfo.cityGrid.MouseLeftButtonDown += (a, b)=>{
				if (cityEvent.city.PlayerId == PlayerId) {
					if (!selectedCity.Contains(cityEvent.city)) {
						selectedCity.Add(cityEvent.city);
					}
				}
				else {
					if (selectedCity.Count != 0) {
						game.GameMap.SendWarriors(selectedCity, cityEvent.city);
						selectedCity.Clear();
					}
				}
			};


		}

		//---------------------------------------------- Events - Support ----------------------------------------------


		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			return false;
		}

		//---------------------------------------------- Settinggable ----------------------------------------------
		public override SettinsSetter CreateLinkedSetting() {
			return new settings.controlable.playerControl.WPFLocalPlayerSettings();
		}
	}
}
