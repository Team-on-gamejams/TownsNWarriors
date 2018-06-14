using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownsAndWarriors.game.settings {
	public static class values {
		public static byte locateMemory_SizeForTowns = 12;
		public static byte locateMemory_SizeForUnits = 12;

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
		public static ushort milisecondsPerTick = 20;

		public static bool gameplay_SaveWarriorsOverCap = true;
		public static bool gameplay_EqualsMeansCapture = true;
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
		public static float basicSity_defendWarriorsPersent = 1.0f;

		public static ushort castleCity_MaxWarriors = 30;
		public static ushort castleCity_StartWarriors = 15;
		public static float castleCity_sendWarriorsPersent = 0.50f;
		public static float castleCity_defendWarriorsPersent = 1.5f;

		public static ushort horceCity_MaxWarriors = 20;
		public static ushort horceCity_StartWarriors = 10;
		public static float horceCity_sendWarriorsPersent = 0.50f;
		public static float horceCity_defendWarriorsPersent = 1.0f;

		public static ushort basicUnit_ticks_MoveWarrior = 7;

		public static byte bot_rushBot_Tick_IgnoreFirstN = 0;
		public static byte bot_rushBot_Tick_React = 50;
		public static byte bot_rushBot_RushCnt = 3;
		public static byte bot_rushBot_MinimumMore = 1;
		public static bool bot_rushBot_IsDropOvercapacityUnits = true;
		public static bool bot_rushBot_NearMaxMeansOvercapacityToo = true;
		public static bool bot_rushBot_IsProtectSities = true;
		public static bool bot_rushBot_IsMoveUnitsToWeakSities = true;
	}
}
