using System;
//  Uncomment  this using statement after you have remove the large Block Comment below 
using System.Drawing;
using System.Windows.Forms;
using Game_Logic_Class;
//  Uncomment  this using statement when you declare any object from Object Classes, eg Board,Square etc.
using Object_Classes;

namespace GUI_Class
{
    public partial class SpaceRaceForm : Form
    {
        // The numbers of rows and columns on the screen.
        const int NUM_OF_ROWS = 7;
        const int NUM_OF_COLUMNS = 8;

        //Global variable to determine whether to do single step or not
        bool singleStep = false;
        

        // When we update what's on the screen, we show the movement of a player 
        // by removing them from their old square and adding them to their new square.
        // This enum makes it clear that we need to do both.
        enum TypeOfGuiUpdate { AddPlayer, RemovePlayer };


        public SpaceRaceForm()
        {
            InitializeComponent();

             Board.SetUpBoard();
             ResizeGUIGameBoard();
             SetUpGUIGameBoard();
             SetupPlayersDataGridView();
             DetermineNumberOfPlayers();
             SpaceRaceGame.SetUpPlayers();
             PrepareToPlay();
        }


        /// <summary>
        /// Handle the Exit button being clicked.
        /// Pre:  the Exit button is clicked.
        /// Post: the game is terminated immediately
        /// </summary>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        
                // <summary>
                // Resizes the entire form, so that the individual squares have their correct size, 
                // as specified by SquareControl.SQUARE_SIZE.  
                // This method allows us to set the entire form's size to approximately correct value 
                // when using Visual Studio's Designer, rather than having to get its size correct to the last pixel.
                // Pre:  none.
                // Post: the board has the correct size.
                // </summary>
        private void ResizeGUIGameBoard()
        {
            const int SQUARE_SIZE = SquareControl.SQUARE_SIZE;
            int currentHeight = tableLayoutPanel.Size.Height;
            int currentWidth = tableLayoutPanel.Size.Width;
            int desiredHeight = SQUARE_SIZE * NUM_OF_ROWS;
            int desiredWidth = SQUARE_SIZE * NUM_OF_COLUMNS;
            int increaseInHeight = desiredHeight - currentHeight;
            int increaseInWidth = desiredWidth - currentWidth;
            this.Size += new Size(increaseInWidth, increaseInHeight);
            tableLayoutPanel.Size = new Size(desiredWidth, desiredHeight);
        }// ResizeGUIGameBoard


                // <summary>
                // Creates a SquareControl for each square and adds it to the appropriate square of the tableLayoutPanel.
                // Pre:  none.
                // Post: the tableLayoutPanel contains all the SquareControl objects for displaying the board.
                // </summary>
        private void SetUpGUIGameBoard()
        {
            for (int squareNum = Board.START_SQUARE_NUMBER; squareNum <= Board.FINISH_SQUARE_NUMBER; squareNum++)
            {
                Square square = Board.Squares[squareNum];
                SquareControl squareControl = new SquareControl(square, SpaceRaceGame.Players);
                AddControlToTableLayoutPanel(squareControl, squareNum);
            }//endfor

        }// end SetupGameBoard

        private void AddControlToTableLayoutPanel(Control control, int squareNum)
        {
            int screenRow = 0;
            int screenCol = 0;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            tableLayoutPanel.Controls.Add(control, screenCol, screenRow);
        }// end Add Control


                // <summary>
                // For a given square number, tells you the corresponding row and column number
                // on the TableLayoutPanel.
                // Pre:  none.
                // Post: returns the row and column numbers, via "out" parameters.
                // </summary>
                // <param name="squareNumber">The input square number.</param>
                // <param name="rowNumber">The output row number.</param>
                // <param name="columnNumber">The output column number.</param>
        private static void MapSquareNumToScreenRowAndColumn(int squareNum, out int screenRow, out int screenCol)
        {
            //Calculate both screenRow and screenCol at once
                //screenCol = squareNum/totalColumns
                //screenRow = totalRows - (screenCol)
            screenRow = NUM_OF_ROWS - 1 - Math.DivRem(squareNum, NUM_OF_COLUMNS, out screenCol);

            //if the column is odd
            if ((screenRow % 2 != 0)) {
                screenCol = NUM_OF_COLUMNS - 1 - screenCol;
            }

        }//end MapSquareNumToScreenRowAndColumn


        private void SetupPlayersDataGridView()
        {
            //Stop the playersDataGridView from using all Player columns.
            playersDataGridView.AutoGenerateColumns = false;
            //Tell the playersDataGridView what its real source of data is.
            playersDataGridView.DataSource = SpaceRaceGame.Players;

        }// end SetUpPlayersDataGridView
        


        /// <summary>
        /// Obtains the current "selected item" from the ComboBox
        ///  and
        ///  sets the NumberOfPlayers in the SpaceRaceGame class.
        ///  Pre: none
        ///  Post: NumberOfPlayers in SpaceRaceGame class has been updated
        /// </summary>
        private void DetermineNumberOfPlayers()
        {
            //clear player list in Game logic class
            SpaceRaceGame.Players.Clear();
 
            // Store the SelectedItem property of the ComboBox in a string
            string numPlayers = (string)comboBox1.SelectedItem;

            // Parse string to a number
            int numberPlayers = int.Parse(numPlayers);

            // Set the NumberOfPlayers in the SpaceRaceGame class to that number
            SpaceRaceGame.NumberOfPlayers = numberPlayers;

        }//end DetermineNumberOfPlayers

        /// <summary>
        /// The players' tokens are placed on the Start square
        /// </summary>
        private void PrepareToPlay()
        {
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);

        }//end PrepareToPlay()


        /// <summary>
        /// Tells you which SquareControl object is associated with a given square number.
        /// Pre:  a valid squareNumber is specified; and
        ///       the tableLayoutPanel is properly constructed.
        /// Post: the SquareControl object associated with the square number is returned.
        /// </summary>
        /// <param name="squareNumber">The square number.</param>
        /// <returns>Returns the SquareControl object associated with the square number.</returns>
        private SquareControl SquareControlAt(int squareNum)
        {
            int screenRow;
            int screenCol;

            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            return (SquareControl)tableLayoutPanel.GetControlFromPosition(screenCol, screenRow);
        }


        /// <summary>
        /// Tells you the current square number of a given player.
        /// Pre:  a valid playerNumber is specified.
        /// Post: the square number of the player is returned.
        /// </summary>
        /// <param name="playerNumber">The player number.</param>
        /// <returns>Returns the square number of the player.</returns>
        private int GetSquareNumberOfPlayer(int playerNumber)
        {
            int playerCounter = 0, playerSquare = 0;
            foreach (Player currentPlayer in SpaceRaceGame.Players) {
                if (playerCounter == playerNumber) {
                    playerSquare = currentPlayer.Position;
                    break;
                } else {
                    playerCounter++;
                }
            }
            return playerSquare;
        }//end GetSquareNumberOfPlayer


        /// <summary>
        /// When the SquareControl objects are updated (when players move to a new square),
        /// the board's TableLayoutPanel is not updated immediately.  
        /// Each time that players move, this method must be called so that the board's TableLayoutPanel 
        /// is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the board's TableLayoutPanel shows the latest information 
        ///       from the collection of SquareControl objects in the TableLayoutPanel.
        /// </summary>
        private void RefreshBoardTablePanelLayout()
        {
            // Uncomment the following line once you've added the tableLayoutPanel to your form.
            tableLayoutPanel.Invalidate(true);
        }

        /// <summary>
        /// When the Player objects are updated (location, etc),
        /// the players DataGridView is not updated immediately.  
        /// Each time that those player objects are updated, this method must be called 
        /// so that the players DataGridView is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the players DataGridView shows the latest information 
        ///       from the collection of Player objects in the SpaceRaceGame.
        /// </summary>
        private void UpdatesPlayersDataGridView()
        {
            SpaceRaceGame.Players.ResetBindings();
        }

        /// <summary>
        /// At several places in the program's code, it is necessary to update the GUI board,
        /// so that player's tokens are removed from their old squares
        /// or added to their new squares. E.g. at the end of a round of play or 
        /// when the Reset button has been clicked.
        /// 
        /// Moving all players from their old to their new squares requires this method to be called twice: 
        /// once with the parameter typeOfGuiUpdate set to RemovePlayer, and once with it set to AddPlayer.
        /// In between those two calls, the players locations must be changed. 
        /// Otherwise, you won't see any change on the screen.
        /// 
        /// Pre:  the Players objects in the SpaceRaceGame have each players' current locations
        /// Post: the GUI board is updated to match 
        /// </summary>
        private void UpdatePlayersGuiLocations(TypeOfGuiUpdate typeOfGuiUpdate)
        {
            int playerNo = 0;
            
            foreach(Player currentPlayer in SpaceRaceGame.Players) {
                if (currentPlayer.RocketFuel != 0) {
                    int squareNum = GetSquareNumberOfPlayer(playerNo);
                    SquareControl controlObject = SquareControlAt(squareNum);
                    //called to move player to their new space
                    if (typeOfGuiUpdate == TypeOfGuiUpdate.AddPlayer) {
                        controlObject.ContainsPlayers[playerNo] = true;
                    } else {
                        //called to remove player from their current space
                        if (typeOfGuiUpdate == TypeOfGuiUpdate.RemovePlayer) {
                            controlObject.ContainsPlayers[playerNo] = false;
                        }
                    }
                }else {
                    //still show where the player with no fuel is
                    int squareNum = GetSquareNumberOfPlayer(playerNo);
                    SquareControl currentLocation = SquareControlAt(squareNum);
                    currentLocation.ContainsPlayers[playerNo] = true;
                }
                playerNo++;
            }//end of foreach loop

            
            RefreshBoardTablePanelLayout();//must be the last line in this method. Do not put inside above loop.
        } //end UpdatePlayersGuiLocations


        private void rollBtn_Click(object sender, EventArgs e) {
            //start of turn
            playersDataGridView.Enabled = false;
            exitButton.Enabled = true;

            //determine whether to move tokens individually or not
            if (singleStep == false) {
                MultiStep();
            } else {
                //SingleStep();
            }
            
            //check if player has reached the final square or if they've run out of fuel
            foreach(Player player in SpaceRaceGame.Players) {
                if (player.Position == Board.FINISH_SQUARE_NUMBER) {
                    MessageBox.Show(player.Name + " has won the Space Race!");
                    rollBtn.Enabled = false;
                }
                if (player.RocketFuel == 0) {
                    MessageBox.Show(player.Name + " has no more rocket fuel.");
                }
            }

            //end of turn
            resetBtn.Enabled = true;
        }

        //All tokens move at once per turn
        private void MultiStep() {
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            SpaceRaceGame.PlayOneRound();
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            UpdatesPlayersDataGridView();
        }

        //Enabling Single Step Mode
        private void yesBtn_Click(object sender, EventArgs e) {
            comboBox1.Enabled = false;
            PrepareToPlay();
            rollBtn.Enabled = true;
            groupBox1.Enabled = false;
            singleStep = true;
            MessageBox.Show("Sorry, single step has not been implemented yet.");
        }
         
        //Enabling Multi-step mode, disabiling controllers and enabling Roll Dice button
        private void noBtn_Click(object sender, EventArgs e) {
            comboBox1.Enabled = false;
            PrepareToPlay();
            rollBtn.Enabled = true;
            groupBox1.Enabled = false;

            singleStep = false;
        }

        //Resets and starts a new game
        private void resetBtn_Click(object sender, EventArgs e) {
            //Move tokens back to starting square
            PrepareToPlay();

            //resetting controllers
            playersDataGridView.Enabled = true;
            comboBox1.Enabled = true;
            groupBox1.Enabled = true;
            rollBtn.Enabled = false;
        }

    }// end class
}
