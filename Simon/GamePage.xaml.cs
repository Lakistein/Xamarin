using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Simon
{
	public partial class GamePage : ContentPage
	{
		private List<int> pattern = new List<int> ();
		private int currGuess = 0, clicksLeft = 0, currPlayingButton = 0;
		private Button[] buttons = new Button[4];
		Random r = new Random (Environment.TickCount);

		public GamePage () {
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
			buttons [0] = btn0;
			buttons [1] = btn1;
			buttons [2] = btn2;
			buttons [3] = btn3;
			DisableButtons ();
			Restart ();
		}

		private void DisableButtons () {
			foreach (var item in buttons)
				item.IsEnabled = false;
		}

		private void EnableButtons () {
			foreach (var item in buttons)
				item.IsEnabled = true;
		}

		private void PlayPattern () {
			currPlayingButton = 0;
			lblTurn.Text = "Remember pattern!";
			DisableButtons ();
			bool firstPass = true;
			Color oldColor = Color.Black;
			Device.StartTimer (TimeSpan.FromSeconds (.5), () => {
				if (firstPass) {
					oldColor = buttons [pattern [currPlayingButton]].BackgroundColor;
					buttons [pattern [currPlayingButton]].BackgroundColor = Color.Transparent;
					firstPass = false;
					return true;
				}
				firstPass = true;
				buttons [pattern [currPlayingButton]].BackgroundColor = oldColor;
				if (currPlayingButton == pattern.Count - 1) {
					EnableButtons ();
					lblTurn.Text = "Your turn.";
					clicksLeft = pattern.Count;
					lblMoves.Text = "Clicks left: " + clicksLeft.ToString ();
					return false;
				}
				currPlayingButton++;
				return true;
			});
		}

		private void AddToPattern () {
			pattern.Add (r.Next (0, 4));
			clicksLeft = pattern.Count;
			currGuess = 0;
		}

		private bool IsGuessCorrect (int num) {
			return (pattern [currGuess] == num);
		}

		private void Restart () {
			lblTurn.Text = "Get ready!";
			currGuess = 0;
			pattern.Clear ();
			List<int> temp = new List<int> ();
			for (int i = 0; i < 100; i++) {
				temp.Add (r.Next (0, 4));
			}
			for (int i = 0; i < 5; i++) {
				pattern.Add (temp [r.Next (temp.Count)]);
			}
			clicksLeft = pattern.Count;
			lblMoves.Text = "Clicks left: " + clicksLeft.ToString ();
			Device.StartTimer (TimeSpan.FromSeconds (3), () => {
				PlayPattern ();
				return false;
			});
		}

		private async void OnButtonClick (object sender, EventArgs e) {
			int num = int.Parse ((sender as Button).Text.ToString ());
			if (IsGuessCorrect (num)) {
				if (currGuess == pattern.Count - 1) {
					DisableButtons ();
					AddToPattern ();
					lblTurn.Text = "Remember pattern!";
					Device.StartTimer (TimeSpan.FromSeconds (1), () => {
						PlayPattern ();
						return false;
					});
					return;
				}
			} else {
				DisableButtons ();
				var answer = await DisplayAlert ("Game Over", "You managed to remember " + currGuess.ToString () + " colors long pattern.\n Play again?", "Yes", "No");
				if (answer) {
					Restart ();
					return;
				} else {
					await Navigation.PopToRootAsync ();
					return;
				}
			}
			currGuess++;
			clicksLeft = pattern.Count - currGuess;
			lblMoves.Text = "Clicks left: " + clicksLeft.ToString ();
		}
	}
}