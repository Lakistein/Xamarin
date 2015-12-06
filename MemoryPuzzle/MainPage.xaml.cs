using System;
using Xamarin.Forms;

namespace MemoryPuzzle
{
	public partial class MainPage : ContentPage
	{
		public static double ResolutionWidth, ResolutionHeight;

		public MainPage () {
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);
			picker.SelectedIndex = 0;
		}

		public void OnBtnStart (object sender, EventArgs e) {
			ResolutionWidth = stkL.Width;
			ResolutionHeight = stkL.Height;
			Navigation.PushAsync (new GamePage (), true);
		}

		public void pickerChanged (object sender, EventArgs e) {
			Picker p = (Picker)sender;
			if (p.SelectedIndex == 0)
				GamePage.BoardSize = GamePage.SMALL_SIZE;
			else if (p.SelectedIndex == 1)
				GamePage.BoardSize = GamePage.MEDIUM_SIZE;
			else
				GamePage.BoardSize = GamePage.BIG_SIZE;
		}
	}
}

