using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;

namespace SlidingPuzzle
{
	public partial class GamePage : ContentPage
	{
		public const int SMALL_BOARD = 3, MEDIUM_BOARD = 4, BIG_BOARD = 5;
		public static int BoardSize;

		private int Steps = 0;
		private Button[,] boxes;

		public GamePage () {
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);

			InitialiseBoard ();
		}

		private List<int> RandomiseNumbers () {
			List<int> numbers = new List<int> ();
			numbers = Enumerable.Range (1, BoardSize * BoardSize - 1).ToList ();
			numbers.Shuffle ();
			return numbers;
		}

		private void InitialiseBoard () {
			grid.Children.Clear ();
			boxes = new Button[BoardSize, BoardSize];
			List<int> numbers = RandomiseNumbers ();
			int count = 0;
			double width = Math.Round (MainPage.ResolutionWidth / (BoardSize));
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					boxes [i, j] = new Button ();
					boxes [i, j].WidthRequest = width;
					boxes [i, j].HeightRequest = width;
					boxes [i, j].BorderRadius = 0;
					boxes [i, j].BackgroundColor = Color.Blue;
					boxes [i, j].Clicked += OnBoxClicked;
					if (i == BoardSize - 1 && j == BoardSize - 1) {
						boxes [i, j] = null;
						i = BoardSize;
						j = BoardSize;
						break;
					}
					boxes [i, j].Text = numbers [count++].ToString ();
					grid.Children.Add (boxes [i, j], j, i);	
				}
			}
		}

		private bool IsGameDone () {
			if (boxes [BoardSize - 1, BoardSize - 1] != null)
				return false;
			int count = 1;
			for (int i = 0; i < BoardSize; i++) {
				for (int j = 0; j < BoardSize; j++) {
					if (boxes [i, j] != null && !boxes [i, j].Text.Equals (count.ToString ()))
						return false;
					count++;
				}
			}
			return true;
		}

		private async void OnBoxClicked (object sender, EventArgs e) {
			Button clicked = sender as Button;
			int i = -1, j = -1;
			for (int x = 0; x < BoardSize; x++) {
				for (int y = 0; y < BoardSize; y++) {
					if (boxes [x, y] != null && boxes [x, y].Text.Equals (clicked.Text)) {
						i = x;
						j = y;
						x = BoardSize;
						break;
					}
				}
			}
			bool moved = false;
			if (i > 0 && boxes [i - 1, j] == null) {
				boxes [i - 1, j] = clicked;
				boxes [i, j] = null;
				grid.Children.Remove (clicked);
				grid.Children.Add (clicked, j, i - 1);
				moved = true;
			} else if (j < BoardSize - 1 && boxes [i, j + 1] == null) {
				boxes [i, j + 1] = clicked;
				boxes [i, j] = null;
				grid.Children.Remove (clicked);
				grid.Children.Add (clicked, j + 1, i);
				moved = true;
			} else if (i < BoardSize - 1 && boxes [i + 1, j] == null) {
				boxes [i + 1, j] = clicked;
				boxes [i, j] = null;
				grid.Children.Remove (clicked);
				grid.Children.Add (clicked, j, i + 1);
				moved = true;
			} else if (j > 0 && boxes [i, j - 1] == null) {
				boxes [i, j - 1] = clicked;
				boxes [i, j] = null;
				grid.Children.Remove (clicked);
				grid.Children.Add (clicked, j - 1, i);
				moved = true;
			}
			if (!moved)
				return;
			Steps++;
			lblMoves.Text = "Moves: " + Steps.ToString();
			if (!IsGameDone ())
				return;

			var answer = await DisplayAlert ("Well Done", "You have finished the game in " + Steps.ToString () + " steps!", "New Game", "Exit");
			if (answer)
				Restart ();
			else
				await Navigation.PushAsync (new MainPage ());
		}

		private void Restart () {
			InitialiseBoard ();
		}
	}
}