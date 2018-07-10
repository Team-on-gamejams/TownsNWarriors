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
			for(byte i = 0; i < 10; ++i)
				selectedCityGroups[i] = new List<BasicCity>();
		}

		public WPFLocalPlayer(byte PlayerId, Game game, window.GameWindow gameWindow) : base(PlayerId, game) {
			window = gameWindow;

			InitCityEvents();
			InitHotkeys(window);
		}

		void InitCityEvents() {
			foreach (var city in game.GameMap.Cities) {
				city.InputInfo = new CityInputInfoWPF();
				city.FirstTick += City_InitMouse;
			}

		}

		void InitHotkeys(Window window) {
			window.InputBindings.Add(new InputBinding(
				new HotkeyCommand() { ExecuteDelegate=SelectAll}, 
				new KeyGesture(Key.A, ModifierKeys.Control)));

			//WPF не дозволяє ставити KeyGesture на цифри без ModifierKeys
			window.KeyDown += (a, b) => {
				if(Key.D0 <= b.Key && b.Key <= Key.D9) {
					if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
						CreateGroup((byte)(b.Key - Key.D0));
					else if((Keyboard.Modifiers & ModifierKeys.Shift) > 0)
						AddToGroup((byte)(b.Key - Key.D0));
					else
						SelectGroup((byte)(b.Key - Key.D0));
				}
			};
		}

		//---------------------------------------------- Hotkeys ----------------------------------------------
		public class HotkeyCommand : ICommand {
			public event EventHandler CanExecuteChanged;
			public bool CanExecute(object parameter) => true;
			public Action<object> ExecuteDelegate;
			public virtual void Execute(object eventArgs) => ExecuteDelegate?.Invoke(eventArgs);
		}

		void SelectAll(object eventArgs) {
			bool isAddOne = false;
			foreach (var city in game.GameMap.Cities) {
				if (city.PlayerId == PlayerId && !selectedCity.Contains(city)) {
					SelectCity(city, false);
					isAddOne = true;
				}
			}
			if (!isAddOne)
				UnselectAll();
		}

		void CreateGroup(byte groupNum) {
			if (selectedCity.Count != 0) {
				selectedCityGroups[groupNum].Clear();
				for (int i = 0; i < selectedCity.Count; ++i)
					selectedCityGroups[groupNum].Add(selectedCity[i]);
			}
		}

		void AddToGroup(byte groupNum) {
			if (selectedCity.Count != 0) {
				for (int i = 0; i < selectedCity.Count; ++i)
					selectedCityGroups[groupNum].Add(selectedCity[i]);
			}
		}

		void SelectGroup(byte groupNum) {
			//if (selectedCityGroups[groupNum].Count != 0) {
				UnselectAll();
				for (int i = 0; i < selectedCityGroups[groupNum].Count; ++i)
					SelectCity(selectedCityGroups[groupNum][i], false);
			//}
		}


		//---------------------------------------------- Events - city ----------------------------------------------
		private void City_InitMouse(BasicCityEvent cityEvent) {
			if (!(cityEvent.city.OutputInfo is OutputInfoWPF outInfo))
				throw new ApplicationException("Wrong OutputInfo in taw.game.controlable.playerControl.WPFLocalPlayer.City_InitLMBPress(BasicCityEvent cityEvent). Must be CityOutputInfoWPF");

			outInfo.cityShape.MouseLeftButtonDown  += (a, b) => SelectCity(cityEvent.city, true);
			outInfo.warriorCnt.MouseLeftButtonDown += (a, b) => SelectCity(cityEvent.city, true);

			outInfo.cityShape.MouseRightButtonDown += (a, b) => SendUnits(cityEvent.city);
			outInfo.warriorCnt.MouseRightButtonDown += (a, b) => SendUnits(cityEvent.city);
		}

		void SelectCity(BasicCity city, bool IsRemoveIfSelected) {
			if (!selectedCity.Contains(city)) {
				if (city.PlayerId == PlayerId) {
					var inInfo = city.InputInfo as CityInputInfoWPF;
					var outInfo = city.OutputInfo as OutputInfoWPF;

					selectedCity.Add(city);
					inInfo.strokeColorBeforeSelection = outInfo.cityShape.Stroke;
					inInfo.strokeThicknessBeforeSelection = outInfo.cityShape.StrokeThickness;
					outInfo.cityShape.Stroke = this.citySelectedStrokeColor;
					outInfo.cityShape.StrokeThickness = this.citySelectedStrokeThickness;
				}
			}
			else if (IsRemoveIfSelected)
				UnselectCity(city);
		}

		void SendUnits(BasicCity city) {
			foreach (var i in selectedCity)
				if(i.PlayerId == PlayerId)
					i.SendUnit(city);
			UnselectAll();
		}

		void UnselectCity(BasicCity city) {
			(city.OutputInfo as OutputInfoWPF).cityShape.Stroke =
				(city.InputInfo as CityInputInfoWPF).strokeColorBeforeSelection;
			(city.OutputInfo as OutputInfoWPF).cityShape.StrokeThickness =
				(city.InputInfo as CityInputInfoWPF).strokeThicknessBeforeSelection;
			selectedCity.Remove(city);
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
