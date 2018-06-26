using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.city;
using taw.game.map;
using taw.game.unit;

namespace taw.game.controlable.botControl.parts {
	class Command {
		public enum FromType : byte { Direct }
		public enum ToType : byte { Direct }
		public enum WarriorsType : byte { Rushes, Count }

		public ushort prioritete;

		public FromType fromType;
		public ToType toType;
		public WarriorsType warriorsType;

		public BasicCity from, to;
		public ushort warriors;
	}
}
