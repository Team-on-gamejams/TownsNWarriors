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
			Unit
				UnitShape
				UnitLabel

		SityShape 50:
			City: 30
			WarriorsLabel: 40

		GameCellShape 25:
	*/
	class WPFOutput : BasicOutput {
		//---------------------------------------------- Fields ----------------------------------------------
		static Point cellSize;

		Grid mainGrid;
		Canvas mainCanvas;
		window.GameWindow window;

		public Brush windowBackgroundColor;

		public Brush gameGridBackgroundColor;
		public Brush roadColor;
		public double roadWidthMod, roadHeightMod;

		public bool unitIsSquere;
		public double unitSizeMod;
		public Brush unitTextColor;


		public bool cityIsSquere;
		public double citySizeMod;
		public List<Brush> cityShapesColors;
		public List<Brush> cityStrokesColors;
		public double cityStrokeThickness;

		public double unitWarriorsCntRelativeMod;

		public double cityWarriorsCntRelativeMod;
		public Brush cityWarriorsCntStrokeColor;

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
			for(int i = 0; i < this.game.X; ++i)
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
			for(int i = 0; i < game.Y; ++i)
				mainGrid.RowDefinitions.Add(new RowDefinition());

			mainCanvas = new Canvas();
			Grid.SetColumnSpan(mainCanvas, mainGrid.ColumnDefinitions.Count);
			Grid.SetRowSpan(mainCanvas, mainGrid.RowDefinitions.Count);
			Grid.SetZIndex(mainCanvas, 100);
			mainGrid.Children.Add(mainCanvas);

			ChangeLinkedToGridValues();
			mainGrid.SizeChanged += (a, b) => ChangeLinkedToGridValues();

			window.dominationBar.Children.Add(new TextBox() { Text = Rand.Seed.ToString() });

			void ChangeLinkedToGridValues() {
				cellSize = mainGrid.GetOneCellSize();
				var unitSize = GetSizeWithMod(this.unitSizeMod, unitIsSquere);
				UnitOutputInfoWPF.shiftX = cellSize.X / 2 - unitSize.Item1 / 2;
				UnitOutputInfoWPF.shiftY = cellSize.Y / 2 - unitSize.Item2 / 2;
			}
		}

		void InitCityEvents() {
			foreach(var city in game.GameMap.Cities) {
				city.OutputInfo = new OutputInfoWPF();
				city.FirstTick += City_InitGrid;
				city.FirstTick += City_InitShape;
				city.FirstTick += City_InitWarriorsCnt;
				city.UnitIncome += City_UnitIncome;
				city.UnitGet += City_UnitGet;
				city.UnitSend += City_UnitSend;
			}
		}

		void InitGameMapGrids() {
			for(int i = 0; i < game.GameMap.Map.Count; ++i) {
				for(int j = 0; j < game.GameMap.Map[i].Count; ++j) {
					Grid mapCellGrid = new Grid {
						Background = gameGridBackgroundColor,
					};
					Grid.SetColumn(mapCellGrid, j);
					Grid.SetRow(mapCellGrid, i);
					Grid.SetZIndex(mapCellGrid, 25);
					mainGrid.Children.Add(mapCellGrid);

					var gameCell = game.GameMap.Map[i][j];
					gameCell.OutputInfo = new OutputInfoWPF() { cityGrid = mapCellGrid };

					if(gameCell.IsOpenLeft) {
						Shape road = FormRoad();
						road.Height *= roadHeightMod;
						road.HorizontalAlignment = HorizontalAlignment.Left;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, heightMod: roadHeightMod);
					}
					if(gameCell.IsOpenRight) {
						Shape road = FormRoad();
						road.Height *= roadHeightMod;
						road.HorizontalAlignment = HorizontalAlignment.Right;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, heightMod: roadHeightMod);
					}
					if(gameCell.IsOpenTop) {
						Shape road = FormRoad();
						road.Width *= roadWidthMod;
						road.VerticalAlignment = VerticalAlignment.Top;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, widthMod: roadWidthMod);
					}
					if(gameCell.IsOpenBottom) {
						Shape road = FormRoad();
						road.Width *= roadWidthMod;
						road.VerticalAlignment = VerticalAlignment.Bottom;
						mapCellGrid.Children.Add(road);
						mapCellGrid.SizeChanged += (a, b) => SetRoadSize(road, widthMod: roadWidthMod);
					}

					if((gameCell.IsOpenLeft || gameCell.IsOpenRight) &&
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
				var originalSize = GetSizeWithMod(1, false);
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
			var cityOut = (basicCityEvent.city.OutputInfo as OutputInfoWPF);
			TextBlock textBlock = new TextBlock {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Foreground = this.cityWarriorsCntStrokeColor,
				Text = basicCityEvent.city.currWarriors + " / " + basicCityEvent.city.maxWarriors,
			};

			Viewbox viewbox = new Viewbox() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Child = textBlock,
			};

			ResizeViewbox();
			cityOut.cityGrid.SizeChanged += (a, b) => ResizeViewbox();

			Grid.SetZIndex(viewbox, 40);
			cityOut.cityGrid.Children.Add(viewbox);
			cityOut.warriorCnt = textBlock;

			void ResizeViewbox() {
				var size = GetSizeWithMod(this.citySizeMod, cityIsSquere);
				viewbox.Width = size.Item1 * cityWarriorsCntRelativeMod;
				viewbox.Height = size.Item2 * cityWarriorsCntRelativeMod;
			}
		}

		void City_UnitIncome(city.events.CityIncomeEvent cityEvent) {
			(cityEvent.city.OutputInfo as OutputInfoWPF).warriorCnt.Text = cityEvent.city.currWarriors + " / " + cityEvent.city.maxWarriors;
		}

		void City_UnitGet(city.events.CityUnitsEvent cityEvent) {
			City_UnitIncome(new city.events.CityIncomeEvent((cityEvent as city.events.BasicCityEvent),
				cityEvent.city.PlayerId == cityEvent.unit.PlayerId
				));
		}

		void City_UnitSend(city.events.CityUnitsEvent cityEvent) {
			cityEvent.unit.OutputInfo = new wpf.UnitOutputInfoWPF();
			cityEvent.unit.FirstTick += Unit_FirstTick;
			cityEvent.unit.Tick += Unit_Tick;
			cityEvent.unit.ReachDestination += Unit_ReachDestination;

			City_UnitIncome(new city.events.CityIncomeEvent((cityEvent as city.events.BasicCityEvent),
				false
				));
		}

		//---------------------------------------------- Events - unit ----------------------------------------------
		void Unit_FirstTick(unit.events.BasicUnitEvent unitEvent) {
			if(!(unitEvent.unit.OutputInfo is UnitOutputInfoWPF outInfo))
				return;

			Grid unitGrid = new Grid();

			Shape unitShape = new Ellipse() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			SetShapeStyle(unitShape, unitEvent.unit.PlayerId);
			Grid.SetZIndex(unitShape, 1);
			unitGrid.Children.Add(unitShape);


			TextBlock textBlock = new TextBlock {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Foreground = this.cityWarriorsCntStrokeColor,
				Text = unitEvent.unit.warriorsCnt.ToString(),
			};

			Viewbox viewbox = new Viewbox() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Child = textBlock,
			};

			Grid.SetZIndex(viewbox, 2);
			unitGrid.Children.Add(viewbox);

			SetSizes();
			mainGrid.SizeChanged += (a, b) => SetSizes();

			mainCanvas.Children.Add(unitGrid);

			outInfo.grid = unitGrid;
			outInfo.shape = unitShape;
			outInfo.unitsCnt = textBlock;

			void SetSizes() {
				var s = GetSizeWithMod(this.unitSizeMod, unitIsSquere);
				unitGrid.Width = s.Item1;
				unitGrid.Height = s.Item2;
				unitShape.Width = s.Item1;
				unitShape.Height = s.Item2;
				viewbox.Width = s.Item1 * unitWarriorsCntRelativeMod;
				viewbox.Height = s.Item2 * unitWarriorsCntRelativeMod;
			}
		}

		void Unit_Tick(unit.events.BasicUnitEvent unitEvent) {
			if(!(unitEvent.unit.OutputInfo is UnitOutputInfoWPF outInfo))
				return;

			double pixelPerTurnX = cellSize.X / unitEvent.unit.tickPerTurn;
			double pixelPerTurnY = cellSize.Y / unitEvent.unit.tickPerTurn;

			double length = UnitOutputInfoWPF.shiftX + unitEvent.unit.X * cellSize.X;
			if(unitEvent.unit.X > unitEvent.unit.NextX)
				length -= unitEvent.unit.currTickOnCell * pixelPerTurnX;
			else if(unitEvent.unit.X < unitEvent.unit.NextX)
				length += unitEvent.unit.currTickOnCell * pixelPerTurnX;
			Canvas.SetLeft(outInfo.grid, length);

			length = UnitOutputInfoWPF.shiftY + unitEvent.unit.Y * cellSize.Y;
			if(unitEvent.unit.Y > unitEvent.unit.NextY)
				length -= unitEvent.unit.currTickOnCell * pixelPerTurnY;
			else if(unitEvent.unit.Y < unitEvent.unit.NextY)
				length += unitEvent.unit.currTickOnCell * pixelPerTurnY;
			Canvas.SetTop(outInfo.grid, length);
		}

		void Unit_ReachDestination(unit.events.UnitReachDestinationEvent unitEvent) {
			if(!(unitEvent.unit.OutputInfo is UnitOutputInfoWPF outInfo))
				return;
			mainCanvas.Children.Remove(outInfo.grid);
		}

		//---------------------------------------------- Events - Support ----------------------------------------------
		Tuple<double, double> GetSizeWithMod(double mod, bool makeSquare) {
			double citySizeX = cellSize.X * mod,
			citySizeY = cellSize.Y * mod;
			if(makeSquare) {
				if(citySizeX > citySizeY)
					citySizeX = citySizeY;
				else if(citySizeY > citySizeX)
					citySizeY = citySizeX;
			}

			return new Tuple<double, double>(citySizeX, citySizeY);
		}

		void ResizeCityShape(Shape shape, double mod) {
			var size = GetSizeWithMod(mod, this.cityIsSquere);
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
