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

		bool isRushing = false;
		byte rushWaveRemains;
		BasicSity rushSity;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public RushBot(game.map.GameMap Map,
			List<game.sity.BasicSity> Sities,
			List<game.unit.BasicUnit> Units,
			byte botId) {
			map = Map;
			sities = Sities;
			units = Units;
			playerId = botId;
		}

		//---------------------------------------------- Methods ----------------------------------------------
		public override bool TickReact() {
			if (globalGameInfo.tick > settings.values.bot_rushBot_Tick_IgnoreFirstN && 
				globalGameInfo.tick % values.bot_rushBot_Tick_React == 0) {
				if (!isRushing) {
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

					uint potentialArmy = 0;
					foreach (var sity in botSities) {
						ushort currSend = 0, rushesCnt = settings.values.bot_rushBot_RushCnt;
						while (rushesCnt-- != 0)
							currSend += (ushort)((sity.currWarriors - currSend) * sity.sendPersent);
						potentialArmy += currSend;
					}

					sity.BasicSity weakestSity = sities[0];
					foreach (var sity in sities)
						if (sity.GetDefWarriors() < weakestSity.GetDefWarriors())
							weakestSity = sity;

					if (potentialArmy > weakestSity.GetDefWarriors() * values.bot_rushBot_MinimumMlt) {
						isRushing = true;
						rushWaveRemains = settings.values.bot_rushBot_RushCnt;
						rushSity = weakestSity;
					}
				}

				RushFromAllSities();
				return true;
			}

			return false;
		}

		void RushFromAllSities() {
			if (isRushing) {
				if(rushWaveRemains-- == 0) 
					isRushing = false;
				else {
					foreach (var sity in botSities) {
						map.SendWarriors(sity, rushSity);
					}
				}
			}
		}

	}
}
