using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.sity
{
    public partial class CastleCity : BasicSity
    {
        public CastleCity()
        {
            maxWarriors = settings.values.basicSity_MaxWarriors;
            currWarriors = settings.values.basicSity_StartWarriors;
            sendPersent = settings.values.basicSity_sendWarriorsPersent;
            defPersent = settings.values.basicSity_defendWarriorsPersent;
            pathToSities = new Dictionary<BasicSity, List<KeyValuePair<int, int>>>(1);
        }
    }
}
