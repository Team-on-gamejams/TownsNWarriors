using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.bot {
	class SimpleBot : BasicBot {
		//---------------------------------------------- Fields ----------------------------------------------
		int botStreakCnt = 0;
		bool botStreak = false;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public SimpleBot(game.map.GameMap Map,
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
			if (globalGameInfo.tick > 100 && globalGameInfo.tick % 50 == 0) {
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
