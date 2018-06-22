using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game {
	static class Rand {
		static Random rand;
		static int seed;

		public static int Seed {
			get => seed;
			set {
				seed = value;
				rand = new Random(seed);
			}
		}

		static Rand() {
			Seed = (int)DateTime.Now.Ticks;
		}

		//[minVal maxVal)
		public static int Next(int minVal, int maxVal) => rand.Next(minVal, maxVal);
		//[0 1)
		public static double NextDouble() => rand.NextDouble();
		//[0 100)
		public static byte NextPersent() => (byte)rand.Next(0, 100);
	}
}
