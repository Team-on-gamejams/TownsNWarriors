using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.settings;
using taw.game.city;
using taw.game.unit;

namespace taw.game.controlable.botControl {
	public class RushBot : BasicBot {
		//---------------------------------------------- Fields ----------------------------------------------
		//with bot cities
		List<city.BasicCity> botCities = new List<city.BasicCity>();
		List<BasicCity> overcapedBotCities = new List<BasicCity>();

		List<BasicCity> botCitiesUnderAttack = new List<BasicCity>();
		List<List<BasicUnit>> botCitiesUnderAttackUnits = new List<List<BasicUnit>>();

		//With enemy cities
		List<city.BasicCity> rushingCities = new List<city.BasicCity>();
		List<city.BasicCity> canAttackDirectly = new List<city.BasicCity>();

		//All cities on map
		protected List<game.city.BasicCity> cities;
		//All existing units
		protected List<game.unit.BasicUnit> units;


		bool isRushing = false;
		byte rushWaveRemains;
		BasicCity rushCity;

		public bool protectCities;
		public byte protectCities_MinimumUnitsLeft;

		public bool dropOvercapacityUnit;
		public byte dropOvercapacityUnit_Nearby;

		public bool moveUnitsToWeakCities;

		public List<KeyValuePair<byte, byte>> rushChances; 
		public byte rushWithMinimumMoreUnits;


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public RushBot(game.map.GameMap Map,
			byte botId
			) : base(Map, botId) {
			cities = Map.Cities;
			units = Map.Units;
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (GlobalGameInfo.tick > ignoreFirstNTicks &&
				GlobalGameInfo.tick % tickReact == 0 &&
				game.controlable.botControl.support.LogicalPlayersSingletone.ControlInfoForParts[this.PlayerId].Count != 0) {

				RecalcBotCities();
				RecalcRushingCities();
				RecalcCanAttackDirectly();

				if (!isRushing)
					CalculateWhoNeedToBeRushed();
				if (isRushing)
					RushFromAllCities();

				RecalcOvercapedBotCities();
				RecalcBotCitiesUnderAttack();

				if (protectCities)
					ProtectCities();

				if (dropOvercapacityUnit)
					DropOvercapacityUnits();

				if (moveUnitsToWeakCities)
					MoveUnitsToWeakCity();

				return true;
			}

			return false;
		}

		//---------------------------------------------- Methods - fillData ----------------------------------------------

		void RecalcBotCities() {
			botCities.Clear();
			foreach (var city in cities) {
				if (city.PlayerId == PlayerId)
					botCities.Add(city);
			}
		}

		void RecalcRushingCities() {
			rushingCities.Clear();
			foreach (var unit in units) {
				if (unit.PlayerId == PlayerId && !rushingCities.Contains(unit.destination))
					rushingCities.Add(unit.destination);
			}
		}

		void RecalcCanAttackDirectly() {
			canAttackDirectly.Clear();
			foreach (var city in cities) {
				if (!botCities.Contains(city)) {
					bool directly = false;
					foreach (var bs in botCities) {
						bs.BuildOptimalPath(city, out BasicCity realDest);
						if (realDest == city) {
							directly = true;
							break;
						}
					}
					if (directly)
						canAttackDirectly.Add(city);
				}
			}
		}

		void RecalcOvercapedBotCities() {
			overcapedBotCities.Clear();
			foreach (var bs in botCities) {
				int currUnits = bs.currWarriors + dropOvercapacityUnit_Nearby;
				foreach (var unit in units) 
					if (unit.PlayerId == this.PlayerId && unit.destination == bs && unit.TicksLeftToDestination() <= this.tickReact)
						currUnits += unit.warriorsCnt;
				if (currUnits >= bs.maxWarriors)
					overcapedBotCities.Add(bs);
			}
		}

		void RecalcBotCitiesUnderAttack() {
			botCitiesUnderAttack.Clear();
			botCitiesUnderAttackUnits.Clear();
			foreach (var bs in botCities) {
				bool isUnderAttack = false;
				foreach (var unit in units) {
					if (unit.PlayerId != this.PlayerId && unit.destination == bs) {
						isUnderAttack = true;
						break;
					}
				}

				if (isUnderAttack) {
					botCitiesUnderAttackUnits.Add(new List<BasicUnit>());
					botCitiesUnderAttack.Add(bs);
					foreach (var unit in units) {
						if (unit.destination == bs) {
							botCitiesUnderAttackUnits[botCitiesUnderAttackUnits.Count - 1].Add(unit);
						}
					}
				}

			}
		}


		//---------------------------------------------- Methods - behavior ----------------------------------------------

		void CalculateWhoNeedToBeRushed() {
			List<List<BasicCity>> potentialRushes = new List<List<BasicCity>>();
			for (byte i = 1; i <= rushChances.Count; ++i) {
				potentialRushes.Add(new List<BasicCity>());

				foreach (var city in canAttackDirectly) {
					uint potentialArmy = CalcPotentialArmy(i, city.defPersent);
					if (!rushingCities.Contains(city) &&
						 GetEnemyArmy(city) < potentialArmy
					) {
						bool isInPrev = false;
						foreach (var rush in potentialRushes) {
							if (rush.Contains(city)) {
								isInPrev = true;
								break;
							}
						}
						if (!isInPrev)
							potentialRushes[i - 1].Add(city);
					}
				}

			}


			List<KeyValuePair<byte, byte>> rushChance = new List<KeyValuePair<byte, byte>>(potentialRushes.Count);

			int tmp = 0;
			while (tmp != potentialRushes.Count) {
				if (potentialRushes[tmp].Count != 0)
					rushChance.Add(new KeyValuePair<byte, byte>(rushChances[tmp].Key, rushChances[tmp].Value));
				++tmp;
			}

			if (rushChance.Count != 0) {
				int potentialRushPos = 0;

				byte sumPersent = 0;
				foreach (var i in rushChance)
					sumPersent += i.Value;
				if (sumPersent != 100) {
					for (int i = 0; i < rushChance.Count; ++i)
						rushChance[i] = new KeyValuePair<byte, byte>(rushChance[i].Key,
							(byte)Math.Round((double)(rushChance[i].Value) / sumPersent * 100));
				}

				byte randPersent = Rand.NextPersent();
				for (int i = 0; i < rushChance.Count; ++i) {
					if (randPersent <= rushChance[i].Value) {
						potentialRushPos = rushChance[i].Key - 1;
						break;
					}
					else
						randPersent -= rushChance[i].Value;
				}

				var potentialRush = potentialRushes[potentialRushPos];
				rushCity = potentialRush[Rand.Next(0, potentialRush.Count)];
				isRushing = true;
				rushWaveRemains = (byte)(potentialRushPos + 1);
			}

		}

		void RushFromAllCities() {
			if (rushCity.PlayerId == this.PlayerId)
				isRushing = false;

			if (rushWaveRemains-- == 0)
				isRushing = false;
			else
				foreach (var city in botCities)
					city.SendUnit(rushCity);
		}

		void ProtectCities() {
			//for (int i = 0; i < botCitiesUnderAttack.Count; ++i) {
			//	var city = botCitiesUnderAttack[i];
			//	var units = botCitiesUnderAttackUnits[i];

			//	uint attackersCnt = 0;
			//	foreach (var unit in units) {
			//		if (unit.playerId != this.playerId)
			//			attackersCnt += unit.warriorsCnt;
			//		else
			//			attackersCnt -= unit.warriorsCnt;
			//	}

			//	if (attackersCnt >= settings.values.bot_rushBot_Protect_MinimumUnitsLeft) {

			//	}

			//}
		}

		void DropOvercapacityUnits() {
			//foreach (var i in overcapedBotCities) {
			//	map.SendWarriors(i, this.cities[values.rnd.Next(0, cities.Count)]);
			//}
		}

		void MoveUnitsToWeakCity() {

		}

		//---------------------------------------------- Methods - Support  ----------------------------------------------

		uint CalcPotentialArmy(byte rushesCntBase, double enemyDefMult) {
			uint potentialArmy = 0;
			foreach (var city in botCities) {
				ushort sendWarriors = 0, currWarriors = city.currWarriors, rushesCnt = rushesCntBase;
				while (rushesCnt-- != 0) {
					sendWarriors += (ushort)Math.Round(currWarriors * city.sendPersent * city.atkPersent);
					currWarriors -= (ushort)(currWarriors * city.sendPersent);
				}
				potentialArmy += (uint)Math.Round((2 - enemyDefMult) * sendWarriors);
			}
			return potentialArmy;
		}

		int GetAvgDistance(BasicCity city) {
			double avg = 0;

			foreach (var bs in botCities) {
				avg += bs.BuildOptimalPath(city, out BasicCity realDest).Count;
			}
			avg /= botCities.Count();

			return (int)Math.Round(avg);
		}

		int GetAvgSpeed() {
			double avg = 0;

			foreach (var bs in botCities) {
				avg += bs.CreateLinkedUnit(0, null).tickPerTurn;
			}
			avg /= botCities.Count();

			return (int)Math.Round(avg);
		}

		double GetEnemyArmy(BasicCity cityTo) {
			if (cityTo.PlayerId == 0)
				return cityTo.currWarriors +
					rushWithMinimumMoreUnits;
			else
				return cityTo.currWarriors +
					rushWithMinimumMoreUnits +
					GetAvgSpeed() * GetAvgDistance(cityTo) / cityTo.ticksPerIncome;
		}

		public override SettinsSetter CreateLinkedSetting() {
			return new settings.controlable.botControl.RushBotSettings();
		}

	}
}
