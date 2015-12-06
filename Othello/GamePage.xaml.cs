using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Othello
{
	public partial class GamePage : ContentPage
	{
		Color WHITE = Color.White, BLACK = Color.Black, currPlayer = Color.White;
		Button[,] Buttons = new Button[8, 8];
		string[] indexes = new string[2];
		int whitePieces = 2, blackPieces = 2;

		public GamePage () {
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
			SetupBoard ();
		}

		void SetupBoard () {
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					Button b = new Button ();
					b.HeightRequest = MainPage.ResolutionWidth / 8;
					b.WidthRequest = MainPage.ResolutionWidth / 8;
					b.BackgroundColor = Color.Gray;
					b.Text = i.ToString () + "," + j.ToString ();
					b.TextColor = Color.Transparent;
					b.Clicked += OnButtonClicked;
					b.IsEnabled = false;
					if ((i == 3 || i == 4) && (j == 3 || j == 4))
						b.BorderRadius = (int)(MainPage.ResolutionWidth / 8);
					Buttons [i, j] = b;
					grid.Children.Add (b, j, i);
				}
			}
			ChangeButtonColor (3, 3, Buttons [3, 3], Color.White);
			ChangeButtonColor (3, 4, Buttons [3, 4], Color.Black);
			ChangeButtonColor (4, 3, Buttons [4, 3], Color.Black);
			ChangeButtonColor (4, 4, Buttons [4, 4], Color.White);
			UpdateCounters ();
		}

		void OnButtonClicked (object sender, EventArgs r) {
			Button b = sender as Button;
			b.BorderRadius = (int)(MainPage.ResolutionWidth / 8);
			indexes = b.Text.Split (',');
			int i = int.Parse (indexes [0]), j = int.Parse (indexes [1]);
			ChangeButtonColor (i, j, b, currPlayer);
			ChangeColors (i, j);
			CountPieces ();
			ChangeTurn ();
		}

		void ChangeButtonColor (int i, int j, Button b, Color c) {
			b.BackgroundColor = c;
			b.IsEnabled = false;
			grid.Children.Add (b, j, i);
			EnableAdjecentNotEnabledButtons (i, j);
		}

		void ChangeTurn () {
			if (currPlayer == Color.White) {
				currPlayer = Color.Black;
				lblTurn.Text = "Black player turn.";
			} else {
				currPlayer = Color.White;
				lblTurn.Text = "White player turn.";
			}
		}

		void EnableAdjecentNotEnabledButtons (int i, int j) {
			if (i - 1 >= 0 && j - 1 >= 0 && Buttons [i - 1, j - 1].BackgroundColor.Equals (Color.Gray)) // gore levo
				EnableButton (i - 1, j - 1);
			if (i - 1 >= 0 && Buttons [i - 1, j].BackgroundColor.Equals (Color.Gray)) // gore
				EnableButton (i - 1, j);
			if (i - 1 >= 0 && j + 1 < 8 && Buttons [i - 1, j + 1].BackgroundColor.Equals (Color.Gray)) // gore desno
				EnableButton (i - 1, j + 1);
			if (j + 1 < 8 && Buttons [i, j + 1].BackgroundColor.Equals (Color.Gray)) // desno
				EnableButton (i, j + 1);
			if (i + 1 < 8 && j + 1 < 8 && Buttons [i + 1, j + 1].BackgroundColor.Equals (Color.Gray)) // dole desno
				EnableButton (i + 1, j + 1);
			if (i + 1 < 8 && Buttons [i + 1, j].BackgroundColor.Equals (Color.Gray)) // dole
				EnableButton (i + 1, j);
			if (i + 1 < 8 && j - 1 >= 0 && Buttons [i + 1, j - 1].BackgroundColor.Equals (Color.Gray)) // dole levo
				EnableButton (i + 1, j - 1);
			if (j - 1 >= 0 && Buttons [i, j - 1].BackgroundColor.Equals (Color.Gray)) // levo
				EnableButton (i, j - 1);
		}

		void EnableButton (int i, int j) {
			Buttons [i, j].IsEnabled = true;
			grid.Children.Add (Buttons [i, j], j, i);
		}

		void ChangeColors (int i, int j) {
			CheckHorizontal (i, j);
			CheckVertical (i, j);
			CheckDiagonalUp (i, j);
			CheckDiagonalDown (i, j);
		}

		void CheckHorizontal (int i, int j) {
			bool isChangeable = false;
			int tmpJ = j - 1;
			// to left
			if (tmpJ >= 1 && !Buttons [i, tmpJ].BackgroundColor.Equals (Color.Gray) && !Buttons [i, tmpJ].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpJ--;
					if (tmpJ == -1 || Buttons [i, tmpJ].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [i, tmpJ].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpJ = j - 1;
				while (!Buttons [i, tmpJ].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (i, tmpJ, Buttons [i, tmpJ], currPlayer);
					tmpJ--;
				}
			}
			isChangeable = false;
			tmpJ = j + 1;
			// to right
			if (tmpJ < 7 && !Buttons [i, tmpJ].BackgroundColor.Equals (Color.Gray) && !Buttons [i, tmpJ].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpJ++;
					if (tmpJ == 8 || Buttons [i, tmpJ].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [i, tmpJ].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpJ = j + 1;
				while (!Buttons [i, tmpJ].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (i, tmpJ, Buttons [i, tmpJ], currPlayer);
					tmpJ++;
				}
			}
		}

		void CheckVertical (int i, int j) {
			bool isChangeable = false;
			int tmpI = i - 1;
			// to left
			if (tmpI >= 1 && !Buttons [tmpI, j].BackgroundColor.Equals (Color.Gray) && !Buttons [tmpI, j].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpI--;
					if (tmpI == -1 || Buttons [tmpI, j].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [tmpI, j].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpI = i - 1;
				while (!Buttons [tmpI, j].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (tmpI, j, Buttons [tmpI, j], currPlayer);
					tmpI--;
				}
			}
			isChangeable = false;
			tmpI = i + 1;
			// to right
			if (tmpI < 7 && !Buttons [tmpI, j].BackgroundColor.Equals (Color.Gray) && !Buttons [tmpI, j].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpI++;
					if (tmpI == 8 || Buttons [tmpI, j].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [tmpI, j].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpI = i + 1;
				while (!Buttons [tmpI, j].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (tmpI, j, Buttons [tmpI, j], currPlayer);
					tmpI++;
				}
			}
		}

		void CheckDiagonalUp (int i, int j) {
			bool isChangeable = false;
			int tmpI = i - 1, tmpJ = j + 1;
			// to left
			if (tmpI >= 1 && tmpJ < 7 && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray) && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpI--;
					tmpJ++;
					if (tmpI == -1 || tmpJ == 8 || Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpI = i - 1;
				tmpJ = j + 1;
				while (!Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (tmpI, tmpJ, Buttons [tmpI, tmpJ], currPlayer);
					tmpI--;
					tmpJ++;
				}
			}
			isChangeable = false;
			tmpI = i + 1;
			tmpJ = j - 1;
			// to right
			if (tmpI < 7 && tmpJ >= 1 && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray) && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpI++;
					tmpJ--;
					if (tmpI == 8 || tmpJ == -1 || Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpI = i + 1;
				tmpJ = j - 1;
				while (!Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (tmpI, tmpJ, Buttons [tmpI, tmpJ], currPlayer);
					tmpI++;
					tmpJ--;
				}
			}
		}

		void CheckDiagonalDown (int i, int j) {
			bool isChangeable = false;
			int tmpI = i - 1, tmpJ = j - 1;
			// to left
			if (tmpI >= 1 && tmpJ >= 1 && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray) && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpI--;
					tmpJ--;
					if (tmpI == -1 || tmpJ == -1 || Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpI = i - 1;
				tmpJ = j - 1;
				while (!Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (tmpI, tmpJ, Buttons [tmpI, tmpJ], currPlayer);
					tmpI--;
					tmpJ--;
				}
			}
			isChangeable = false;
			tmpI = i + 1;
			tmpJ = j + 1;
			// to right
			if (tmpI < 7 && tmpJ < 7 && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray) && !Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
				isChangeable = true;
				while (true) {
					tmpI++;
					tmpJ++;
					if (tmpI == 8 || tmpJ == 8 || Buttons [tmpI, tmpJ].BackgroundColor.Equals (Color.Gray)) {
						isChangeable = false;
						break;
					}
					if (Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer))
						break;
				}
			}
			if (isChangeable) {
				tmpI = i + 1;
				tmpJ = j + 1;
				while (!Buttons [tmpI, tmpJ].BackgroundColor.Equals (currPlayer)) {
					ChangeButtonColor (tmpI, tmpJ, Buttons [tmpI, tmpJ], currPlayer);
					tmpI++;
					tmpJ++;
				}
			}
		}

		Color GetOpositeColor (Color c) {
			return (c == Color.White) ? Color.Black : Color.White;
		}

		void CountPieces () {
			whitePieces = 0;
			blackPieces = 0;
			bool isGameOver = true;
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					if (Buttons [i, j].BackgroundColor.Equals (Color.Gray)) {
						isGameOver = false;
						continue;
					}
					if (Buttons [i, j].BackgroundColor.Equals (Color.White))
						whitePieces++;
					else
						blackPieces++;
				}
			}
			UpdateCounters ();
			if (isGameOver)
				GameOver ();
		}

		async void GameOver () {
			string winner = (whitePieces > blackPieces) ? "White " : "Black: ";
			winner = (whitePieces == blackPieces) ? "No " : winner;
			var answer = await DisplayAlert (winner + "player wins.", "Play again?", "Yes", "No");
			if (answer)
				Restart ();
			else
				await Navigation.PopToRootAsync (true);
		}

		void UpdateCounters () {
			lblPieces.Text = "White: x" + whitePieces.ToString () + " Black: x" + blackPieces.ToString ();
		}

		void Restart () {
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					Button b = Buttons [i, j];
					b.BackgroundColor = Color.Gray;
					b.IsEnabled = false;
					if (!(i == 3 && j == 3 || i == 3 && j == 4 || i == 4 && j == 3 || i == 4 && j == 4))
						b.BorderRadius = 5;
					grid.Children.Add (b, j, i);
				}
			}
			ChangeButtonColor (3, 3, Buttons [3, 3], Color.White);
			ChangeButtonColor (3, 4, Buttons [3, 4], Color.Black);
			ChangeButtonColor (4, 3, Buttons [4, 3], Color.Black);
			ChangeButtonColor (4, 4, Buttons [4, 4], Color.White);
			whitePieces = 2;
			blackPieces = 2;
			currPlayer = Color.White;
			UpdateCounters ();
		}
	}
}