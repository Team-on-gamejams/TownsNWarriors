using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taw.game.basicInterfaces {
	/// <summary>
	/// Якщо клас помічений як Settingable, то для нього має бути створений відповідний класс SettinsSetter.
	/// Або, якщо клас унаслідуваний від Settingable-класу, то можна використовувати його SettinsSetter.
	/// </summary>
	public interface ISettingable {
		/// <summary>
		/// Встановлює налаштування для обєкту
		/// </summary>
		/// <example>
		/// public void GetSettings(SettinsSetter settinsSetter) {
		///		settinsSetter.SetSettings(this);
		/// }
		/// </example>
		/// <param name="settinsSetter">Відповідний цьому класу клас налаштувань, або клас налаштувань для батькіського класу</param>
		void SetSettings(taw.game.settings.SettinsSetter settinsSetter);

		/// <summary>
		/// Має бути virtual.
		/// Повертає Відповідний клас налаштувань
		/// </summary>
		/// <example>
		/// public virtual settings.SettinsSetter CreateLinkedSetting() {
		///		return new settings.unit.BasicUnitSettings();
		/// }
		/// </example>
		/// <returns>Відповідний цьому класу клас налаштувань</returns>
		game.settings.SettinsSetter CreateLinkedSetting();
	}
}
