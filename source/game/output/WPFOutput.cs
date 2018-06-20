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
		Warriors: 3
		City: 50
	*/
	class WPFOutput : BasicOutput {
		//---------------------------------------------- Fields ----------------------------------------------
		Grid mainGrid;
		Canvas mainCanvas;
		window.GameWindow window;

		public bool cityIsSquere;
		public double citySizeMod;
		
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
			Grid.SetColumnSpan(mainCanvas, mainGrid.ColumnDefinitions.Count);
			Grid.SetRowSpan(mainCanvas, mainGrid.RowDefinitions.Count);
			Grid.SetZIndex(mainCanvas, 100);
			mainGrid.Children.Add(mainCanvas);
		}

		void InitCityEvents() {
			foreach (var city in game.GameMap.Cities) {
				city.OutputInfo = new CityOutputInfo();
				city.FirstTick += City_InitGrid;
				city.FirstTick += City_InitShape;
				city.FirstTick += City_InitWarriorsCnt;
				city.UnitIncome += City_UnitIncome;
			}
		}

		//---------------------------------------------- Events - city ----------------------------------------------
		void City_InitGrid(city.events.BasicCityEvent basicCityEvent) {
			Grid cityGrid = new Grid();
			Grid.SetColumn(cityGrid, basicCityEvent.city.Y);
			Grid.SetRow(cityGrid, basicCityEvent.city.X);
			mainGrid.Children.Add(cityGrid);

			(basicCityEvent.city.OutputInfo as CityOutputInfo).cityGrid = cityGrid;
		}

		void City_InitShape(city.events.BasicCityEvent basicCityEvent) {
			var cityOut = (basicCityEvent.city.OutputInfo as CityOutputInfo);

			Shape shape = new Ellipse() {
				Fill = Brushes.Red,
				Stroke = Brushes.Black,
				StrokeThickness = 2,
			};

			ResizeShape();
			cityOut.cityGrid.SizeChanged += (b, c) => ResizeShape();

			Grid.SetZIndex(shape, 50);
			cityOut.cityGrid.Children.Add(shape);

			cityOut.cityShape = shape;

			void ResizeShape() {
				var size = GetCitySizeWithMod(this.citySizeMod);
				shape.Width = size.Item1;
				shape.Height = size.Item1;
			}
		}

		void City_InitWarriorsCnt(city.events.BasicCityEvent basicCityEvent) {
			
		}

		void City_UnitIncome(city.events.CityIncomeEvent cityEvent) {

		}

		//---------------------------------------------- Events - Support ----------------------------------------------
		Tuple<double, double> GetCitySizeWithMod(double mod) {
			double citySizeX = mainGrid.GetOneCellSize().X * mod,
			citySizeY = mainGrid.GetOneCellSize().Y * mod;
			if (cityIsSquere) {
				if (citySizeX > citySizeY)
					citySizeX = citySizeY;
				else if (citySizeY > citySizeX)
					citySizeY = citySizeX;
			}

			return new Tuple<double, double>(citySizeX, citySizeY);
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			return false;
		}


		//---------------------------------------------- Settinggable ----------------------------------------------
		public override SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.output.WPFOutputSettings();
		}


		public class CityOutputInfo {
			public Grid cityGrid;
			public Shape cityShape;
			public PathFigure warriorsArc;
		}
	}
}
