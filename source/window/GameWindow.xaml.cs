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
using System.Windows.Shapes;


namespace TownsAndWarriors.window {
	/// <summary>
	/// Interaction logic for GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window {
		public GameWindow() {
			InitializeComponent();
		}

		public void ForceResize() {
			//this.OnRenderSizeChanged(new SizeChangedInfo(this, this.RenderSize, false, false));
			if(this.Width % 2 == 0)
				this.Width--;
			else
				this.Width++;
		}
	}
}
