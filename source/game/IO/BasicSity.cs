using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TownsAndWarriors.game.sity {
	public partial class BasicSity {
		Label text = new Label();

		public override void InitializeShape() {
			shape = new Grid();
			shape.Children.Add(new Rectangle() {
				Fill = Brushes.Green
			});
			shape.Children.Add(text);
            //тут створювати всі собитія з городом
		}
		public override void UpdateValue() {
			text.Content = this.currWarriors.ToString() + '/' + maxWarriors.ToString() + '\n' + this.playerId.ToString();
		}
	}
}
