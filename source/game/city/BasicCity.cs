using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using taw.game.basicInterfaces;
using taw.game.unit;
using taw.game.settings;
using taw.game.city.events;


namespace taw.game.city {
	public partial class BasicCity : ITickable, IWithPlayerId, ISettingable, IOutputable, IInputable {
		//---------------------------------------------- Fields ----------------------------------------------
		public static taw.game.map.GameMap gameMap;
		private int x, y;

		//Load from settings
		public ushort currWarriors, maxWarriors;
		public double sendPersent, atkPersent, defPersent;
		public ushort ticksPerIncome;

		public bool saveOvercapedUnits;
		public bool removeOvercapedUnits;

		public bool equalsMeanCaptured;
		public bool equalsMeanCapturedForNeutral; 

		//---------------------------------------------- Properties ----------------------------------------------
		public byte PlayerId { get; set; }
		public int X { get => x; set => x = value; }
		public int Y { get => y; set => y = value; }
		public object OutputInfo { get; set; }
		public object InputInfo { get; set; }

		//---------------------------------------------- Events ----------------------------------------------
		public delegate void CityBasicDelegate(BasicCityEvent cityEvent);
		public delegate void CityUnitsDelegate(CityUnitsEvent cityEvent);
		public delegate void CityIncomeDelegate(CityIncomeEvent cityEvent);
		public delegate void CityCaptureDelegate(CityCaptureEvent cityEvent);

		BasicCityEvent basicCityEvent;

		public event CityCaptureDelegate Captured;

		public event CityIncomeDelegate UnitIncome;

		public event CityUnitsDelegate UnitSend;
		public event CityUnitsDelegate UnitGet;

		public event CityBasicDelegate Tick;
		public event CityBasicDelegate FirstTick;

		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicCity() {
			this.SetSettings(this.CreateLinkedSetting());

			basicCityEvent = new BasicCityEvent(this);
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public bool TickReact() {
			if(GlobalGameInfo.tick == 1 && FirstTick != null) 
				FirstTick(basicCityEvent);

			Tick?.Invoke(basicCityEvent);

			if (game.GlobalGameInfo.tick % ticksPerIncome == 0) {
				if (PlayerId != 0 && maxWarriors > currWarriors) {
					++currWarriors;
					UnitIncome?.Invoke(new CityIncomeEvent(basicCityEvent, true));
				}
				else if (removeOvercapedUnits && maxWarriors < currWarriors) {
					--currWarriors;
					UnitIncome?.Invoke(new CityIncomeEvent(basicCityEvent, false));
				}


				return true;
			}

			return false;
		}

		//Повертає кількість воїнів, які вийдуть при наступній атаці
		public ushort GetAtkWarriors() {
			return (ushort)Math.Round(currWarriors * sendPersent * atkPersent);
		}

		//Повертає кількість воїнів, яких вистачить щоб вбити всіх в цьому місті (до 0)
		public ushort GetDefWarriors() {
			return (ushort)Math.Round(currWarriors * defPersent);
		}

		//Створює юніта і задає йому шлях для руху
		public BasicUnit SendUnit(BasicCity to) {
			ushort sendWarriors = GetAtkWarriors();
			if(sendWarriors == 0) 
				return null;
			
			currWarriors -= sendWarriors;
			if (currWarriors < 0)
				currWarriors = 0;

			BasicUnit unit = CreateLinkedUnit(sendWarriors, to);
			UnitSend?.Invoke(new CityUnitsEvent(basicCityEvent, unit));
			return unit;
		}

		//Опрацьовує юніта, який зайшов у місто
		public void GetUnits(BasicUnit unit) {
			UnitGet?.Invoke(new CityUnitsEvent(basicCityEvent, unit));

			if (PlayerId == unit.PlayerId) {
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
						(PlayerId == 0 && equalsMeanCapturedForNeutral) ||					
						(equalsMeanCaptured)
					) {
					CityCaptureEvent captureCityEvent = new CityCaptureEvent(basicCityEvent, PlayerId, unit.PlayerId);
					currWarriors = (ushort)(unit.warriorsCnt - currWarriors);
					PlayerId = unit.PlayerId;
					Captured?.Invoke(captureCityEvent);
				}
			}

			gameMap.Units.Remove(unit);
		}

		//Повертає шлях до міста. є 2 типи шляхів
		//1) Шлях в обхід всіх ворожих міст
		//2) Шлях напролом. Створюється якщо не існує шляху1.
		public List<KeyValuePair<int, int>> BuildOptimalPath(BasicCity to, out bool isDirectly) {
			if (to == null) {
				isDirectly = false;
				return null;
			}

			int minFindValue = int.MaxValue;
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

			var recList = new Queue<RecInfo>();
			recList.Enqueue(new RecInfo() { x = fromX, y = fromY, value = 0 });
			while (recList.Count != 0) {
				if (rez == true)
					RecAvoidEnemyCities(recList.Dequeue());
				else
					RecThroughEnemyCities(recList.Dequeue());
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
			//Пошук шляху в обхід ворога
			void RecAvoidEnemyCities(RecInfo info) {
				int x = info.x, y = info.y;

				if ((finder[y, x].num != -1 && finder[y, x].num < info.value) ||
					finder[y, x].num > minFindValue)
					return;

				if (x == toX && y == toY && finder[y, x].num < minFindValue)
					minFindValue = finder[y, x].num;

				if (gameMap.Map[y][x].Sity != null && gameMap.Map[y][x].Sity.PlayerId != this.PlayerId && x != toX && y != toY)
					return;

				finder[y, x].num = info.value++;

				AddNearbyToRecList(x, y, info.value);
			}

			//Пошук шляху напролом
			void RecThroughEnemyCities(RecInfo info) {
				int x = info.x, y = info.y;

				if ((finder[y, x].num != -1 && finder[y, x].num < info.value) ||
					finder[y, x].num > minFindValue)
					return;

				if (x == toX && y == toY && finder[y, x].num < minFindValue)
					minFindValue = finder[y, x].num;

				finder[y, x].num = info.value++;

				AddNearbyToRecList(x, y, info.value);
			}

			//Дадає клетки в ліст для наступного пошуку
			void AddNearbyToRecList(int x, int y, int val) {
				if (finder[y, x].IsOpenBottom)
					recList.Enqueue(new RecInfo() { x = x, y = y + 1, value = val });
				if (finder[y, x].IsOpenRight)
					recList.Enqueue(new RecInfo() { x = x + 1, y = y, value = val });
				if (finder[y, x].IsOpenTop)
					recList.Enqueue(new RecInfo() { x = x, y = y - 1, value = val });
				if (finder[y, x].IsOpenLeft)
					recList.Enqueue(new RecInfo() { x = x - 1, y = y, value = val });
			}

			//Будує сам шлях від міста до міста
			bool BuildBackPath(int x, int y, int prevValue) {
				if (prevValue == finder[y, x].num && finder[y, x].num != -1) {
					reversedPath.Add(new KeyValuePair<int, int>(x, y));

					List<KeyValuePair<int, int>> nextPathElement = new List<KeyValuePair<int, int>>(4);
					if (finder[y, x].IsOpenBottom)
						nextPathElement.Add(new KeyValuePair<int, int>(x, y + 1));
					if (finder[y, x].IsOpenTop)
						nextPathElement.Add(new KeyValuePair<int, int>(x, y - 1));
					if (finder[y, x].IsOpenLeft)
						nextPathElement.Add(new KeyValuePair<int, int>(x - 1, y));
					if (finder[y, x].IsOpenRight)
						nextPathElement.Add(new KeyValuePair<int, int>(x + 1, y));

					if (nextPathElement.Count > 1) {
						int timesToChange = Rand.Next(nextPathElement.Count + 1, (nextPathElement.Count + 1) * 2);
						while (timesToChange-- != 0) {
							int pos1, pos2;
							do {
								pos1 = Rand.Next(0, nextPathElement.Count);
								pos2 = Rand.Next(0, nextPathElement.Count);
							} while (pos1 == pos2);
							KeyValuePair<int, int> tmp = nextPathElement[pos1];
							nextPathElement[pos1] = nextPathElement[pos2];
							nextPathElement[pos2] = tmp;
						};
					}

					while (nextPathElement.Count != 0) {
						if (BuildBackPath(nextPathElement[0].Key, nextPathElement[0].Value, prevValue - 1))
							break;
						nextPathElement.RemoveAt(0);
					}

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

		public void SetSettings(SettinsSetter settinsSetter) {
			settinsSetter.SetSettings(this);
		}

		public virtual SettinsSetter CreateLinkedSetting() {
			return new settings.city.BasicCitySettings();
		}

		//Створює юнита, якого посилатиме це місто
		public virtual BasicUnit CreateLinkedUnit(ushort sendWarriors, BasicCity to) {
			return new BasicUnit(sendWarriors, this.PlayerId, BuildOptimalPath(to, out bool b), to);
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
