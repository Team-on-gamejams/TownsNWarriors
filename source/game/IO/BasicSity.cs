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

namespace TownsAndWarriors.game.sity
{
	public partial class BasicSity
	{
		Label text = new Label();

		public static List<BasicSity> selected = new List<BasicSity>();

		public override void InitializeShape()
		{
			shape = new Grid();
			//shape.Style = (Style)shape.FindResource("BasicCityMouseMove");

			Label newLabel = new Label();
			newLabel.Style = (Style)newLabel.FindResource("BasicCityStyle");

			shape.Children.Add(newLabel);

			shape.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
			{
				selected.Add(this);
			};

			shape.Children.Add(text);
			//тут створювати всі собитія з городом
		}
		public override void UpdateValue()
		{
			text.Content = this.currWarriors.ToString() + '/' + maxWarriors.ToString() + '\n' + this.playerId.ToString();
		}
	}
}