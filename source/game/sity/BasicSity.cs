using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.unit;
using TownsAndWarriors.game.settings;

namespace TownsAndWarriors.game.sity {
	public partial class BasicSity : GameCellDrawableObj, tickable, withPlayerId, Settingable {
		//---------------------------------------------- Fields ----------------------------------------------
		public static TownsAndWarriors.game.map.GameMap gameMap;

		//Load from settings
		public ushort currWarriors, maxWarriors;
		public double sendPersent, atkPersent, defPersent;
		public ushort ticksPerIncome;

		public bool saveOvercapedUnits;
		public bool removeOvercapedUnits;

		public bool equalsMeanCaptured;
		public bool equalsMeanCapturedForNeutral; 

		//---------------------------------------------- Properties ----------------------------------------------
		public byte playerId { get; set; }


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicSity() {
			this.GetSettings(this.CreateLinkedCitySettings());
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public bool TickReact() {
			if(game.globalGameInfo.tick % ticksPerIncome == 0) {
				if (playerId != 0 && maxWarriors > currWarriors)
					++currWarriors;
				else if (removeOvercapedUnits && maxWarriors < currWarriors)
					--currWarriors;

				return true;
			}

			return false;
		}

		public ushort GetAtkWarriors() {
			return (ushort)Math.Round(currWarriors * sendPersent * atkPersent);
		}

		public ushort GetDefWarriors() {
			return (ushort)Math.Round(currWarriors * defPersent);
		}

		public BasicUnit SendUnit(BasicSity to) {
			ushort sendWarriors = GetAtkWarriors();
			if(sendWarriors == 0) 
				return null;
			
			currWarriors -= sendWarriors;
			if (currWarriors < 0)
				currWarriors = 0;

			BasicUnit unit = CreateLinkedUnit(sendWarriors, to);

			return unit;
		}

		public void GetUnits(BasicUnit unit) {
			if (playerId == unit.playerId) {
				this.currWarriors += unit.warriorsCnt;
				if (!saveOvercapedUnits && currWarriors > maxWarriors)
					currWarriors = maxWarriors;
			}
			else {
				unit.warriorsCnt = (ushort)Math.Round((2 - this.defPersent) * unit.warriorsCnt);

				if (currWarriors > unit.warriorsCnt) {
					currWarriors -= unit.warriorsCnt;
				}
				else if (currWarriors < unit.warriorsCnt ||
						(playerId == 0 && equalsMeanCapturedForNeutral) ||					
						(equalsMeanCaptured)
					) {
					currWarriors = (ushort)(unit.warriorsCnt - currWarriors);
					playerId = unit.playerId;
				}
			}

			gameMap.Units.Remove(unit);
		}

		public void GetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public List<KeyValuePair<int, int>> BuildOptimalPath(BasicSity to, out bool isDirectly) {
			bool rez = true;
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

			UNOPTIMAL_PATH_FINDER:

			var recList = new List<RecInfo>() { new RecInfo() { x = fromX, y = fromY, value = 0 } };
			while (recList.Count != 0) {
				if (rez == true)
					RecAvoidEnemyCities(recList[0]);
				else
					RecThroughEnemyCities(recList[0]);
				recList.RemoveAt(0);
			}

			List<KeyValuePair<int, int>> reversedPath = new List<KeyValuePair<int, int>>();
			BuildBackPath(toX, toY, finder[toY, toX].num);
			reversedPath.Reverse();

			if (reversedPath.Count == 0 && rez) {
				rez = false;
				recList.Clear();
				for (int i = 0; i < finder.GetLength(0); ++i)
					for (int j = 0; j < finder.GetLength(1); ++j)
						finder[i, j].num = -1;
				goto UNOPTIMAL_PATH_FINDER;
			}

			//------------------------------- Inner methods ---------------------------------------
			void RecAvoidEnemyCities(RecInfo info) {
				int x = info.x, y = info.y;

				if (finder[y, x].num != -1 && finder[y, x].num < info.value)
					return;

				finder[y, x].num = info.value++;

				if (gameMap.Map[y][x].Sity != null && gameMap.Map[y][x].Sity.playerId != this.playerId)
					return;

				AddNearbyToRecList(x, y, info.value);
			}

			void RecThroughEnemyCities(RecInfo info) {
				int x = info.x, y = info.y;

				if (finder[y, x].num != -1 && finder[y, x].num < info.value)
					return;

				finder[y, x].num = info.value++;

				AddNearbyToRecList(x, y, info.value);
			}

			void AddNearbyToRecList(int x, int y, int val) {
				if (finder[y, x].IsOpenBottom)
					recList.Add(new RecInfo() { x = x, y = y + 1, value = val  });
				if (finder[y, x].IsOpenRight)
					recList.Add(new RecInfo() { x = x + 1, y = y, value = val });
				if (finder[y, x].IsOpenTop)
					recList.Add(new RecInfo() { x = x, y = y - 1, value = val });
				if (finder[y, x].IsOpenLeft)
					recList.Add(new RecInfo() { x = x - 1, y = y, value = val });
			}

			bool BuildBackPath(int x, int y, int prevValue) {
				if (prevValue == finder[y, x].num && finder[y, x].num != -1) {
					bool prev = false;
					reversedPath.Add(new KeyValuePair<int, int>(x, y));
					if (finder[y, x].IsOpenBottom)
						prev = BuildBackPath(x, y + 1, prevValue - 1);
					if (finder[y, x].IsOpenTop && !prev)
						prev = BuildBackPath(x, y - 1, prevValue - 1);
					if (finder[y, x].IsOpenLeft && !prev)
						prev = BuildBackPath(x - 1, y, prevValue - 1);
					if (finder[y, x].IsOpenRight && !prev)
						prev = BuildBackPath(x + 1, y, prevValue - 1);
					return true;
				}
				return false;
			}
			//------------------------------- END of Inner methods ---------------------------------------

			isDirectly = rez;
			if (reversedPath.Count != 0)
				return reversedPath;
			return null;
		}

		public virtual settings.city.BasicCitySettings CreateLinkedCitySettings() {
			return new settings.city.BasicCitySettings();
		}

		protected virtual BasicUnit CreateLinkedUnit(ushort sendWarriors, BasicSity to) {
			bool b;
			return new BasicUnit(sendWarriors, this.playerId, BuildOptimalPath(to, out b), to);
		}

		//////////////////////////////////////////////////////////////////////////

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

		class RecInfo {
			public int x, y, value;
		}
	}
}
