using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.settings {
	public static class values {
		//---------------------------------------------- Memory ----------------------------------------------
		public static byte locateMemory_SizeForTowns = 12;
		public static byte locateMemory_SizeForUnits = 12;

		public static bool gameplay_SaveWarriorsOverCap = true;
		public static bool gameplay_EqualsMeansCapture = false;
		public static bool gameplay_RemoveOvercapedUnits = true;

		public static byte generator_TunenelMapGenerator_SkipChance = 10;
		public static byte generator_TunenelMapGenerator_IgnoreSkipChanceForFirstNTitles = 10;
		public static bool generator_TunenelMapGenerator_CrossOnStart = true;


		public static ushort basicSity_MaxWarriors = 20;
		public static ushort basicSity_StartWarriors = 10;
		public static ushort basicSity_ticks_NewWarrior = 50;
		public static float basicSity_sendWarriorsPersent = 0.50f;

		public static ushort basicUnit_ticks_MoveWarrior = 50;
	}
}
