using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.settings {
	public static class values {
		public static byte locateMemory_SizeForTowns = 12;
		public static byte locateMemory_SizeForUnits = 12;

		public static ushort fieldSizeX = 10;
		public static ushort fieldSizeY = 10;
		public static ushort milisecondsPerTick = 20;

		public static bool gameplay_SaveWarriorsOverCap = true;
		public static bool gameplay_EqualsMeansCapture = false;
		public static bool gameplay_RemoveOvercapedUnits = true;

		public static byte generator_TunenelMapGenerator_SkipChance = 10;
		public static byte generator_TunenelMapGenerator_IgnoreSkipChanceForFirstNTitles = 10;
		public static bool generator_TunenelMapGenerator_CrossOnStart = true;

		public static bool generator_SityPlacer14_QuadIsRoad = false;
		public static byte generator_SityPlacer14_Quad_Size = 25;
		public static byte generator_SityPlacer14_Quad_MinimimCnt = 2;
		public static byte generator_SityPlacer14_Quad_Sities_Min = 2;
		public static byte generator_SityPlacer14_Quad_Sities_Max = 4;
		public static bool generator_SityPlacer14_FillAllWith1Road = true;
		public static byte generator_SityPlacer14_Chance_PosWith1Road = 100;
		public static byte generator_SityPlacer14_Chance_PosWith2Road = 10;
		public static byte generator_SityPlacer14_Chance_PosWith3Road = 25;
		public static byte generator_SityPlacer14_Chance_PosWith4Road = 100;
		public static byte generator_SityPlacer14_Code_MaxSityPlaceRepeats = 3;

		public static byte generator_CityId_Bots = 1;
		public static byte generator_CityId_TownsPerPlayer = 1;
		public static byte generator_CityId_TownsPerBot = 1;

		public static ushort basicSity_MaxWarriors = 20;
		public static ushort basicSity_StartWarriors = 10;
		public static ushort basicSity_ticks_NewWarrior = 50;
		public static float basicSity_sendWarriorsPersent = 0.50f;

		public static ushort basicUnit_ticks_MoveWarrior = 5;

		public static byte bot_rushBot_Tick_IgnoreFirstN = 0;
		public static byte bot_rushBot_Tick_React = 50;
	}
}
