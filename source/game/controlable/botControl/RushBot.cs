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
		List<city.BasicCity> botSities = new List<city.BasicCity>();
		List<BasicCity> overcapedBotSities = new List<BasicCity>();

		List<BasicCity> botSitiesUnderAttack = new List<BasicCity>();
		List<List<BasicUnit>> botSitiesUnderAttackUnits = new List<List<BasicUnit>>();

		//With enemy cities
		List<city.BasicCity> rushingSities = new List<city.BasicCity>();
		List<city.BasicCity> canAttackDirectly = new List<city.BasicCity>();

		//All cities on map
		protected List<game.city.BasicCity> sities;
		//All existing units
		protected List<game.unit.BasicUnit> units;


		bool isRushing = false;
		byte rushWaveRemains;
		BasicCity rushSity;

		public bool protectCities;
		public byte protectCities_MinimumUnitsLeft;

		public bool dropOvercapacityUnit;
		public byte dropOvercapacityUnit_Nearby;

		public bool moveUnitsToWeakSities;

		public List<KeyValuePair<byte, byte>> rushChances; 
		public byte rushWithMinimumMoreUnits;


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public RushBot(game.map.GameMap Map,
			byte botId
			) : base(Map, botId) {
			sities = Map.Cities;
			units = Map.Units;
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (GlobalGameInfo.tick > ignoreFirstNTicks &&
				GlobalGameInfo.tick % tickReact == 0) {

				RecalcBotSities();
				RecalcRushingSities();
				RecalcCanAttackDirectly();

				if (!isRushing)
					CalculateWhoNeedToBeRushed();
				if (isRushing)
					RushFromAllSities();

				RecalcOvercapedBotSities();
				RecalcBotSitiesUnderAttack();

				if (protectCities)
					ProtectSities();

				if (dropOvercapacityUnit)
					DropOvercapacityUnits();

				if (moveUnitsToWeakSities)
					MoveUnitsToWeakSity();

				return true;
			}

			return false;
		}

		//---------------------------------------------- Methods - fillData ----------------------------------------------

		void RecalcBotSities() {
			botSities.Clear();
			foreach (var sity in sities) {
				if (sity.PlayerId == PlayerId)
					botSities.Add(sity);
			}
		}

		void RecalcRushingSities() {
			rushingSities.Clear();
			foreach (var unit in units) {
				if (unit.PlayerId == PlayerId && !rushingSities.Contains(unit.destination))
					rushingSities.Add(unit.destination);
			}
		}

		void RecalcCanAttackDirectly() {
			canAttackDirectly.Clear();
			foreach (var sity in sities) {
				if (!botSities.Contains(sity)) {
					bool directly = false;
					foreach (var bs in botSities) {
						bs.BuildOptimalPath(sity, out bool tmp);
						if (tmp) {
							directly = true;
							break;
						}
					}
					if (directly)
						canAttackDirectly.Add(sity);
				}
			}
		}

		void RecalcOvercapedBotSities() {
			overcapedBotSities.Clear();
			foreach (var bs in botSities) {
				int currUnits = bs.currWarriors + dropOvercapacityUnit_Nearby;
				foreach (var unit in units) 
					if (unit.PlayerId == this.PlayerId && unit.destination == bs && unit.TicksLeftToDestination() <= this.tickReact)
						currUnits += unit.warriorsCnt;
				if (currUnits >= bs.maxWarriors)
					overcapedBotSities.Add(bs);
			}
		}

		void RecalcBotSitiesUnderAttack() {
			botSitiesUnderAttack.Clear();
			botSitiesUnderAttackUnits.Clear();
			foreach (var bs in botSities) {
				bool isUnderAttack = false;
				foreach (var unit in units) {
					if (unit.PlayerId != this.PlayerId && unit.destination == bs) {
						isUnderAttack = true;
						break;
					}
				}

				if (isUnderAttack) {
					botSitiesUnderAttackUnits.Add(new List<BasicUnit>());
					botSitiesUnderAttack.Add(bs);
					foreach (var unit in units) {
						if (unit.destination == bs) {
							botSitiesUnderAttackUnits[botSitiesUnderAttackUnits.Count - 1].Add(unit);
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

				foreach (var sity in canAttackDirectly) {
					uint potentialArmy = CalcPotentialArmy(i, sity.defPersent);
					if (!rushingSities.Contains(sity) &&
						 GetEnemyArmy(sity) < potentialArmy
					) {
						bool isInPrev = false;
						foreach (var rush in potentialRushes) {
							if (rush.Contains(sity)) {
								isInPrev = true;
								break;
							}
						}
						if (!isInPrev)
							potentialRushes[i - 1].Add(sity);
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
				rushSity = potentialRush[Rand.Next(0, potentialRush.Count)];
				isRushing = true;
				rushWaveRemains = (byte)(potentialRushPos + 1);
			}

		}

		void RushFromAllSities() {
			if (rushSity.PlayerId == this.PlayerId)
				isRushing = false;

			if (rushWaveRemains-- == 0)
				isRushing = false;
			else
				foreach (var sity in botSities)
					map.SendWarriors(sity, rushSity);
		}

		void ProtectSities() {
			//for (int i = 0; i < botSitiesUnderAttack.Count; ++i) {
			//	var sity = botSitiesUnderAttack[i];
			//	var units = botSitiesUnderAttackUnits[i];

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
			//foreach (var i in overcapedBotSities) {
			//	map.SendWarriors(i, this.sities[values.rnd.Next(0, sities.Count)]);
			//}
		}

		void MoveUnitsToWeakSity() {

		}

		//---------------------------------------------- Methods - Support  ----------------------------------------------

		uint CalcPotentialArmy(byte rushesCntBase, double enemyDefMult) {
			uint potentialArmy = 0;
			foreach (var sity in botSities) {
				ushort sendWarriors = 0, currWarriors = sity.currWarriors, rushesCnt = rushesCntBase;
				while (rushesCnt-- != 0) {
					sendWarriors += (ushort)Math.Round(currWarriors * sity.sendPersent * sity.atkPersent);
					currWarriors -= (ushort)(currWarriors * sity.sendPersent);
				}
				potentialArmy += (uint)Math.Round((2 - enemyDefMult) * sendWarriors);
			}
			return potentialArmy;
		}

		int GetAvgDistance(BasicCity sity) {
			double avg = 0;

			foreach (var bs in botSities) {
				avg += bs.BuildOptimalPath(sity, out bool b).Count;
			}
			avg /= botSities.Count();

			return (int)Math.Round(avg);
		}

		int GetAvgSpeed() {
			double avg = 0;

			foreach (var bs in botSities) {
				avg += bs.CreateLinkedUnit(0, null).tickPerTurn;
			}
			avg /= botSities.Count();

			return (int)Math.Round(avg);
		}

		double GetEnemyArmy(BasicCity sityTo) {
			if (sityTo.PlayerId == 0)
				return sityTo.currWarriors +
					rushWithMinimumMoreUnits;
			else
				return sityTo.currWarriors +
					rushWithMinimumMoreUnits +
					GetAvgSpeed() * GetAvgDistance(sityTo) / sityTo.ticksPerIncome;
		}

		public override SettinsSetter CreateLinkedSetting() {
			return new settings.controlable.botControl.RushBotSettings();
		}

	}
}
