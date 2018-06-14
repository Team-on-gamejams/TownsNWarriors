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
		List<sity.BasicSity> botSities = new List<sity.BasicSity>();
		List<sity.BasicSity> rushingSities = new List<sity.BasicSity>();
		List<sity.BasicSity> canAttackDirectly = new List<sity.BasicSity>();

		bool isRushing = false;
		byte rushWaveRemains;
		BasicSity rushSity;

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
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (globalGameInfo.tick > settings.values.bot_rushBot_Tick_IgnoreFirstN && 
				globalGameInfo.tick % values.bot_rushBot_Tick_React == 0) {

				RecalcRushingSities();
				RecalcBotSities();
				RecalcCanAttackDirectly();

				if (!isRushing) 
					CalculateWhoNeedToBeRushed();
				if(isRushing)
					RushFromAllSities();

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
			REPEAT_FILL_CITY:
			foreach (var sity in sities) {
				if (botSities.Contains(sity)) {
					if (sity.playerId != this.playerId) {
						botSities.Remove(sity);
						goto REPEAT_FILL_CITY;
					}
				}
				else if (sity.playerId == this.playerId) {
					botSities.Add(sity);
					goto REPEAT_FILL_CITY;
				}
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
				if (sity.playerId != playerId) {
					bool directly = false;
					foreach (var bs in botSities) {
						bool tmp;
						sity.GetShortestPath(sity, out tmp);
						if (tmp) {
							directly = true;
						}
					}
					if (directly) {
						canAttackDirectly.Add(sity);
					}
				}
			}
		}


		//---------------------------------------------- Methods - behavior ----------------------------------------------

		void CalculateWhoNeedToBeRushed() {
			List<List<BasicSity>> potentialRushes = new List<List<BasicSity>>();
			for(byte i = 1; i <= settings.values.bot_rushBot_RushCnt; ++i ) {
				potentialRushes.Add(new List<BasicSity>());
				uint potentialArmy = CalcPotentialArmy(i);

				foreach (var sity in canAttackDirectly) {
					if (sity.playerId != playerId &&
						!rushingSities.Contains(sity) &&
						GetEnemyArmy(sity) < potentialArmy * (2 - sity.defPersent)
					) {
						bool isInPrev = false;
						foreach (var rush in potentialRushes) {
							if (rush.Contains(sity)) {
								isInPrev = true;
								break;
							}
						}
						if(!isInPrev)
							potentialRushes[i - 1].Add(sity);
					}
				}

			}

			if (potentialRushes.Count != 0) {
				bool canRush = false;
				int potentialRushPos = 0;

				if (potentialRushes.Count == 1)
					potentialRushPos = 0;
				else {
					int tmp = 0;
					List<KeyValuePair<byte, byte>> rushChance = new List<KeyValuePair<byte, byte>>(potentialRushes.Count);
					while (tmp != potentialRushes.Count) {
						if(potentialRushes[tmp].Count != 0) 
						rushChance.Add(new KeyValuePair<byte, byte>(values.bot_rushBot_RushWavesChance[tmp].Key, values.bot_rushBot_RushWavesChance[tmp].Value));
						++tmp;
					}

					if (rushChance.Count != 0) {
						canRush = true;

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
						}
					}
					else
						canRush = false;
				}

				if (canRush) {
					var potentialRush = potentialRushes[potentialRushPos];
					rushSity = potentialRush[settings.values.rnd.Next(0, potentialRush.Count)];
					isRushing = true;
					rushWaveRemains = (byte)(potentialRushPos + 1);
				}
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

		void DropOvercapacityUnits() {

		}

		void ProtectSities() {

		}

		void MoveUnitsToWeakSity() {

		}

		//---------------------------------------------- Methods - Support  ----------------------------------------------

		uint CalcPotentialArmy(byte rushesCntBase) {
			uint potentialArmy = 0;
			foreach (var sity in botSities) {
				ushort currSend = 0, rushesCnt = rushesCntBase;
				while (rushesCnt-- != 0)
					currSend += (ushort)((sity.currWarriors - currSend) * sity.sendPersent);
				potentialArmy += currSend;
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
					values.bot_rushBot_MinimumMore;
			else
				return sityTo.currWarriors +
					values.bot_rushBot_MinimumMore +
					((values.basicUnit_ticks_MoveWarrior * GetAvgDistance(sityTo)) / sityTo.ticksPerIncome);
		}

	}

}
