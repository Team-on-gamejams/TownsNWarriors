using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taw.game.city;
using taw.game.unit;

namespace taw.game.controlable.botControl.support {
	class UnitsMovingToCity {
		private readonly List<BasicUnit> allyUnitsMovingToCity;
		private readonly List<BasicUnit> enemyUnitsMovingToCity;

		public List<BasicUnit> AllyUnitsMovingToCity { get => allyUnitsMovingToCity; }
		public List<BasicUnit> EnemyUnitsMovingToCity { get => enemyUnitsMovingToCity;}

		public UnitsMovingToCity() {
			allyUnitsMovingToCity = new List<BasicUnit>();
			enemyUnitsMovingToCity = new List<BasicUnit>();
		}
	}
}
