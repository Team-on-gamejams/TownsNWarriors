using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.IO;

namespace TownsAndWarriors.game.sity {
	public partial class BasicSity : GameCellDrawableObj, tickable {
		//---------------------------------------------- Fields ----------------------------------------------
		protected ushort currWarriors, maxWarriors, ticksPerIncome;
		
		
		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicSity() {
			maxWarriors = settings.values.basicSity_MaxWarriors;
			currWarriors = settings.values.basicSity_StartWarriors;
			ticksPerIncome = settings.values.basicSity_ticks_NewWarrior;
		}

		public void TickReact() {
			if (maxWarriors != currWarriors || game.globalGameInfo.tick % ticksPerIncome == 0)
				++currWarriors;
		}


		//---------------------------------------------- Methods ----------------------------------------------

	}
}
