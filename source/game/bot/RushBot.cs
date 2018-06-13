﻿using System;
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

		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			if (globalGameInfo.tick > settings.values.bot_rushBot_Tick_IgnoreFirstN && 
				globalGameInfo.tick % values.bot_rushBot_Tick_React == 0) {
				RecalcBotSities();
				if (!isRushing) 
					CalculateWhoNeedToBeRushed();
				else
					RushFromAllSities();



				return true;
			}

			return false;
		}

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

		void CalculateWhoNeedToBeRushed() {
			uint potentialArmy = CalcPotentialArmy(settings.values.bot_rushBot_RushCnt);

			RecalcRushingSities();

			List<BasicSity> potentialRush = new List<BasicSity>();
			foreach (var sity in sities)
				if (sity.GetDefWarriors() * values.bot_rushBot_MinimumMlt < potentialArmy && sity.playerId != playerId && !rushingSities.Contains(sity))
					potentialRush.Add(sity);

			if (potentialRush.Count != 0) {
				rushSity = potentialRush[settings.values.rnd.Next(0, potentialRush.Count)];
				isRushing = true;
				for(byte i = 1; i <= settings.values.bot_rushBot_RushCnt; ++i) {
					if (CalcPotentialArmy(i) > rushSity.GetDefWarriors() * values.bot_rushBot_MinimumMlt) {
						rushWaveRemains = i;
						break;
					}
				}
			}
		}

		uint CalcPotentialArmy(byte rushesCntBase) {
			uint potentialArmy = 0;
			foreach (var sity in botSities) {
				ushort currSend = 0, rushesCnt = rushesCntBase;
				while (rushesCnt-- != 0)
					currSend += (ushort)((sity.GetAtkWarriors() - currSend) * sity.sendPersent);
				potentialArmy += currSend;
			}
			return potentialArmy;
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

	//	void DropOverca
	}
}
