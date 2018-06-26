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
		List<PartWithPriority> parts;
		List<PartCommandWithPriority> commands;
		List<PartCommandWithPriority> LongTimeCommands;

		//---------------------------------------------- Properties ----------------------------------------------


		//---------------------------------------------- Ctor ----------------------------------------------
		public BasicPartsBot(GameMap Map, byte botId) : base(Map, botId) {
			parts = new List<PartWithPriority>();
			commands = new List<PartCommandWithPriority>();
		}

		//---------------------------------------------- Methods - Main ----------------------------------------------
		public override bool TickReact() {
			if (GlobalGameInfo.tick > ignoreFirstNTicks && GlobalGameInfo.tick % tickReact == 0 &&
				LogicalPlayersSingletone.ControlInfoForParts[this.PlayerId].Count != 0
				) {
				commands.Clear();
				foreach (var part in parts)
					if (part.Part.TickReact())
						commands.Add(new PartCommandWithPriority(part.Priority, part.Part.GetTickReactResult()));

				commands.Sort(new Comparison<PartCommandWithPriority>((a, b) => a.Priority - b.Priority));

				//for (int i = 0; i < commands.Count; ++i) {
				//	for (int j = 0; j < commands.Count; ++j) {
				//		if (i != j) {
				//			if (commands[i].Command.CityTo == commands[j].Command.CityTo) {
				//				//Обєднати команди
				//			}
				//		}
				//	}
				//}

				if (commands.Count != 0) {
					ExecuteCommand(commands[0].Command);
				}


				return true;
			}
			return false;
		}

		bool ExecuteCommand(PartCommand command) {
			if (command.CityTo == null && command.CityFrom == null)
				return false;

			BasicCity to = command.CityTo, from = command.CityFrom;
			int warriors = command.MinWarriors, tick = command.MaxTick;

			if(from == null && to != null) {
				foreach (var i in LogicalPlayersSingletone.ControlInfoForParts[this.PlayerId]) {
					from = i.Key;
					break;
				}
				BasicCity.gameMap.SendWarriors(from, to);
			}

			return false;
		}

		//---------------------------------------------- Methods - behavior ----------------------------------------------
		public void AddPart(BasicPart part, short priority) => AddPart(new PartWithPriority(priority, part));
		public void AddPart(PartWithPriority partWithPriority){
			partWithPriority.Part.PlayerId = this.PlayerId;
			parts.Add(partWithPriority);
		}
		public void GetPartEnumerator() => parts.GetEnumerator();
		public void RemovePart(PartWithPriority partWithPriority) => parts.Remove(partWithPriority);
		public void ClearParts(PartWithPriority partWithPriority) => parts.Clear();


		//---------------------------------------------- Methods - Support  ----------------------------------------------
	}
}
