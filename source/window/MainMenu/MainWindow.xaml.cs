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

using taw.game;
using taw.game.controlable.botControl;

using taw.game.controlable.botControl.parts.attack;
using taw.game.controlable.botControl.parts.defence;

namespace taw.window {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		//---------------------------- Init ----------------------------
		public MainWindow() {
			InitializeComponent();
			ChoosePlayerGrid.Opacity = 0;
		}

		//---------------------------- Click events ----------------------------
		private void Button_Click_Singleplayer(object sender, RoutedEventArgs e) {
			ChoosePlayerGrid.Opacity = 1;
		}

		private void Button_Click_Hightscores(object sender, RoutedEventArgs e) {
			HightScoresWindow hightScoresWindow = new HightScoresWindow();
			ReopenWindow(this, hightScoresWindow);
		}

		private void Button_Click_Settings(object sender, RoutedEventArgs e) {
			SettingsWindow settingsWindow = new SettingsWindow();
			ReopenWindow(this, settingsWindow);
		}

		private void Button_Click_Credits(object sender, RoutedEventArgs e) {
			//CreditsWindow creditsWindow = new CreditsWindow();
			//ReopenWindow(this, creditsWindow);

			GameWindow gameWindow = new GameWindow();
			MainWindow.ReopenWindow(this, gameWindow);

			game.Game game = new game.Game();

			var mapGen = new game.map.generators.map.TunnelMapGenerator();
			var cityGen = new game.map.generators.city.CityPlacer14();
			var idGen = new game.map.generators.idSetters.IdSetterDiffCorners();
			game.CreateGameMap(mapGen, cityGen, idGen);


			var output = new taw.game.output.WPFOutput(game, gameWindow);
			var controlsInput = new List<taw.game.controlable.Controlable>();

			for (int i = 0; i < idGen.ControlsCnt; ++i)
				if(i == 0)
					controlsInput.Add(new game.controlable.playerControl.WPFLocalPlayer((byte)(i + 1), game, gameWindow));
			//else if(i == 1) {
			//	var partBot = new BasicPartsBot(game.GameMap, (byte)(i + 1));
			//	partBot.AddPart(new CaptureNeutral(100));
			//	partBot.AddPart(new RushWeakestCity(100));
			//	controlsInput.Add(partBot);
			//}
			else
				controlsInput.Add(new RushBot(game.GameMap, (byte)(i + 1)));

			taw.game.controlable.botControl.support.LogicalPlayersSingletone.Init(game.GameMap, idGen.ControlsCnt);

			game.Play(output, controlsInput);

			this.Close();
		}

		private void Button_Click_Exit(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void Button_Click_Create_Profile(object sender, RoutedEventArgs e) {
			SingleplayerWindow singleplayerWindow = new SingleplayerWindow();
			ReopenWindow(this, singleplayerWindow);

			singleplayerWindow.Closing += (a, b) => {
				if (singleplayerWindow.IsExit) {
					this.Close();
					return;
				}
				if (singleplayerWindow.IsBack) 
					ReopenWindow(singleplayerWindow, this);
			};
		}

		private void Button_Click_Delete_Profile(object sender, RoutedEventArgs e) {

		}

		private void Button_Click_Select_Profile(object sender, RoutedEventArgs e) {
		}

		//---------------------------- Support ----------------------------
		public static void ReopenWindow(Window opened, Window toOpen){
			toOpen.Width = opened.ActualWidth;
			toOpen.Height = opened.ActualHeight;

			if (opened.WindowState == WindowState.Maximized) {
				toOpen.WindowState = WindowState.Maximized;
			}
			else {
				toOpen.Top = opened.Top;
				toOpen.Left = opened.Left;
			}

			toOpen.Show();
			opened.Hide();
		}

	}
}