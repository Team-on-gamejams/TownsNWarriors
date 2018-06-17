using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.settings {
	public static class values {
		public static Random rnd { get; private set; }
		public static int seedField;
		public static int seed {
			get => seedField;
			set {
				seedField = value;
				rnd = new Random(seed);
			}
		}

		public static ushort fieldSizeX = 10;
		public static ushort fieldSizeY = 10;

		public static ushort style_Num = 1;
	}
}
