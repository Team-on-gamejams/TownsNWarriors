using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using taw.game.city;

namespace taw.game.controlable.botControl.parts {
	public class PartCommand {
		
		public BasicCity CityFrom { get; set; }
		
		public BasicCity CityTo { get; set; }
		public int MinWarriors { get; set; }
		public int MaxTick { get; set; }
		public bool IsForced { get; set; }

		///<param name="cityFrom">
		/// Має бути хоч би CityFrom або CityTo. Може бути оба.
		/// Звідки послати юнитів. 
		/// Може бути null, якщо це не важно(например атака чи защита).
		///</param>
		///
		/// <param name="cityTo">
		/// Куди послати юнитів. Може бути і враг і союзник. 
		/// Може бути null, якщо це не важно(например отступление). 
		///</param>
		///
		/// <param name="minWarriors">
		/// Мінімальна кількість юнитів для посилання. 
		///0 - якщо не важно(тоді просто піде 1 волна). -1 - всі.
		///Якщо CityFrom == null, то можуть піти з кількох міст.
		///</param>
		///
		/// <param name="maxTick">Обмеження на максимум тиків, поки юнит дійде. 
		/// 0 - нема. 
		/// Якщо ніяк не буде можливості послати MinWarriors за MaxTick
		/// </param>
		/// 
		/// <param name="isForced">Не ігнорувати цю команду.
		/// Виконається навіть якщо не вистачить юнітів чи не вкладеться в час по тиках.
		/// </param>
		public PartCommand(BasicCity cityFrom, BasicCity cityTo, int minWarriors, int maxTick, bool isForced) {
			CityFrom = cityFrom;
			CityTo = cityTo;
			MinWarriors = minWarriors;
			MaxTick = maxTick;
			IsForced = isForced;
		}
	}
}
