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
		Lazy<List<BasicUnit>> units;

		//---------------------------------------------- Properties ----------------------------------------------
		public bool IsOpenLeft { get; set; }
		public bool IsOpenTop { get; set; }
		public bool IsOpenRight { get; set; }
		public bool IsOpenBottom { get; set; }

		public BasicCity Sity { get; set; }
		public List<BasicUnit> Units => units.Value;

		public object OutputInfo { get; set; }

		//---------------------------------------------- Ctor ----------------------------------------------
		public GameCell() {
			IsOpenBottom = IsOpenLeft = IsOpenRight = IsOpenTop = false;
			units = new Lazy<List<BasicUnit>>();
			Sity = null;
		}
	}
}
