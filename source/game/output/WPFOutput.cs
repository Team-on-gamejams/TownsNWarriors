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
	GameGrid:
		Canvas: 100

		GameCellShape 25:

		SityShape 50:
			WarriorsLabel: 20
			City: 30
	*/
	class WPFOutput : BasicOutput {
		//---------------------------------------------- Fields ----------------------------------------------
		Grid mainGrid;
		Canvas mainCanvas;
		window.GameWindow window;

		public Brush windowBackgroundColor;

		public Brush gameGridBackgroundColor;
		public Brush roadColor;
		public double roadWidthMod, roadHeightMod;

		public bool cityIsSquere;
		public double citySizeMod;
		public List<Brush> cityShapesColors;
		public List<Brush> cityStrokesColors;
		public double cityStrokeThickness;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public WPFOutput(Game game, window.GameWindow gameWindow) : base(game) {
			window = gameWindow;

			InitWindow();
			InitCityEvents();
			InitGameMapGrids();
		}

		protected virtual void InitWindow() {
			window.Background = windowBackgroundColor;

			mainGrid = window.mainGameGrid;
			for (int i = 0; i < this.game.X; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < game.Y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainCanvas = new Canvas();
			Grid.SetColumnSpan(mainCanvas, mainGrid.ColumnDefinitions.Count);
			Grid.SetRowSpan(mainCanvas, mainGrid.RowDefinitions.Count);
			Grid.SetZIndex(mainCanvas, 100);
			mainGrid.Children.Add(mainCanvas);
		}

		void InitCityEvents() {
			foreach (var city in game.GameMap.Cities) {
				city.OutputInfo = new OutputInfoWPF();
				city.FirstTick += City_InitGrid;
				city.FirstTick += City_InitShape;
				city.FirstTick += City_InitWarriorsCnt;
				city.UnitIncome += City_UnitIncome;
			}
		}

		void InitGameMapGrids() {
			for(int i = 0; i < game.GameMap.Map.Count; ++i) {
				for (int j = 0; j < game.GameMap.Map[i].Count; ++j) {
					Grid mapCellGrid = new Grid {
						Background = gameGridBackgroundColor,
					};
					Grid.SetColumn(mapCellGrid, j);
					Grid.SetRow(mapCellGrid, i);
					Grid.SetZIndex(mapCellGrid, 25);
					mainGrid.Children.Add(mapCellGrid);

					var gameCell = game.GameMap.Map[i][j];
					gameCell.OutputInfo = new OutputInfoWPF() { cityGrid = mapCellGrid };

					if (gameCell.IsOpenLeft) {
						Shape road = FormRoad();
						road.Height *= roadHeightMod;
						road.HorizontalAlignment = HorizontalAlignment.Left;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, heightMod: roadHeightMod);
					}
					if (gameCell.IsOpenRight) {
						Shape road = FormRoad();
						road.Height *= roadHeightMod;
						road.HorizontalAlignment = HorizontalAlignment.Right;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, heightMod: roadHeightMod);
					}
					if (gameCell.IsOpenTop) {
						Shape road = FormRoad();
						road.Width *= roadWidthMod;
						road.VerticalAlignment = VerticalAlignment.Top;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, widthMod: roadWidthMod);
					}
					if (gameCell.IsOpenBottom) {
						Shape road = FormRoad();
						road.Width *= roadWidthMod;
						road.VerticalAlignment = VerticalAlignment.Bottom;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, widthMod: roadWidthMod);
					}

					if ((gameCell.IsOpenLeft || gameCell.IsOpenRight) &&
						(gameCell.IsOpenTop || gameCell.IsOpenBottom)
						) {
						Shape road = FormRoad();
						SetRoadSize(road, roadWidthMod, roadHeightMod);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, roadWidthMod, roadHeightMod);
						mapCellGrid.Children.Add(road);
					}

				}
			}

			Shape FormRoad() {
				var rect = new Rectangle {
					Fill = roadColor,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
				};
				SetRoadSize(rect);
				return rect;
			}
			void SetRoadSize(Shape shape, double widthMod = 0.5, double heightMod = 0.5) {
				var originalSize = GetSizeWithMod(1);
				shape.Width = originalSize.Item1 * widthMod;
				shape.Height = originalSize.Item2 * heightMod;
			}
		}

		//---------------------------------------------- Events - city ----------------------------------------------
		void City_InitGrid(city.events.BasicCityEvent basicCityEvent) {
			Grid cityGrid = new Grid();
			Grid.SetColumn(cityGrid, basicCityEvent.city.X);
			Grid.SetRow(cityGrid, basicCityEvent.city.Y);
			Grid.SetZIndex(cityGrid, 50);
			mainGrid.Children.Add(cityGrid);
			(basicCityEvent.city.OutputInfo as OutputInfoWPF).cityGrid = cityGrid;
		}

		void City_InitShape(city.events.BasicCityEvent basicCityEvent) {
			var cityOut = (basicCityEvent.city.OutputInfo as OutputInfoWPF);
			Shape shape = new Ellipse();

			SetShapeStyle(shape, basicCityEvent.city.PlayerId);
			basicCityEvent.city.Captured += (b) => SetShapeStyle(shape, basicCityEvent.city.PlayerId);
		
			ResizeCityShape(shape, this.citySizeMod);
			cityOut.cityGrid.SizeChanged += (b, c) => ResizeCityShape(shape, this.citySizeMod);

			Grid.SetZIndex(shape, 30);
			cityOut.cityGrid.Children.Add(shape);
			cityOut.cityShape = shape;
		}

		void City_InitWarriorsCnt(city.events.BasicCityEvent basicCityEvent) {

		}

		void City_UnitIncome(city.events.CityIncomeEvent cityEvent) {

		}

		//---------------------------------------------- Events - Support ----------------------------------------------
		Tuple<double, double> GetSizeWithMod(double mod) {
			double citySizeX = mainGrid.GetOneCellSize().X * mod,
			citySizeY = mainGrid.GetOneCellSize().Y * mod;

			return new Tuple<double, double>(citySizeX, citySizeY);
		}

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

		void ResizeCityShape(Shape shape, double mod) {
			var size = GetCitySizeWithMod(mod);
			shape.Width = size.Item1;
			shape.Height = size.Item1;
		}

		void SetShapeStyle(Shape shape, int PlayerId) {
			shape.StrokeThickness = cityStrokeThickness;
			shape.Fill = cityShapesColors[PlayerId % cityShapesColors.Count];
			shape.Stroke = cityStrokesColors[PlayerId % cityStrokesColors.Count];
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
