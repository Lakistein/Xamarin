using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FloodIt
{
	public partial class GamePage : ContentPage
	{
		public const int SMALL_SIZE = 12, MEDIUM_SIZE = 18, BIG_SIZE = 24;
		public static int BoardSize;
		public static BoxView[,] colors;

		private int MovesRemaining;

		private Color selectedColor = Color.Purple, oldColor = Color.Purple;
		Dictionary<int, Color> colorDict = new Dictionary<int, Color> ();

		public GamePage () {
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();

			colorDict.Add (0, Color.Purple);
			colorDict.Add (1, Color.Blue);
			colorDict.Add (2, Color.Green);
			colorDict.Add (3, Color.Red);
			colorDict.Add (4, Color.Maroon);
			colorDict.Add (5, Color.Yellow);
			Initialise ();
			SetupBoxes ();
		}

		public void Initialise () {
			colors = new BoxView[BoardSize, BoardSize];
			double width = Math.Truncate (MainPage.ResolutionWidth / (BoardSize + 1));
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					colors [i, j] = new BoxView ();
					colors [i, j].WidthRequest = width;
					colors [i, j].HeightRequest = width;
				}
			}
		}

		private void SetupBoxes () {
			Random r = new Random ();
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					colors [i, j].Color = colorDict [r.Next (6)];
					grid.Children.Add (colors [i, j], i, j);
				}
			}
			MovesRemaining = BoardSize * 2;
			lblMoves.Text = "Moves remaining: " + MovesRemaining.ToString ();
		}

		private void Restart () {
			MovesRemaining = BoardSize * 2;
			SetupBoxes ();
		}

		public async void OnColorButtonClicked (object sender, EventArgs e) {
			lblMoves.Text = "Moves remaining: " + (--MovesRemaining).ToString ();
			oldColor = colors [0, 0].Color;
			selectedColor = ((Button)sender).BackgroundColor;
			CheckAdjecentBox (0, 0);
			if (IsGameDone ()) {
				var answer = await DisplayAlert ("Well Done", "You have finished the game!", "New Game", "Exit");
				if (answer)
					Restart ();
				else
					await Navigation.PushAsync (new MainPage (), true);
			} else if (MovesRemaining == 0) {
				var answer = await DisplayAlert ("Game Over", "You have failed!", "New game", "Exit");
				if (answer)
					Restart ();
				else
					await Navigation.PushAsync (new MainPage (), true);
			}
		}

		private void CheckAdjecentBox (int i, int j) {
			if (oldColor == selectedColor || colors [i, j].Color != oldColor)
				return;

			colors [i, j].Color = selectedColor;

			if (j > 0)
				CheckAdjecentBox (i, j - 1);
			if (j < BoardSize - 1)
				CheckAdjecentBox (i, j + 1);
			if (i > 0)
				CheckAdjecentBox (i - 1, j);
			if (i < BoardSize - 1)
				CheckAdjecentBox (i + 1, j);
		}

		private bool IsGameDone () {
			Color c = colors [0, 0].Color;
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					if (colors [i, j].Color != c)
						return false;
				}
			}
			return true;
		}
	}
}