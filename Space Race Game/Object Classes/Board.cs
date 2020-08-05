using System.Diagnostics;

namespace Object_Classes {
    /// <summary>
    /// Models a game board for Space Race consisting of three different types of squares
    /// 
    /// Ordinary squares, Wormhole squares and Blackhole squares.
    ///     /// landing on a Wormhole or Blackhole square at the end of a player's move 
    /// results in the player moving to another square
    /// 
    /// </summary>

    public static class Board {
        /// <summary>
        /// Models a game board for Space Race consisting of three different types of squares
        /// 
        /// Ordinary squares, Wormhole squares and Blackhole squares.
        /// 
        /// landing on a Wormhole or Blackhole square at the end of a player's move 
        /// results in the player moving to another square
        /// 
        /// 
        /// </summary>

        public const int NUMBER_OF_SQUARES = 56;
        public const int START_SQUARE_NUMBER = 0;
        public const int FINISH_SQUARE_NUMBER = NUMBER_OF_SQUARES - 1;

        private static Square[] squares = new Square[NUMBER_OF_SQUARES];

        public static Square[] Squares {
            get {
                Debug.Assert(squares != null, "squares != null",
                   "The game board has not been instantiated");
                return squares;
            }
        }

        public static Square StartSquare {
            get {
                return squares[START_SQUARE_NUMBER];
            }
        }


        /// <summary>
        ///  Eight Wormhole squares.
        ///  
        /// Each row represents a Wormhole square number, the square to jump forward to and the amount of fuel consumed in that jump.
        /// 
        /// For example {2, 22, 10} is a Wormhole on square 2, jumping to square 22 and using 10 units of fuel
        /// 
        /// </summary>
        private static int[,] wormHoles =
        {
            {2, 22, 10},
            {3, 9, 3},
            {5, 17, 6},
            {12, 24, 6},
            {16, 47, 15},
            {29, 38, 4},
            {40, 51, 5},
            {45, 54, 4}
        };

        /// <summary>
        ///  Eight Blackhole squares.
        ///  
        /// Each row represents a Blackhole square number, the square to jump back to and the amount of fuel consumed in that jump.
        /// 
        /// For example {10, 4, 6} is a Blackhole on square 10, jumping to square 4 and using 6 units of fuel
        /// 
        /// </summary>
        private static int[,] blackHoles =
        {
            {10, 4, 6}, // <- to access 10, it's blackHole[0, 0]
            {26, 8, 18},
            {30, 19, 11},
            {35,11, 24},
            {36, 34, 2},
            {49, 13, 36},
            {52, 41, 11},
            {53, 42, 11}
        };


        /// <summary>
        /// Parameterless Constructor
        /// Initialises a board consisting of a mix of Ordinary Squares,
        ///     Wormhole Squares and Blackhole Squares.
        /// 
        /// Pre:  none
        /// Post: board is constructed
        /// </summary>
        public static void SetUpBoard() {

            // Create the 'start' square where all players will start.
            squares[START_SQUARE_NUMBER] = new Square("Start", START_SQUARE_NUMBER);

            int wormCounter = 0, counter = 0;
            int fuel = 0, destSquare = 0, currentSquare = 0;
            //iterate through Squares array
            for (int position = 1; position < (squares.Length - 1); position++)
            {
                //create BlackHole square if current position in Squares array matches
                    //the first index of any of BlackHole array's elements
                if (counter < blackHoles.GetLength(0) && position == blackHoles[counter, 0])
                {
                    //set blackHole elements to appropriate identifier
                    currentSquare = blackHoles[counter, 0];
                    destSquare = blackHoles[counter, 1];
                    fuel = blackHoles[counter, 2];

                    //create new BlackHole Square with values specified in arrays
                    squares[position] = new BlackholeSquare(position.ToString(), currentSquare, destSquare, fuel);
                    //determine values for next destination square and fuel consumption
                    FindDestSquare(blackHoles, currentSquare, out destSquare, out fuel);

                    counter++;
                }//end creation of blackHole square

                //create wormHole square if current position matches 
                    //first index of any wormHole array elements
                else if (wormCounter < wormHoles.GetLength(0) && position == wormHoles[wormCounter, 0])
                {
                    currentSquare = wormHoles[counter, 0];
                    destSquare = wormHoles[counter, 1];
                    fuel = wormHoles[counter, 2];
                    
                    squares[position] = new WormholeSquare(position.ToString(),
                        (wormHoles[wormCounter, 0]), (wormHoles[wormCounter, 1]), (wormHoles[wormCounter, 2]));
                    FindDestSquare(wormHoles, position, out destSquare, out fuel);

                    wormCounter++;
                }//end creation of wormHole square
                else
                {
                    //if current position doesn't match elements in array, create an ordinary square 
                    squares[position] = new Square(position.ToString(), position);
                }//end creation of ordinary squares
            }//end iteration
            // Create the 'finish' square.
            squares[FINISH_SQUARE_NUMBER] = new Square("Finish", FINISH_SQUARE_NUMBER);

            
        } // end SetUpBoard

        /// <summary>
        /// Finds the destination square and the amount of fuel used for either a 
        /// Wormhole or Blackhole Square.
        /// 
        /// pre: squareNum is either a Wormhole or Blackhole square number
        /// post: destNum and amount are assigned correct values.
        /// </summary>
        /// <param name="holes">a 2D array representing either the Wormholes or Blackholes squares information</param>
        /// <param name="squareNum"> a square number of either a Wormhole or Blackhole square</param>
        /// <param name="destNum"> destination square's number</param>
        /// <param name="amount"> amount of fuel used to jump to the deestination square</param>
        private static void FindDestSquare(int[,] holes, int squareNum, out int destNum, out int amount) {
            //const int start = 0, exit = 1, fuel = 2;
            destNum = 0; amount = 0;

            //iterate through each element in specified array holes - either wormHole or blackHole
            for (int count = 0; count < holes.GetLength(0); count++) {
                
                if (count < holes.GetLength(0) && squareNum == holes[count, 0]) {
                    //set values to destNum and amount
                    destNum = holes[count, 1];
                    amount = holes[count, 2];                   
                }//end if
            }//end for          
        } //end FindDestSquare

    } //end class Board
}