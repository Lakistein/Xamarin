using System;
using Xamarin.Forms;

namespace Simon
{
	public partial class MainPage : ContentPage
	{
		public static double ResolutionWidth, ResolutionHeight;

		public MainPage () {
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);
		}

		public void OnBtnStart (object sender, EventArgs e) {
			ResolutionWidth = stkL.Width;
			ResolutionHeight = stkL.Height;
			Navigation.PushAsync (new GamePage (), true);
		}
	}
}

