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
using taw.game.controlable.playerControl.wpf;

namespace taw.game.controlable.playerControl {
	class WPFLocalPlayer : BasicPlayer {
		//---------------------------------------------- Fields ----------------------------------------------
		static List<city.BasicCity> selectedCity;
		static List<city.BasicCity>[] selectedCityGroups;

		window.GameWindow window;

		public double citySelectedStrokeThickness;
		public Brush citySelectedStrokeColor;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		static WPFLocalPlayer() {
			selectedCity = new List<BasicCity>(10);
			selectedCityGroups = new List<BasicCity>[10];
			for(byte i = 0; i < 10; ++i) {
				selectedCityGroups[i] = new List<BasicCity>();
			}
		}

		public WPFLocalPlayer(byte PlayerId, Game game, window.GameWindow gameWindow) : base(PlayerId, game) {
			window = gameWindow;

			InitCityEvents();
			InitHotkeys(window);
		}

		void InitCityEvents() {
			foreach (var city in game.GameMap.Cities) {
				city.InputInfo = new CityInputInfoWPF();
				city.FirstTick += City_InitLMBPress;
			}

		}

		void InitHotkeys(Window window) {
			window.InputBindings.Add(
				new InputBinding(new HotkeyCommand() { ExecuteDelegate=SelectAll},
						 new KeyGesture(Key.A, ModifierKeys.Control)
				)
			);
		}

		//---------------------------------------------- Hotkeys ----------------------------------------------
		public class HotkeyCommand : ICommand {
			private Window _window;

			public Action<object> ExecuteDelegate;

			public event EventHandler CanExecuteChanged;
			public bool CanExecute(object parameter) => true;

			public void Execute(object eventArgs) {
				ExecuteDelegate?.Invoke(eventArgs);
			}
		}

		void SelectAll(object eventArgs) {
			bool isAddOne = false;
			foreach (var city in game.GameMap.Cities) {
				if (city.PlayerId == PlayerId && !selectedCity.Contains(city)) {
					SelectCity(city);
					isAddOne = true;
				}
			}
			if (!isAddOne)
				UnselectAll();
		}


		//---------------------------------------------- Events - city ----------------------------------------------
		private void City_InitLMBPress(BasicCityEvent cityEvent) {
			if (!(cityEvent.city.OutputInfo is OutputInfoWPF outInfo))
				throw new ApplicationException("Wrong OutputInfo in taw.game.controlable.playerControl.WPFLocalPlayer.City_InitLMBPress(BasicCityEvent cityEvent). Must be CityOutputInfoWPF");

			outInfo.cityShape.MouseLeftButtonDown  += (a, b) => SelectCity(cityEvent.city);
			outInfo.warriorCnt.MouseLeftButtonDown += (a, b) => SelectCity(cityEvent.city);

		}

		void SelectCity(BasicCity city) {
			var inInfo = city.InputInfo as CityInputInfoWPF;
			var outInfo = city.OutputInfo as OutputInfoWPF;

			if (city.PlayerId == PlayerId && !selectedCity.Contains(city)) {
				selectedCity.Add(city);
				inInfo.strokeColorBeforeSelection = outInfo.cityShape.Stroke;
				inInfo.strokeThicknessBeforeSelection = outInfo.cityShape.StrokeThickness;
				outInfo.cityShape.Stroke = this.citySelectedStrokeColor;
				outInfo.cityShape.StrokeThickness = this.citySelectedStrokeThickness;
			}
			else if (selectedCity.Count != 0) {
				game.GameMap.SendWarriors(selectedCity, city);
				UnselectAll();
			}
		}

		void UnselectAll() {
			selectedCity.ForEach((a) => {
				(a.OutputInfo as OutputInfoWPF).cityShape.Stroke =
				(a.InputInfo as CityInputInfoWPF).strokeColorBeforeSelection;
				(a.OutputInfo as OutputInfoWPF).cityShape.StrokeThickness =
				(a.InputInfo as CityInputInfoWPF).strokeThicknessBeforeSelection;
			});
			selectedCity.Clear();
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
