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
using taw.game.output.wpf;


namespace taw.game.output {
	/*Z-Levels:
		Canvas: 100

	*/
	class WPFOutput : BasicOutput {
		//---------------------------------------------- Fields ----------------------------------------------
		Grid mainGrid;
		Canvas mainCanvas;
		window.GameWindow window;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public WPFOutput(Game game, window.GameWindow gameWindow) : base(game) {
			window = gameWindow;

			InitWindow();
			InitCityEvents();
		}

		protected virtual void InitWindow() {
			mainGrid = window.mainGameGrid;
			for (int i = 0; i < this.game.X; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < game.Y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainGrid.ShowGridLines = true;

			mainCanvas = new Canvas();
			Grid.SetZIndex(mainCanvas, 100);
			mainGrid.Children.Add(mainCanvas);
		}

		protected virtual void InitCityEvents() {
			foreach (var city in game.GameMap.Sities) {
				city.FirstTick += City_InitShape;
				city.FirstTick += City_InitWarriorsCnt;
				city.UnitIncome += City_UnitIncome;
			} 
		}

		//---------------------------------------------- Events - city ----------------------------------------------
		protected virtual void City_InitShape(city.events.BasicCityEvent basicCityEvent) {
			Grid cityGrid = new Grid();

			Shape shape = new Ellipse() {
				Fill = Brushes.Red,
				Stroke = Brushes.Black,
				StrokeThickness = 2,
				Width = mainGrid.GetOneCellSize().X * 0.75,
				Height = mainGrid.GetOneCellSize().Y * 0.75,
			};
			cityGrid.Children.Add(shape);
			cityGrid.SizeChanged += (b, c) => {
				shape.Width = mainGrid.GetOneCellSize().X * 0.75;
				shape.Height = mainGrid.GetOneCellSize().Y * 0.75;
			};

			Grid.SetColumn(cityGrid, basicCityEvent.city.Y);
			Grid.SetRow(cityGrid, basicCityEvent.city.X);
			mainGrid.Children.Add(cityGrid);
		}

		protected virtual void City_InitWarriorsCnt(city.events.BasicCityEvent basicCityEvent) {
			//cityGrid.Children.Add(new Label() { Content = basicCityEvent.city.PlayerId.ToString(), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center });

			//Grid.SetColumn(cityGrid, basicCityEvent.city.Y);
			//Grid.SetRow(cityGrid, basicCityEvent.city.X);
			//mainGrid.Children.Add(cityGrid);
		}

		private void City_UnitIncome(city.events.CityIncomeEvent cityEvent) {
			//throw new NotImplementedException();
			//	cityGrid.Children.Add(new Label() { Content = basicCityEvent.city.PlayerId.ToString(), HorizontalAlignment=HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center });
		}


		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			return false;
		}


		//---------------------------------------------- Settinggable ----------------------------------------------
		public override SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.output.WPFOutputSettings();
		}
	}
}
