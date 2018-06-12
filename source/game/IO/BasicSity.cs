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

        public static List<BasicSity> selected = new List<BasicSity>();

        public override void InitializeShape() {
			shape = new Grid();

			//Label
			shape.Children.Add(text);

			//Elipse
			var elipse = new Ellipse() {
				Fill = settings.colors.neutralTownFill,
				Stroke = settings.colors.neutralTownStroke,
				Width = 30,
				Height = 30
			};

			if(playerId == 1) {
				elipse.Fill = settings.colors.playerTownFill;
				elipse.Stroke = settings.colors.playerTownStroke;
			}
			else if (playerId != 0) {
				if (settings.colors.TownFills.Count <= playerId - 2)
					elipse.Fill = settings.colors.TownFills[settings.colors.TownFills.Count - 1];
				else
					elipse.Fill = settings.colors.TownFills[playerId - 2];

				if (settings.colors.TownStrokes.Count <= playerId - 2)
					elipse.Stroke = settings.colors.TownStrokes[settings.colors.TownStrokes.Count - 1];
				else
					elipse.Stroke = settings.colors.TownStrokes[playerId - 2];

			}

			shape.Children.Add(elipse);

			//Delegates

			//Rectangle newRec = new Rectangle();
			//newRec.Fill = Brushes.Green;
			shape.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
			{
				selected.Add(this);
			};
			//shape.MouseMove += delegate (object sender, MouseEventArgs e)
			//{
			//	newRec.Fill = Brushes.DarkGray;
			//};
			//shape.Children.Add(newRec);

            //тут створювати всі собитія з городом
		}
		public override void UpdateValue() {
			text.Content = this.currWarriors.ToString() + '/' + maxWarriors.ToString() + '\n' + this.playerId.ToString();
		}
	}
}
