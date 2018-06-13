using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.settings;


namespace TownsAndWarriors.game.bot {
	public class RushBot : BasicBot {
		//---------------------------------------------- Fields ----------------------------------------------
		int botStreakCnt = 0;
		bool botStreak = false;

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
				if (globalGameInfo.tick % 200 == 0)
					map.SendWarriors(sities[0], sities[2]);

				if (sities[2].currWarriors >= 20) {
					botStreak = true;
					botStreakCnt = 0;
				}

				if (botStreak) {
					++botStreakCnt;
					map.SendWarriors(sities[2], sities[1]);

					if (botStreakCnt == 3)
						botStreak = false;
				}

				return true;
			}

			return false;
		}
	}
}
