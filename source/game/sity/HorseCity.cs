using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TownsAndWarriors.game.IO;
using TownsAndWarriors.game.basicInterfaces;
using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.sity
{
	public partial class HorseCity : BasicSity
	{
		public HorseCity() : base()
		{
			maxWarriors = settings.values.horceCity_MaxWarriors;
			currWarriors = settings.values.horceCity_StartWarriors;
			sendPersent = settings.values.horceCity_sendWarriorsPersent;
			defPersent = settings.values.horceCity_defendWarriorsPersent;
		}

		protected override BasicUnit CreateLinkedUnit(ushort sendWarriors, BasicSity to)
		{
			return new HorseUnit(sendWarriors, this.playerId, pathToSities[to], to);
		}
	}
}
