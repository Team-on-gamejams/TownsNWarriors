using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.sity {
	public partial class BasicSity : GameCellDrawableObj, tickable, withPlayerId {
		//---------------------------------------------- Fields ----------------------------------------------
		public static TownsAndWarriors.game.map.GameMap gameMap;

		public ushort currWarriors, maxWarriors;
		public double sendPersent, defPersent;

		public ushort ticksPerIncome;
		protected Dictionary<BasicSity, List<KeyValuePair<int, int>>> pathToSities;

		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicSity() {
			maxWarriors = settings.values.basicSity_MaxWarriors;
			currWarriors = settings.values.basicSity_StartWarriors;
			sendPersent = settings.values.basicSity_sendWarriorsPersent;
			defPersent = settings.values.basicSity_defendWarriorsPersent;
			ticksPerIncome = settings.values.basicSity_ticks_NewWarrior;
			pathToSities = new Dictionary<BasicSity, List<KeyValuePair<int, int>>>(1);
        }

        //---------------------------------------------- Methods ----------------------------------------------
        public bool TickReact() {
			if (playerId != 0 && maxWarriors > currWarriors && game.globalGameInfo.tick % ticksPerIncome == 0) {
				++currWarriors;
				return true;
			}
			else if (settings.values.gameplay_RemoveOvercapedUnits && maxWarriors < currWarriors && game.globalGameInfo.tick % ticksPerIncome == 0) {
				--currWarriors;
				return true;
			}
			return false;
		}

		public ushort GetAtkWarriors() {
			return (ushort)(currWarriors * sendPersent);
		}

		public ushort GetDefWarriors() {
			return (ushort)(currWarriors * defPersent);
		}

		public BasicUnit SendUnit(BasicSity to) {
			ushort sendWarriors = GetAtkWarriors();
			currWarriors -= sendWarriors;
			if (currWarriors < 0)
				currWarriors = 0;

			bool b;
			GetShortestPath(to, out b);
			//System.Windows.MessageBox.Show("ISDIR" + b.ToString());

			BasicUnit unit = CreateLinkedUnit(sendWarriors, to);

            return unit;
		}

		bool BuildPathWithoutEnemySitiesPath(BasicSity to) {
			bool rez;
			PathFinderCell[,] finder = new PathFinderCell[gameMap.Map.Count, gameMap.Map[0].Count];
			int fromX = 0, fromY = 0, toX = 0, toY = 0;

			for (int i = 0; i < finder.GetLength(0); ++i) {
				for (int j = 0; j < finder.GetLength(1); ++j) {
					finder[i, j] = new PathFinderCell(gameMap.Map[i][j]);
					if (gameMap.Map[i][j].Sity == this) {
						fromX = j; fromY = i;
					}
					else if (gameMap.Map[i][j].Sity == to) {
						toX = j; toY = i;
					}
				}
			}

			Rec(fromX, fromY, 0);

			List<KeyValuePair<int, int>> reversedPath = new List<KeyValuePair<int, int>>();
			UnRec(toX, toY, finder[toY, toX].num);
			reversedPath.Reverse();

			if (reversedPath.Count != 0) {
				pathToSities.Add(to, reversedPath);
				rez = true;
			}
			else {
				BuildPath(to);
				rez = false;
			}

			void Rec(int x, int y, int value) {
				if (finder[y, x].num != -1 && finder[y, x].num < value)
					return;

				finder[y, x].num = value++;

				if (gameMap.Map[y][x].Sity != null && gameMap.Map[y][x].Sity.playerId != this.playerId)
					return;

				if (finder[y, x].IsOpenBottom)
					Rec(x, y + 1, value);
				if (finder[y, x].IsOpenRight)
					Rec(x + 1, y, value);
				if (finder[y, x].IsOpenTop)
					Rec(x, y - 1, value);
				if (finder[y, x].IsOpenLeft)
					Rec(x - 1, y, value);
			}

			bool UnRec(int x, int y, int prevValue) {
				if (prevValue == finder[y, x].num && finder[y, x].num != -1) {
					bool prev = false;
					reversedPath.Add(new KeyValuePair<int, int>(x, y));
					if (finder[y, x].IsOpenBottom)
						prev = UnRec(x, y + 1, prevValue - 1);
					if (finder[y, x].IsOpenTop && !prev)
						prev = UnRec(x, y - 1, prevValue - 1);
					if (finder[y, x].IsOpenLeft && !prev)
						prev = UnRec(x - 1, y, prevValue - 1);
					if (finder[y, x].IsOpenRight && !prev)
						prev = UnRec(x + 1, y, prevValue - 1);
					return true;
				}
				return false;
			}

			return rez;
		}

		void BuildPath(BasicSity to) {
			PathFinderCell[,] finder = new PathFinderCell[gameMap.Map.Count, gameMap.Map[0].Count];
			int fromX = 0, fromY = 0, toX = 0, toY = 0;

			for (int i = 0; i < finder.GetLength(0); ++i) {
				for (int j = 0; j < finder.GetLength(1); ++j) {
					finder[i, j] = new PathFinderCell(gameMap.Map[i][j]);
					if (gameMap.Map[i][j].Sity == this) {
						fromX = j; fromY = i;
					}
					else if (gameMap.Map[i][j].Sity == to) {
						toX = j; toY = i;
					}
				}
			}

			//bool findSity = false;
			Rec(fromX, fromY, 0);

			List<KeyValuePair<int, int>> reversedPath = new List<KeyValuePair<int, int>>();
			UnRec(toX, toY, finder[toY, toX].num);
			reversedPath.Reverse();
			pathToSities.Add(to, reversedPath);

			//string str = "";
			//for (int i = 0; i < finder.GetLength(0); ++i) {
			//	for (int j = 0; j < finder.GetLength(1); ++j)
			//		str += finder[i, j].num.ToString() + " ";
			//	str += "\n";
			//}
			//System.Windows.MessageBox.Show(str);

			//str = "";
			//for (int i = 0; i < reversedPath.Count; ++i)
			//	str += reversedPath[i].ToString() + "\n";
			//System.Windows.MessageBox.Show(str);

			void Rec(int x, int y, int value) {
				//	if (findSity)
				//	return;
				//if (x == toX && y == toY)
				//	findSity = true;


				if (finder[y, x].num != -1 && finder[y, x].num < value)
					return;

				finder[y, x].num = value++;

				if (finder[y, x].IsOpenBottom)
					Rec(x, y + 1, value);
				if (finder[y, x].IsOpenRight)
					Rec(x + 1, y, value);
				if (finder[y, x].IsOpenTop)
					Rec(x, y - 1, value);
				if (finder[y, x].IsOpenLeft)
					Rec(x - 1, y, value);
			}

			bool UnRec(int x, int y, int prevValue) {
				if (prevValue == finder[y, x].num && finder[y, x].num != -1) {
					bool prev = false;
					reversedPath.Add(new KeyValuePair<int, int>(x, y));
					if (finder[y, x].IsOpenBottom)
						prev = UnRec(x, y + 1, prevValue - 1);
					if (finder[y, x].IsOpenTop && !prev)
						prev = UnRec(x, y - 1, prevValue - 1);
					if (finder[y, x].IsOpenLeft && !prev)
						prev = UnRec(x - 1, y, prevValue - 1);
					if (finder[y, x].IsOpenRight && !prev)
						prev = UnRec(x + 1, y, prevValue - 1);
					return true;
				}
				return false;
			}
		}

		public int GetShortestPath(BasicSity to, out bool isDirectly) {
			pathToSities.Remove(to);
			isDirectly = BuildPathWithoutEnemySitiesPath(to);
			return pathToSities[to].Count - 1;
		}

		protected virtual BasicUnit CreateLinkedUnit(ushort sendWarriors, BasicSity to){
            return new BasicUnit(sendWarriors, this.playerId, pathToSities[to], to);
        }

		public void GetUnits(BasicUnit unit) {
			if(playerId == unit.playerId) {
				this.currWarriors += unit.warriorsCnt;
				if (!settings.values.gameplay_SaveWarriorsOverCap && currWarriors > maxWarriors)
					currWarriors = maxWarriors;
			}
			else {
				ushort defWarriors = currWarriors;
				unit.warriorsCnt = (ushort)Math.Round((2 - this.defPersent) * unit.warriorsCnt);

				if (defWarriors > unit.warriorsCnt) {
					defWarriors -= unit.warriorsCnt;
					if(currWarriors > defWarriors)
						currWarriors = defWarriors;
				}
				else if (!settings.values.gameplay_EqualsMeansCapture && defWarriors == unit.warriorsCnt) {
					currWarriors = 0;
				}
				else {
					currWarriors = (ushort)(unit.warriorsCnt - defWarriors);
					playerId = unit.playerId;
					this.shape.Children.Clear();
					this.FillShape();
				}

			}

			gameMap.Units.Remove(unit);
		}

		class PathFinderCell {
			public bool IsOpenBottom = false, IsOpenLeft = false, IsOpenRight = false, IsOpenTop = false;
			public int num = -1;

			public PathFinderCell(game.map.GameCell cell) {
				IsOpenBottom = cell.IsOpenBottom;
				IsOpenLeft = cell.IsOpenLeft;
				IsOpenRight = cell.IsOpenRight;
				IsOpenTop = cell.IsOpenTop;
			}
		}
	}
}
