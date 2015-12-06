using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MemoryPuzzle
{
	public partial class GamePage : ContentPage
	{
		public const int SMALL_SIZE = 4, MEDIUM_SIZE = 6, BIG_SIZE = 8;
		public static int BoardSize;
		public static Button[,] colors;
		private Button clicked = null;
		private int MovesRemaining;
		List<Color> symbols = new List<Color> ();
		bool IsCounting = false;
		Dictionary<int, Color> colorDict = new Dictionary<int, Color> ();

		public GamePage () {
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();

			colorDict.Add (0, Color.Accent);
			colorDict.Add (1, Color.Aqua);
			colorDict.Add (2, Color.Blue);
			colorDict.Add (3, Color.Fuchsia);
			colorDict.Add (4, Color.Gray);
			colorDict.Add (5, Color.Green);
			colorDict.Add (6, Color.Lime);
			colorDict.Add (7, Color.Maroon);
			colorDict.Add (8, Color.Navy);
			colorDict.Add (9, Color.Olive);
			colorDict.Add (10, Color.Pink);
			colorDict.Add (11, Color.Purple);
			colorDict.Add (12, Color.Red);
			colorDict.Add (13, Color.Silver);
			colorDict.Add (14, Color.Teal);
			colorDict.Add (15, Color.Yellow);

			symbols.Add (Color.Yellow);
			symbols.Add (Color.Aqua);
			symbols.Add (Color.Blue);
			symbols.Add (Color.Gray);
			symbols.Add (Color.Green);
			symbols.Add (Color.Lime);
			symbols.Add (Color.Navy);
			symbols.Add (Color.Olive);
			if (BoardSize > 8) {
				symbols.Add (Color.Pink);
				symbols.Add (Color.Purple);
				symbols.Add (Color.Red);
				symbols.Add (Color.Silver);
				symbols.Add (Color.Teal);
				symbols.Add (Color.Accent);
				symbols.Add (Color.Fuchsia);
				symbols.Add (Color.Maroon);
			}

			symbols.Add (Color.Yellow);
			symbols.Add (Color.Aqua);
			symbols.Add (Color.Blue);
			symbols.Add (Color.Gray);
			symbols.Add (Color.Green);
			symbols.Add (Color.Lime);
			symbols.Add (Color.Navy);
			symbols.Add (Color.Olive);
			if (BoardSize > 8) {
				symbols.Add (Color.Pink);
				symbols.Add (Color.Purple);
				symbols.Add (Color.Red);
				symbols.Add (Color.Silver);
				symbols.Add (Color.Teal);
				symbols.Add (Color.Accent);
				symbols.Add (Color.Fuchsia);
				symbols.Add (Color.Maroon);
			}

			Initialise ();
			SetupBoxes ();
		}

		public void Initialise () {
			colors = new Button[BoardSize, BoardSize];
			double width = Math.Truncate (MainPage.ResolutionWidth / (BoardSize));
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					colors [i, j] = new Button ();
					colors [i, j].WidthRequest = width;
					colors [i, j].HeightRequest = width;
					colors [i, j].BorderRadius = 0;
					colors [i, j].Clicked += OnBoxClicked;
				}
			}
		}

		private void SetupBoxes () {
			Random r = new Random ();
			grid.Children.Clear ();
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					Color c = symbols [r.Next (symbols.Count)];
					symbols.Remove (c);
					colors [i, j].TextColor = c;
					colors [i, j].BackgroundColor = Color.Black;
					grid.Children.Add (colors [i, j], i, j);
					colors [i, j].IsEnabled = true;
				}
			}
			MovesRemaining = 0;
			lblMoves.Text = "Moves: " + MovesRemaining.ToString ();
		}

		private void Restart () {
			MovesRemaining = 0;
			symbols.Clear ();
			symbols.Add (Color.Yellow);
			symbols.Add (Color.Aqua);
			symbols.Add (Color.Blue);
			symbols.Add (Color.Gray);
			symbols.Add (Color.Green);
			symbols.Add (Color.Lime);
			symbols.Add (Color.Navy);
			symbols.Add (Color.Olive);
			if (BoardSize > 8) {
				symbols.Add (Color.Pink);
				symbols.Add (Color.Purple);
				symbols.Add (Color.Red);
				symbols.Add (Color.Silver);
				symbols.Add (Color.Teal);
				symbols.Add (Color.Accent);
				symbols.Add (Color.Fuchsia);
				symbols.Add (Color.Maroon);
			}

			symbols.Add (Color.Yellow);
			symbols.Add (Color.Aqua);
			symbols.Add (Color.Blue);
			symbols.Add (Color.Gray);
			symbols.Add (Color.Green);
			symbols.Add (Color.Lime);
			symbols.Add (Color.Navy);
			symbols.Add (Color.Olive);
			if (BoardSize > 8) {
				symbols.Add (Color.Pink);
				symbols.Add (Color.Purple);
				symbols.Add (Color.Red);
				symbols.Add (Color.Silver);
				symbols.Add (Color.Teal);
				symbols.Add (Color.Accent);
				symbols.Add (Color.Fuchsia);
				symbols.Add (Color.Maroon);
			}
			SetupBoxes ();
		}

		public async void OnBoxClicked (object sender, EventArgs e) {
			if (IsCounting)
				return;


			if (clicked == null) {
				clicked = sender as Button;
				clicked.BackgroundColor = clicked.TextColor;
				clicked.IsEnabled = false;
				return;
			}

			MovesRemaining++;
			lblMoves.Text = "Moves: " + MovesRemaining.ToString ();

			Button secButt = (Button)sender;
			secButt.BackgroundColor = secButt.TextColor;

			if (clicked.BackgroundColor != secButt.BackgroundColor) {
				IsCounting = true;
				Device.StartTimer (TimeSpan.FromSeconds (1), () => {
					clicked.BackgroundColor = Color.Black;
					clicked.IsEnabled = true;
					clicked = null;
					secButt.BackgroundColor = Color.Black;
					IsCounting = false;
					return false;
				});
				return;
			} 

			clicked.IsEnabled = false;
			secButt.IsEnabled = false;
			clicked = null;
			if (IsGameDone ()) {
				bool answer = await DisplayAlert ("Well Done", "You have solved the puzzle in " + MovesRemaining.ToString () + " steps!", "New Game", "Exit");
				if (answer)
					Restart ();
				else
					await Navigation.PushAsync (new MainPage ());
			}
		}

		private bool IsGameDone () {
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					if (colors [i, j].BackgroundColor == Color.Black)
						return false;
				}
			}
			return true;
		}
	}
}