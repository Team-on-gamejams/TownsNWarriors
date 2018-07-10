using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.city;
using taw.game.unit;

namespace taw.game.map {
	public partial class GameCell : basicInterfaces.IOutputable {
		//---------------------------------------------- Fields ----------------------------------------------
		bool IsEventsCreated = false;
		Lazy<collection.ListWithEvents<BasicUnit>> units;

		//---------------------------------------------- Properties ----------------------------------------------
		public bool IsOpenLeft { get; set; }
		public bool IsOpenTop { get; set; }
		public bool IsOpenRight { get; set; }
		public bool IsOpenBottom { get; set; }

		public BasicCity City { get; set; }
		public List<BasicUnit> Units { get { InitEvents(); return units.Value; } } 

		public object OutputInfo { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public GameCell() {
			IsOpenBottom = IsOpenLeft = IsOpenRight = IsOpenTop = false;
			units = new Lazy<collection.ListWithEvents<BasicUnit>>();
			City = null;
		}

		void InitEvents() {
			if (!IsEventsCreated) {
				IsEventsCreated = true;
				units.Value.AddItem += () => TryUnionUnits();
			}
		}

		void TryUnionUnits() {
			REPEAT:
			for (int i = 0; i < units.Value.Count; ++i) {
				for (int j = i + 1; j < units.Value.Count; ++j) {
					if(units.Value[i].X == units.Value[j].X &&
						units.Value[i].Y == units.Value[j].Y &&
						units.Value[i].destination == units.Value[j].destination &&
						units.Value[i].PlayerId == units.Value[j].PlayerId &&
						units.Value[i].GetType() == units.Value[j].GetType()
					) {
						units.Value[i].currTickOnCell = (ushort)((units.Value[i].currTickOnCell + units.Value[j].currTickOnCell) / 2);
						units.Value[i].warriorsCnt += units.Value[j].warriorsCnt;

						units.Value[j].DestroyUnit();
						goto REPEAT;
					}
				}
			}
		}
	}
}
