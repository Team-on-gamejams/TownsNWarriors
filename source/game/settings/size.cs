using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.settings {
	static class size {
		public delegate void SizeChangedDelegate();
		static public event SizeChangedDelegate SizeChanged;

		static public double oneCellSizeX = 1;
		static public double oneCellSizeY = 1;

		static public double OneCellSizeX {
			get {
				return oneCellSizeX;
			}
			set {
				oneCellSizeX = value;
				if (SizeChanged != null)
					SizeChanged();
			}
		}
		static public double OneCellSizeY {
			get {
				return oneCellSizeY;
			}
			set{
				oneCellSizeY = value;
				if (SizeChanged != null)
					SizeChanged();
			}
		}

		public static double sitySizeMult = 0.67;

		public static double roadWidth = 0.2;
		public static double roadHeight = 0.2;

		public static byte growIncr = 3;
	}
}
