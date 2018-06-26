using System;
using System.Collections.Generic;
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
		public static map.GameMap gameMap;

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
		public int X { get; set; }
		public int Y { get; set; }
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
		public ushort GetAtkWarriors() => (ushort)Math.Round(GetAtkWarriorsWithoutAtk() * atkPersent);
		ushort GetAtkWarriorsWithoutAtk() => (ushort)Math.Round(currWarriors * sendPersent);

		//Повертає кількість воїнів, яких вистачить щоб вбити всіх в цьому місті (до 0)
		public ushort GetDefWarriors() => (ushort)Math.Round(currWarriors * defPersent);

		//Створює юніта і задає йому шлях для руху
		public BasicUnit SendUnit(BasicCity to) {
			ushort sendWarriors = GetAtkWarriorsWithoutAtk();
			if(sendWarriors == 0) 
				return null;
			
			currWarriors -= sendWarriors;
			sendWarriors = (ushort)Math.Round(sendWarriors * atkPersent);

			BasicUnit unit = CreateLinkedUnit(sendWarriors, to);
			UnitSend?.Invoke(new CityUnitsEvent(basicCityEvent, unit));
			return unit;
		}

		//Опрацьовує юніта, який зайшов у місто
		public void GetUnits(BasicUnit unit) {
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
				else if (
						(currWarriors < unit.warriorsCnt) ||
						(
							currWarriors == unit.warriorsCnt &&
							((PlayerId == 0 && equalsMeanCapturedForNeutral) ||
							(equalsMeanCaptured))
						)
					) {
					CityCaptureEvent captureCityEvent = new CityCaptureEvent(basicCityEvent, PlayerId, unit.PlayerId);
					currWarriors = (ushort)(unit.warriorsCnt - currWarriors);
					PlayerId = unit.PlayerId;
					Captured?.Invoke(captureCityEvent);
				}
				else if(currWarriors == unit.warriorsCnt) {
					currWarriors = 0;
				}
			}

			gameMap.Units.Remove(unit);
			UnitGet?.Invoke(new CityUnitsEvent(basicCityEvent, unit));
		}

		//Повертає шлях до міста. є 2 типи шляхів
		//1) Шлях в обхід всіх ворожих міст
		//2) Шлях напролом. Створюється якщо не існує шляху1.
		public List<KeyValuePair<int, int>> BuildOptimalPath(BasicCity to, out bool isDirectly) {
			if (to == null) {
				isDirectly = false;
				return null;
			}

			isDirectly = true;
			int minFindValue = int.MaxValue;
			PathFinderCell[,] finder = new PathFinderCell[gameMap.Map.Count, gameMap.Map[0].Count];
			List<KeyValuePair<int, int>> reversedPath = new List<KeyValuePair<int, int>>();

			for (int i = 0; i < finder.GetLength(0); ++i)
				for (int j = 0; j < finder.GetLength(1); ++j)
					finder[i, j] = new PathFinderCell(gameMap.Map[i][j]);

			UNOPTIMAL_PATH_FINDER:

			var recQueue = new Queue<RecInfo>();
			recQueue.Enqueue(new RecInfo() { x = X, y = Y, value = 0 });
			while (recQueue.Count != 0) {
				if (isDirectly)
					RecAvoidEnemyCities(recQueue.Dequeue());
				else
					RecThroughEnemyCities(recQueue.Dequeue());
			}

			BuildBackPath(to.X, to.Y, finder[to.Y, to.X].num);
			reversedPath.Reverse();

			if (reversedPath.Count == 0 && isDirectly) {
				isDirectly = false;
				recQueue.Clear();
				for (int i = 0; i < finder.GetLength(0); ++i)
					for (int j = 0; j < finder.GetLength(1); ++j)
						finder[i, j].num = -1;
				goto UNOPTIMAL_PATH_FINDER;
			}

			if (reversedPath.Count != 0 && !isDirectly) 
				SetNewDestination();
			

			//------------------------------- Inner methods ---------------------------------------
			//Пошук шляху в обхід ворога
			void RecAvoidEnemyCities(RecInfo info) {
				int x = info.x, y = info.y;
				if ((finder[y, x].num != -1 && finder[y, x].num <= info.value) ||
					(info.value >= minFindValue) ||
					(
						gameMap.Map[y][x].Sity != null && gameMap.Map[y][x].Sity.PlayerId != this.PlayerId && 
						(x != to.X || y != to.Y)
					)
				)
					return;

				if (x == to.X && y == to.Y)
					minFindValue = info.value;

				finder[y, x].num = info.value++;

				if(x != to.X || y != to.Y)
					AddNearbyToRecList(x, y, info.value);
			}

			//Пошук шляху напролом
			void RecThroughEnemyCities(RecInfo info) {
				if ((finder[info.y, info.x].num != -1 && finder[info.y, info.x].num <= info.value) ||
					(info.value >= minFindValue)
				)
					return;

				if (info.x == to.X && info.y == to.Y)
					minFindValue = info.value;

				finder[info.y, info.x].num = info.value++;

				if (info.x != to.X || info.y != to.Y)
					AddNearbyToRecList(info.x, info.y, info.value);
			}

			//Дадає клетки в ліст для наступного пошуку
			void AddNearbyToRecList(int x, int y, int val) {
				if (finder[y, x].IsOpenBottom)
					recQueue.Enqueue(new RecInfo() { x = x, y = y + 1, value = val });
				if (finder[y, x].IsOpenRight)
					recQueue.Enqueue(new RecInfo() { x = x + 1, y = y, value = val });
				if (finder[y, x].IsOpenTop)
					recQueue.Enqueue(new RecInfo() { x = x, y = y - 1, value = val });
				if (finder[y, x].IsOpenLeft)
					recQueue.Enqueue(new RecInfo() { x = x - 1, y = y, value = val });
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

					while (nextPathElement.Count != 0) {
						int rPos = Rand.Next(0, nextPathElement.Count);
						if (BuildBackPath(nextPathElement[rPos].Key, nextPathElement[rPos].Value, prevValue - 1))
							break;
						nextPathElement.RemoveAt(rPos);
					}

					return true;
				}
				return false;
			}

			//Якщо послали в місто куди нема прямого шляху, то встановить новий Destination. Гравцю шо з цим методом, шо без нього, все одно нічого не помітно. 
			//Але крепко воно діє на бота. Бот бачить що його рашать, і пробує щось робити, а ворог навіть не дійшов)
			void SetNewDestination() {
				for(int i = 0; i < reversedPath.Count; ++i) {
					if(gameMap.Map[reversedPath[i].Value][reversedPath[i].Key].Sity != null &&
						gameMap.Map[reversedPath[i].Value][reversedPath[i].Key].Sity.PlayerId != this.PlayerId) {
						reversedPath.RemoveRange(i + 1, reversedPath.Count - i - 1);
						break;
					}

				}
			}
			//------------------------------- END of Inner methods ---------------------------------------

			if (reversedPath.Count != 0)
				return reversedPath;
			return null;
		}

		public void SetSettings(SettinsSetter settinsSetter) => settinsSetter.SetSettings(this);

		public virtual SettinsSetter CreateLinkedSetting() => new settings.city.BasicCitySettings();

		//Створює юнита, якого посилатиме це місто
		public virtual BasicUnit CreateLinkedUnit(ushort sendWarriors, BasicCity to) =>
			new BasicUnit(sendWarriors, this.PlayerId, BuildOptimalPath(to, out bool b), to);

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
