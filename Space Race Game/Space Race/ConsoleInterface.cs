using System;
//DO NOT DELETE the two following using statements *********************************
using Game_Logic_Class;
using Object_Classes;


namespace Space_Race
{
    class Console_Class
    {
        /// <summary>
        /// Algorithm below currently plays only one game
        /// 
        /// when have this working correctly, add the abilty for the user to 
        /// play more than 1 game if they choose to do so.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DisplayIntroductionMessage();
            Board.SetUpBoard();

            bool playGame = true;
            //until user doesn't want to play another game
            while (playGame) {
                //Determine and create specified number of players and initialise players
                NumPlayers();
                SpaceRaceGame.SetUpPlayers();

                bool endGame = false;
                //until the game ends
                while (endGame != true) {
                    Console.WriteLine("\nPress enter to play a round...");
                    Console.ReadLine();

                    SpaceRaceGame.PlayOneRound();

                    foreach (Player currentPlayer in SpaceRaceGame.Players) {
                        if (currentPlayer.Position >= Board.FINISH_SQUARE_NUMBER) {
                            //still want to display all details before stating who won.
                            Console.WriteLine("\t{0} on square {1} with {2} yottawott of power remaining.",
                                    currentPlayer.Name, currentPlayer.Position, currentPlayer.RocketFuel);
                            endGame = true;
                        }//end AtFinish check if
                        else {
                            if (currentPlayer.RocketFuel == 0) {
                                Console.WriteLine("\t{0} at square {1} has run out of fuel.",
                                    currentPlayer.Name, currentPlayer.Position);
                            } else {
                                Console.WriteLine("\t{0} on square {1} with {2} yottawott of power remaining.",
                                    currentPlayer.Name, currentPlayer.Position, currentPlayer.RocketFuel);
                            }//end write current player's info if still playing
                        }//end if rocket fuel is finished
                    }//end foreach
                }//end while        

                //determine who has finished
                Console.WriteLine("\nThe following player(s) finished the game\n");
                FinishedPlayers();

                //output all players details
                Console.WriteLine("\nIndividual players finished with their remaining fuel at the locations specified.");
                PlayerInfo();

                //prompt player if they want to play another game
                playGame = PlayAgain();
            }
            PressEnter(); //terminates program
        }//end Main

        //Determine number of players for the current game
        static void NumPlayers()
        {
            int numPlayers;
            bool success = true;
            bool playerInput = false;
            Console.WriteLine("This game is for 2 to 6 players.");
            //prompt number of players
            while (!playerInput) {
                Console.Write("How many players (2 - 6): ");
                success = int.TryParse(Console.ReadLine(), out numPlayers);
                if (success && numPlayers >= 2 && numPlayers <= 6) {
                    SpaceRaceGame.NumberOfPlayers = numPlayers;
                    playerInput = true;
                } else {
                    Console.Write("Error: invalid number of players entered.");
                }
                
            }
        }//end of NumPlayers 

        //prompt user to play another game
        static bool PlayAgain()
        {
            string playAgain;
            Console.Write("\nPress Enter key to continue...");
            Console.ReadLine();

            Console.Write("\n\n\nPlay Again? (Y or N): ");
            playAgain = Console.ReadLine();

            if (playAgain == "Y" || playAgain == "y")
            {
                //clear Players binding list to play a new game
                SpaceRaceGame.ResetPlayers();
                return true;
            }
            else
            {
                Console.Write("\n\nThanks for playing Space Race.\n");
                return false;
            }
        }//end playAgain method

        //display who has won the game
        static void FinishedPlayers() {
            foreach(Player FinishedPlayer in SpaceRaceGame.Players) {
                if (FinishedPlayer.Position == Board.FINISH_SQUARE_NUMBER) {
                    Console.Write("\t" + FinishedPlayer.Name + "\n"); 
                }
            }
        }//end finishedPlayers

        //outputs player's name, current fuel and position at the end of the game
        static void PlayerInfo() {
            foreach(Player players in SpaceRaceGame.Players) {
                Console.WriteLine("\t {0} with {1} yattowatt of power at square {2}",
                        players.Name, players.RocketFuel, players.Position);
            }
        }
       

        /// <summary>
        /// Display a welcome message to the console
        /// Pre:    none.
        /// Post:   A welcome message is displayed to the console.
        /// </summary>
        static void DisplayIntroductionMessage()
        {
            Console.WriteLine("Welcome to Space Race.\n");
        } //end DisplayIntroductionMessage

        /// <summary>
        /// Displays a prompt and waits for a keypress.
        /// Pre:  none
        /// Post: a key has been pressed.
        /// </summary>
        static void PressEnter()
        {
            Console.Write("\nPress Enter to terminate program ...");
            Console.ReadLine();
        } // end PressAny



    }//end Console class
}
