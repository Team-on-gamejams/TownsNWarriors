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

				if (!isRushing) 
					CalculateWhoNeedToBeRushed();
				if(isRushing)
					RushFromAllSities();

				if (values.bot_rushBot_IsDropOvercapacityUnits)
					DropOvercapacityUnits();

				if (values.bot_rushBot_IsProtectSities)
					ProtectSities();

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

		//---------------------------------------------- Methods - behavior ----------------------------------------------

		void CalculateWhoNeedToBeRushed() {
			List<List<BasicSity>> potentialRushes = new List<List<BasicSity>>();
			for(byte i = 1; i <= settings.values.bot_rushBot_RushCnt; ++i ) {
				potentialRushes.Add(new List<BasicSity>());
				uint potentialArmy = CalcPotentialArmy(i);

				foreach (var sity in sities) {
					if (sity.playerId != playerId &&
						!rushingSities.Contains(sity) &&
						GetEnemyArmy(sity) < potentialArmy
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

			int potentialRushPos = values.rnd.Next(0, potentialRushes.Count - 1);
			var potentialRush = potentialRushes[potentialRushPos];
			if (potentialRush.Count != 0) {
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
			double avg = 0;

			foreach (var bs in botSities) {
				avg += bs.GetShortestPath(sity);
			}
			avg /= botSities.Count();

			return (int)Math.Round(avg);
		}

		double GetEnemyArmy(BasicSity sityTo) {
			if (sityTo.playerId == 0)
				return sityTo.GetDefWarriors() +
					values.bot_rushBot_MinimumMore;
			else
				return sityTo.GetDefWarriors() +
					values.bot_rushBot_MinimumMore +
					((values.basicUnit_ticks_MoveWarrior * GetAvgDistance(sityTo)) / sityTo.ticksPerIncome);
		}

		
	}
}
