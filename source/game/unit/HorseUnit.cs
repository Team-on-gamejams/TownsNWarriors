using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.sity;

namespace TownsAndWarriors.game.unit
{
	public partial class HorseUnit : BasicUnit
	{
		public HorseUnit(ushort warriorsCnt, byte PlayerId, List<KeyValuePair<int, int>> Path, BasicSity destination) : base(warriorsCnt, PlayerId, Path, destination)
		{
			tickPerTurn = TownsAndWarriors.game.settings.values.horseUnit_ticks_MoveWarrior;
		}
	}
}
