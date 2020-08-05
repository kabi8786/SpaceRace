using System.Drawing;
using System.ComponentModel;
using Object_Classes;


namespace Game_Logic_Class
{
    public static class SpaceRaceGame
    {
        // Minimum and maximum number of players.
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 6;
   
        private static int numberOfPlayers = 2;  //default value for test purposes only 
        public static int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }

        public static string[] names = { "One", "Two", "Three", "Four", "Five", "Six" };  // default values
        
        // Only used in Part B - GUI Implementation, the colours of each player's token
        private static Brush[] playerTokenColours = new Brush[MAX_PLAYERS] { Brushes.Yellow, Brushes.Red,
                                                                       Brushes.Orange, Brushes.White,
                                                                      Brushes.Green, Brushes.DarkViolet};
        /// <summary>
        /// A BindingList is like an array which grows as elements are added to it.
        /// </summary>
        private static BindingList<Player> players = new BindingList<Player>();
        public static BindingList<Player> Players
        {
            get
            {
                return players;
            }
        }

        // The pair of die
        private static Die die1 = new Die(), die2 = new Die();
       

        /// <summary>
        /// Set up the conditions for this game as well as
        ///   creating the required number of players, adding each player 
        ///   to the Binding List and initialize the player's instance variables
        ///   except for playerTokenColour and playerTokenImage in Console implementation.
        ///   
        ///     
        /// Pre:  none
        /// Post:  required number of players have been initialsed for start of a game.
        /// </summary>
        public static void SetUpPlayers() 
        {
            //clear all players currently stored in List
            players.Clear();

            for (int counter = 0; counter < NumberOfPlayers; counter++) {
                //create new player object with name
                Player playerName = new Player(names[counter]);

                //initialise player properties
                playerName.Location = Board.StartSquare;
                playerName.Position = Board.START_SQUARE_NUMBER;
                playerName.RocketFuel = Player.INITIAL_FUEL_AMOUNT;
                playerName.PlayerTokenColour = playerTokenColours[counter];

                //add players to binding list
                players.Add(playerName);

            }                
        }

            /// <summary>
            ///  Plays one round of a game
            /// </summary>
        public static void PlayOneRound() 
        {   
            //each player completes a round
            foreach (Player playerName in players) 
            {
                playerName.Play(die1, die2);
            }
        }

        //reset player's information from previous games
        public static void ResetPlayers() {
            players.Clear();
        }//end ResetPlayers

    }//end SnakesAndLadders
}