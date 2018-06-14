using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.settings;
using TownsAndWarriors.game.sity;
using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.bot {
	public class RushBot : BasicBot {
		//---------------------------------------------- Fields ----------------------------------------------
		//with bot cities
		List<sity.BasicSity> botSities = new List<sity.BasicSity>();
		List<BasicSity> overcapedBotSities = new List<BasicSity>();

		List<BasicSity> botSitiesUnderAttack = new List<BasicSity>();
		List<List<BasicUnit>> botSitiesUnderAttackUnits = new List<List<BasicUnit>>();

		//With enemy cities
		List<sity.BasicSity> rushingSities = new List<sity.BasicSity>();
		List<sity.BasicSity> canAttackDirectly = new List<sity.BasicSity>();


		bool isRushing = false;
		byte rushWaveRemains;
		BasicSity rushSity;
		byte tickReact;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public RushBot(game.map.GameMap Map,
			List<game.sity.BasicSity> Sities,
			List<game.unit.BasicUnit> Units,
			byte botId
			) {
			map = Map;
			sities = Sities;
			units = Units;
			playerId = botId;
			tickReact = values.bot_rushBot_Tick_React;
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (globalGameInfo.tick > settings.values.bot_rushBot_Tick_IgnoreFirstN &&
				globalGameInfo.tick % tickReact == 0) {

				RecalcBotSities();
				RecalcRushingSities();
				RecalcCanAttackDirectly();

				if (!isRushing)
					CalculateWhoNeedToBeRushed();
				if (isRushing)
					RushFromAllSities();

				RecalcOvercapedBotSities();
				RecalcBotSitiesUnderAttack();

				if (values.bot_rushBot_IsProtectSities)
					ProtectSities();

				if (values.bot_rushBot_IsDropOvercapacityUnits)
					DropOvercapacityUnits();

				if (values.bot_rushBot_IsMoveUnitsToWeakSities)
					MoveUnitsToWeakSity();

				return true;
			}

			return false;
		}

		//---------------------------------------------- Methods - fillData ----------------------------------------------

		void RecalcBotSities() {
			botSities.Clear();
			foreach (var sity in sities) {
				if (sity.playerId == playerId)
					botSities.Add(sity);
			}
		}

		void RecalcRushingSities() {
			rushingSities.Clear();
			foreach (var unit in units) {
				if (unit.playerId == playerId && !rushingSities.Contains(unit.destination))
					rushingSities.Add(unit.destination);
			}
		}

		void RecalcCanAttackDirectly() {
			canAttackDirectly.Clear();
			foreach (var sity in sities) {
				if (!botSities.Contains(sity)) {
					bool directly = false;
					foreach (var bs in botSities) {
						bool tmp;
						sity.GetShortestPath(sity, out tmp);
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
				int currUnits = bs.currWarriors + values.bot_rushBot_Overcapacity_NearValue;
				foreach (var unit in units) {
					if (unit.playerId == this.playerId && unit.destination == bs && unit.TicksLeftToDestination() <= this.tickReact)
						currUnits += unit.warriorsCnt;
				}
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
					if (unit.playerId != this.playerId && unit.destination == bs) {
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
			List<List<BasicSity>> potentialRushes = new List<List<BasicSity>>();
			for (byte i = 1; i <= settings.values.bot_rushBot_Rush_Cnt; ++i) {
				potentialRushes.Add(new List<BasicSity>());

				foreach (var sity in canAttackDirectly) {
					uint potentialArmy = CalcPotentialArmy(i, sity.defPersent);
					if ( !rushingSities.Contains(sity) &&
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
					rushChance.Add(new KeyValuePair<byte, byte>(values.bot_rushBot_RushWaves_Chance[tmp].Key, values.bot_rushBot_RushWaves_Chance[tmp].Value));
				++tmp;
			}

			//System.Windows.MessageBox.Show(rushChance.Count.ToString());

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

				byte randPersent = (byte)values.rnd.Next(0, 100);
				for (int i = 0; i < rushChance.Count; ++i) {
					if (randPersent <= rushChance[i].Value) {
						potentialRushPos = rushChance[i].Key - 1;
						break;
					}
					else
						randPersent -= rushChance[i].Value;
				}

				var potentialRush = potentialRushes[potentialRushPos];
				rushSity = potentialRush[settings.values.rnd.Next(0, potentialRush.Count)];
				isRushing = true;
				rushWaveRemains = (byte)(potentialRushPos + 1);
			}
			
		}

		void RushFromAllSities() {
			if (rushSity.playerId == this.playerId)
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

		}

		void MoveUnitsToWeakSity() {

		}

		//---------------------------------------------- Methods - Support  ----------------------------------------------

		uint CalcPotentialArmy(byte rushesCntBase, double enemyDefMult) {
			uint potentialArmy = 0;
			foreach (var sity in botSities) {
				ushort currSend = 0, rushesCnt = rushesCntBase;
				while (rushesCnt-- != 0)
					currSend += (ushort)((sity.currWarriors - currSend) * sity.sendPersent);
				potentialArmy += (uint)Math.Round((2 - enemyDefMult) * currSend);
			}
			//System.Windows.MessageBox.Show(botSities[0].playerId.ToString() + "  " + potentialArmy.ToString());
			return potentialArmy;
		}

		int GetAvgDistance(BasicSity sity) {
			bool b;
			double avg = 0;

			foreach (var bs in botSities) {
				avg += bs.GetShortestPath(sity, out b);
			}
			avg /= botSities.Count();

			return (int)Math.Round(avg);
		}

		double GetEnemyArmy(BasicSity sityTo) {
			if (sityTo.playerId == 0)
				return sityTo.currWarriors +
					values.bot_rushBot_Rush_MinimumMore;
			else
				return sityTo.currWarriors +
					values.bot_rushBot_Rush_MinimumMore +
					((values.basicUnit_ticks_MoveWarrior * GetAvgDistance(sityTo)) / sityTo.ticksPerIncome);
		}
	}

}
