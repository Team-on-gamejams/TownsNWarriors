using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.settings {
	public abstract class SettinsSetter {
		/// <summary>
		/// Заповнює всі поля обєкту вказаними значеннями.
		/// 
		/// </summary>
		/// <example>
		/// public override void SetSettings(game.basicInterfaces.Settingable obj) {
		///		BasicUnit unit = obj as BasicUnit;
		/// 	if (unit == null)
		/// 		throw new ApplicationException("Wrong unit in BasicUnitSettings.SetSettings");
		/// 
		/// 	base.SetSettings(obj);
		/// 
		///		unit.tickPerTurn = 10;
		/// }
		/// 
		/// </example>
		/// <param name="obj">Відповідний обєкту SettinsSetter, обєкт Settingable </param>
		abstract public void SetSettings(taw.game.basicInterfaces.ISettingable obj);
		abstract protected void LoadSettingsFromFile();
	}
}
