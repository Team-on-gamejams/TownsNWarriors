﻿using System;
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

		public static byte generator_CityId_Bots = 3;
		public static byte generator_CityId_TownsPerPlayer = 1;
		public static byte generator_CityId_TownsPerBot = 1;


		//Воно ще нада, але в значенні середньої кількості тіків, яка треба на рух
		public static ushort basicUnit_ticks_MoveWarrior = 10;

		public static ushort style_Num = 1;

		public static byte bot_rushBot_Tick_IgnoreFirstN = 200;
		public static byte bot_rushBot_Tick_React = (byte)(basicUnit_ticks_MoveWarrior / 2);
		public static bool bot_rushBot_IsProtectSities = true;
		public static byte bot_rushBot_Protect_MinimumUnitsLeft = 2;
		public static bool bot_rushBot_IsDropOvercapacityUnits = true;
		public static byte bot_rushBot_Overcapacity_NearValue = 1;
		public static bool bot_rushBot_IsMoveUnitsToWeakSities = true;
		public static byte bot_rushBot_Rush_Cnt = 4;
		public static List<KeyValuePair<byte, byte>> bot_rushBot_RushWaves_Chance = new List<KeyValuePair<byte, byte>>() {
			new KeyValuePair<byte, byte>(1, 25),
			new KeyValuePair<byte, byte>(2, 25),
			new KeyValuePair<byte, byte>(3, 25),
			new KeyValuePair<byte, byte>(4, 25),
		};
		public static byte bot_rushBot_Rush_MinimumMore = 1;
	}
}
