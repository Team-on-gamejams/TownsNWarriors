using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TownsAndWarriors.game.unit;

namespace TownsAndWarriors.game.sity
{
    public partial class CastleCity : BasicSity
    {
        public CastleCity() : base()
        {
            maxWarriors = settings.values.castleCity_MaxWarriors;
            currWarriors = settings.values.castleCity_StartWarriors;
            sendPersent = settings.values.castleCity_sendWarriorsPersent;
            defPersent = settings.values.castleCity_defendWarriorsPersent;
        }

		protected override BasicUnit CreateLinkedUnit(ushort sendWarriors, BasicSity to)
		{
			return new BasicUnit(sendWarriors, this.playerId, pathToSities[to], to);
		}
	}
}
