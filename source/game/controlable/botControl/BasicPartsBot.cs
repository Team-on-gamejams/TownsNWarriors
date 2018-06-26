using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using taw.game.city;
using taw.game.map;
using taw.game.unit;

using taw.game.controlable.botControl.parts;
using taw.game.controlable.botControl.support;

namespace taw.game.controlable.botControl {
	class BasicPartsBot : BasicBot {
		//---------------------------------------------- Fields ----------------------------------------------
		List<BasicPart> parts;

		List<Command> currCommands;
		List<Command> longTimeCommands;


		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicPartsBot(GameMap Map, byte botId) : base(Map, botId) {
			currCommands = new List<Command>();
			longTimeCommands = new List<Command>();
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (GlobalGameInfo.tick > ignoreFirstNTicks && GlobalGameInfo.tick % tickReact == 0 &&
				LogicalPlayersSingletone.ControlInfoForParts[this.PlayerId].Count != 0) {



				return true;
			}
			return false;
		}

		void FillCommands() {

		}

		bool ExecuteCommand(Command c) {
			SetWarriorsToRushes();

			if (c.toType == Command.ToType.Direct && c.fromType == Command.FromType.Direct) {
				c.from.SendUnit(c.to);
			}

			return false;

			void SetWarriorsToRushes() {
				if (c.warriorsType == Command.WarriorsType.Count) {
					if (c.warriors < c.from.currWarriors) {
						ushort rushes = 1;
						while (GetSendByRushesCnt(c.from, rushes) < c.warriors)
							++rushes;
						c.warriorsType = Command.WarriorsType.Rushes;
						c.warriors = rushes;
					}
				}
			}

			int GetSendByRushesCnt(BasicCity city, ushort rushesCnt) {
				ushort sendWarriors = 0, currWarriors = city.currWarriors;
				while (rushesCnt-- != 0) {
					sendWarriors += (ushort)Math.Round(currWarriors * city.sendPersent * city.atkPersent);
					currWarriors -= (ushort)(currWarriors * city.sendPersent);
				}
				return sendWarriors;
			}
		}

		//---------------------------------------------- Methods - behavior ----------------------------------------------
		public void AddPart(BasicPart part) {
			part.PlayerId = this.PlayerId;
			parts.Add(part);
		}
		public void GetPartEnumerator() => parts.GetEnumerator();
		public void RemovePart(BasicPart part) => parts.Remove(part);
		public void ClearParts() => parts.Clear();
		

		//---------------------------------------------- Methods - Support  ----------------------------------------------
	}
}
