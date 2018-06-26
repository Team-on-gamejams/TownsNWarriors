using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.settings;

namespace taw.game.map.generators.idSetters {
	class IdSetterDiffCorners : BasicIdSetter {
		//---------------------------------------------- Fields ----------------------------------------------

		//---------------------------------------------- Ctor ----------------------------------------------
		public IdSetterDiffCorners() {
		}

		//------------------------------------------ Inharitated methods ------------------------------------------
		public override void SetId() {
			bool end = false;
			for(int controlNum = 0; controlNum < townsPerControl.Count; ++controlNum) {
				int townsCnt = townsPerControl[controlNum];
				while (townsCnt-- != 0) {

					int needI = -1, needJ = -1;
					for (int i = gameMap.SizeY - 1; i >= 0; --i) {
						for (int j = gameMap.SizeX - 1; j >= 0; --j)
							if (gameMap.Map[i][j].City != null &&
								gameMap.Map[i][j].City.PlayerId == 0 && 
								(
								((controlNum + 1) % 4 == 1 && IsNearbyLeftTop(needI, needJ, i, j)) ||
								((controlNum + 1) % 4 == 2 && IsNearbyRightTop(needI, needJ, i, j)) ||
								((controlNum + 1) % 4 == 3 && IsNearbyRightBottom(needI, needJ, i, j)) ||
								((controlNum + 1) % 4 == 0 && IsNearbyLeftBottom(needI, needJ, i, j)) 
								)
							) {
								needI = i;
								needJ = j;
							}
					}

					if (needI == -1 || needJ == -1) {
						end = true;
						break;
					}


					gameMap.Map[needI][needJ].City.PlayerId = (byte)(controlNum + 1);
				}

				if (end)
					break;
			}
		}

		bool IsNearbyLeftTop(int ni, int nj, int i, int j) => (((ni & nj & -0x1) == -1)? true : (ni + nj > i + j));
		bool IsNearbyLeftBottom(int ni, int nj, int i, int j) => (((ni & nj & -0x1) == -1)? true : (nj - ni > j - i));
		bool IsNearbyRightTop(int ni, int nj, int i, int j) => (((ni & nj & -0x1) == -1)? true : (ni - nj > i - j));
		bool IsNearbyRightBottom(int ni, int nj, int i, int j) => (((ni & nj & -0x1) == -1)? true : (ni + nj < i + j));

		public override SettinsSetter CreateLinkedSetting() {
			return new taw.game.settings.generators.IdSetterDiffCornersSettings();
		}

		//------------------------------------------ Support methods ------------------------------------------


	}
}
