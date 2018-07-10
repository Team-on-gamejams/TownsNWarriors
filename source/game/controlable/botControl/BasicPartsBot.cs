using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using taw.game.city;
using taw.game.map;
using taw.game.unit;
using lp = taw.game.controlable.botControl.support.LogicalPlayersSingletone;

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
			parts = new List<BasicPart>();
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (GlobalGameInfo.tick > ignoreFirstNTicks && GlobalGameInfo.tick % tickReact == 0 &&
				LogicalPlayersSingletone.ControlInfoForParts[this.PlayerId].Count != 0) {

				foreach (var part in parts)
					if (part.TickReact())
						currCommands.Add(part.GetRezult());

				currCommands.Sort(new Comparison<Command>((a, b) => a.prioritete - b.prioritete));

				/*
				//for (int i = 0; i < commands.Count; ++i) {
				//	for (int j = 0; j < commands.Count; ++j) {
				//		if (i != j) {
				//			if (commands[i].Command.CityTo == commands[j].Command.CityTo) {
				//				//Обєднати команди
				//			}
				//		}
				//	}
				//}
				*/
				while (currCommands.Count != 0) {
					if (ExecuteCommand(currCommands[0]))
						break;
					currCommands.RemoveAt(0);
				}


				return true;
			}
			return false;
		}

		void FillCommands() {

		}

		bool ExecuteCommand(Command c) {
			c = SetFromToDirect(c);
			c = SetWarriorsToRushes(c);

			if (c.toType == Command.ToType.Direct && c.fromType == Command.FromType.Direct && c.warriorsType == Command.WarriorsType.Rushes) {
				c.from.SendUnit(c.to);
				return true;
			}

			return false;

			Command SetFromToDirect(Command InCommand) {
				Command outCommand = (Command)InCommand.Clone();
				List<Tuple<BasicCity, int>> paths = new List<Tuple<BasicCity, int>>(lp.ControlInfoForParts[this.PlayerId].Keys.Count);

				foreach (var fromCity in lp.ControlInfoForParts[this.PlayerId].Keys) {
					var currPath = fromCity.BuildOptimalPath(outCommand.to, out BasicCity real);
					if (real == outCommand.to) 
						paths.Add(new Tuple<BasicCity, int>(fromCity, currPath.Count));
				}

				if (paths.Count == 0)
					return InCommand;

				int minId = 0;
				for (int i = 1; i < paths.Count; ++i)
					if (paths[i].Item2 < paths[minId].Item2)
						minId = i;

				outCommand.fromType = Command.FromType.Direct;
				outCommand.from = paths[minId].Item1;

				return outCommand;
			}

			Command SetWarriorsToRushes(Command InCommand) {
				Command outCommand = (Command)InCommand.Clone();

				if (outCommand.warriorsType == Command.WarriorsType.Count) {
					if (outCommand.warriors < outCommand.from.currWarriors) {
						ushort rushes = 1;
						while (GetSendByRushesCnt(outCommand.from, rushes) < outCommand.warriors)
							++rushes;
						outCommand.warriorsType = Command.WarriorsType.Rushes;
						outCommand.warriors = rushes;
					}
				}

				return outCommand;

				int GetSendByRushesCnt(BasicCity city, ushort rushesCnt) {
					ushort sendWarriors = 0, currWarriors = city.currWarriors;
					while (rushesCnt-- != 0) {
						sendWarriors += (ushort)Math.Round(currWarriors * city.sendPersent * city.atkPersent);
						currWarriors -= (ushort)(currWarriors * city.sendPersent);
					}
					return sendWarriors;
				}
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
