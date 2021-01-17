using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            int moves = 0;
            ChessPiece[,] board = { { new Rook('W'), new Knight('W'), new Bishop('W'), new Queen('W'), new King('W'), new Bishop('W'), new Knight('W'), new Rook('W')},
                                    { new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W')},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'),},
                                    { new Rook('B'), new Knight('B'), new Bishop('B'), new Queen('B'), new King('B'), new Bishop('B'), new Knight('B'), new Rook('B'),}  };
            printBoard(board);
            while (true)
            {

                string[] isValid = askAndCheckNextMove(board, moves);

                bool correctPieceChosen = checkWhoseTurn(isValid[1], moves, board);
                if (correctPieceChosen)
                {
                    string[] Possible = IsPossible(board, isValid[1]);
                    if (Possible[0] == "True")
                    {
                        ChessPiece[,] newBoard = Moving(board, Possible[1], Possible[2]);
                        moves++;
                        if(newBoard[0,0] == null)
                        {
                            string name = moves % 2 == 0 ? "White" : "Black";
                            Console.WriteLine("{0} player WINS", name);
                            return;
                        }
                        printBoard(newBoard);
                    }
                }
            }
        }

        static void printBoard(ChessPiece[,] board)
        {
            Console.WriteLine("");
            Console.WriteLine("  A   B   C   D   E   F   G   H");
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    Console.Write("|");
                    Console.Write(board[i, g] + " ");
                }
                Console.Write("|   " + (i + 1));
                Console.WriteLine("");
                Console.WriteLine("+---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("");
        }
        static string[] askAndCheckNextMove(ChessPiece[,] board, int moves)
        {
            bool wantToMoveValid = false;
            string wantToMove;
            char whoseTurn;
            string[] list = new string[2];
            while (!wantToMoveValid)
            {
                if (moves % 2 == 0)         //Checking if its white's turn
                {
                    Console.WriteLine("It is WHITE's turn\nWhich piece do you want to move and to where?:(ex: 'A1A2')");
                    whoseTurn = 'W';
                }
                if (moves % 2 != 0)          // checking if its black's turn
                {
                    Console.WriteLine("It is BLACK's turn\nWhich piece do you want to move and to where?:(ex: 'A1A2')");
                    whoseTurn = 'B';
                }
                wantToMove = Console.ReadLine();
                if (wantToMove == "")
                    Console.WriteLine("You pressed ENTER without any number");        //If user pressed nothing, looping over again


                else if (wantToMove.Length != 4)
                    Console.WriteLine("You entered less than required, enter which piece you want to move and to where"); // if user entered less than 4 letters, loop over again

                else
                    wantToMoveValid = checkUserInput(wantToMove);       // If user didnt press enter without any value or less than 4 letters, then checking if the letters are legit

                list[0] = wantToMoveValid + "";
                list[1] = wantToMove;
            }
            return list;
        }
        static bool checkUserInput(string userInput)
        {
            string checkLetters = "" + userInput[0] + userInput[2];
            string checkNumbers = "" + userInput[1] + userInput[3];
            checkLetters = checkLetters.ToUpper();
            string availableLetters = "ABCDEFGH";
            string availableNumbers = "12345678";
            bool validLetter = false;
            bool validNumber = false;
            int countValidLetters = 0;
            int countValidNumbers = 0;
            for (int i = 0; i < checkLetters.Length && !validLetter; i++)
            {
                for (int g = 0; g < availableLetters.Length && !validLetter; g++)
                {
                    if (checkLetters[i] == availableLetters[g])
                    {
                        countValidLetters++;
                        g = availableLetters.Length;
                    }
                }
            }

            for (int i = 0; i < checkNumbers.Length && !validNumber; i++)
            {
                for (int g = 0; g < availableNumbers.Length; g++)
                {
                    if (checkNumbers[i] == availableNumbers[g])
                    {
                        countValidNumbers++;
                        g = availableLetters.Length;
                    }
                }
            }
            validLetter = countValidLetters == 2 ? true : false;
            validNumber = countValidNumbers == 2 ? true : false;

            if (validNumber && validLetter)
                return true;
            if (!validLetter)
                Console.WriteLine("You entered an invalid letter");
            if (!validNumber)
                Console.WriteLine("You entered an invalid number");
            return false;
        }
    

    static bool checkWhoseTurn(string wantToMove, int move, ChessPiece[,] board)
        {
            bool checkWhoseTurn = true;
            string movingPiece = "" + wantToMove[0] + wantToMove[1];  // User chosen piece to move
            char whoseTurn = move % 2 == 0 ? 'W' : 'B';   //Is it white's turn or black's turn

            string tempLocation = changeTo(board, movingPiece);
            int column = tempLocation[0] - '0';
            int row = tempLocation[2] - '0';
            if (whoseTurn == 'B' && board[column, row].getColor() == 'W')        // If its blackplayer's turn and a white soldier was picked
            {
                Console.WriteLine("You chose a white soldier and its black player's turn");
                checkWhoseTurn = false;
            }
            if (whoseTurn == 'W' && board[column, row].getColor() == 'B')
            {
                Console.WriteLine("You chose a black soldier and its white player's turn");
                checkWhoseTurn = false;
            }
            return checkWhoseTurn;
        }

        static string[] IsPossible(ChessPiece[,] board, string wantToMove) 
        {
            
            string movingPiece = "" + wantToMove[0] + wantToMove[1];  // User chosen piece to move
            string moveTo = "" + wantToMove[2] + wantToMove[3];
            string pieceLocation = changeTo(board, movingPiece);
            string moveToLocation = changeTo(board, moveTo);
            bool canIt = canDoThatMove(board, pieceLocation, moveToLocation);

            //if (canIt == true)
            //   newBoard = Moving(board, pieceLocation, moveToLocation);
            string[] result = new string[3];
            result[0] = canIt + "";
            result[1] = pieceLocation;
            result[2] = moveToLocation;
            return result;
        }
        static bool canDoThatMove(ChessPiece[,] board, string pieceLocation, string MovetoLocation)
        {
            // board[pieceLocationColumn, pieceLocationRow] is the soldier that user wants to move
            // board[column - 1, row] is the spot that the user wants to move it to.
            int pieceLocationColumn = pieceLocation[0] - '0';
            int pieceLocationRow = pieceLocation[2] - '0';

            int moveToLocationColumn = MovetoLocation[0] - '0';
            int moveToLocationRow = MovetoLocation[2] - '0';
            bool canIt = false;
            //Checking  which piece the user want to move, once detects then goes to its setmove function with
            //the current location minus the desired new location
            if (board[pieceLocationColumn, pieceLocationRow] is Pawn)
                canIt = ((Pawn)board[pieceLocationColumn, pieceLocationRow]).setMove( board, pieceLocationColumn , pieceLocationRow, moveToLocationColumn, moveToLocationRow); // Finding out if the column is more than the pawn can make
            if (board[pieceLocationColumn, pieceLocationRow] is Blank)
                canIt = ((Blank)board[pieceLocationColumn, pieceLocationRow]).setMove( board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow);
            if (board[pieceLocationColumn, pieceLocationRow] is Knight)
                canIt = ((Knight)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow);
            if (board[pieceLocationColumn, pieceLocationRow] is Rook)
                canIt = ((Rook)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow);
            if (board[pieceLocationColumn, pieceLocationRow] is Bishop)
                canIt = ((Bishop)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow);
            if (board[pieceLocationColumn, pieceLocationRow] is King)
                canIt = ((King)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow);
            if (board[pieceLocationColumn, pieceLocationRow] is Queen)
                canIt = ((Queen)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow);

            return canIt;
        }
        static ChessPiece[,] Moving(ChessPiece[,] board, string pieceLocation, string moveToLocation)
        {
            int pieceLocationColumn = pieceLocation[0] - '0';  // The piece location before moves
            int pieceLocationRow = pieceLocation[2] - '0';
            int column = moveToLocation[0] - '0';
            int row = moveToLocation[2] - '0';
            string classname = board[pieceLocationColumn, pieceLocationRow].GetType().Name;
            ChessPiece tempLocationPieceBeforeMoves = null;
            if (classname == "Pawn")
                tempLocationPieceBeforeMoves = (Pawn)board[pieceLocationColumn, pieceLocationRow];

            if (classname == "Blank")
                tempLocationPieceBeforeMoves = (Blank)board[pieceLocationColumn, pieceLocationRow];

            if (classname == "Knight")
                tempLocationPieceBeforeMoves = (Knight)board[pieceLocationColumn, pieceLocationRow];

            if (classname == "Rook")
                tempLocationPieceBeforeMoves = (Rook)board[pieceLocationColumn, pieceLocationRow];

            if (classname == "Bishop")
                tempLocationPieceBeforeMoves = (Bishop)board[pieceLocationColumn, pieceLocationRow];

            if (classname == "King")
                tempLocationPieceBeforeMoves = (King)board[pieceLocationColumn, pieceLocationRow];

            if (classname == "Queen")
                tempLocationPieceBeforeMoves = (Queen)board[pieceLocationColumn, pieceLocationRow];

            // board[pieceLocationColumn, pieceLocationRow] is the soldier that user wants to move
            // board[column - 1, row] is the spot that the user wants to move it to.

            string classname2 = board[column, row].GetType().Name;
            ChessPiece tempNewLocation = null;
            if (classname2 == "Pawn")
                tempNewLocation = (Pawn)board[column, row];

            else if (classname2 == "Blank")
                tempNewLocation = (Blank)board[column, row];

            else if (classname2 == "Knight")
                tempNewLocation = (Knight)board[column, row];

            else if (classname2 == "Rook")
                tempNewLocation = (Rook)board[column, row];

            else if (classname2 == "Bishop")
                tempNewLocation = (Bishop)board[column, row];

            else if (classname2 == "King")
                tempNewLocation = (King)board[column, row];

            else if (classname2 == "Queen")
                tempNewLocation = (Queen)board[column, row];

            if (board[column, row] is Blank)
               board[pieceLocationColumn, pieceLocationRow] = tempNewLocation;

            else if(classname == "King" && board[pieceLocationColumn, pieceLocationRow].getColor() == board[column, row].getColor() && classname2 == "Rook")
            {
                board[pieceLocationColumn, pieceLocationRow] = tempNewLocation;
            }

            else
            {
               Console.WriteLine("You ate his " + board[column, row].GetType().Name);
               board[pieceLocationColumn, pieceLocationRow] = new Blank('E');
               if (board[column, row].GetType().Name == "King")
               {
                   board[0, 0] = null;
               }
            }
            board[column, row] = tempLocationPieceBeforeMoves;

            
            return board;
        }

        static string changeTo(ChessPiece[,] board, string moveTo)   //7|1
        {
            string tempString = moveTo.ToUpper();
            int column = (char)tempString[1] - '0';     // converts the number as char to int
            int row = 0;
            switch ((char)tempString[0])
            {
                case 'A':
                    row = 0;
                    break;
                case 'B':
                    row = 1;
                    break;
                case 'C':
                    row = 2;
                    break;
                case 'D':
                    row = 3;
                    break;
                case 'E':
                    row = 4;
                    break;
                case 'F':
                    row = 5;
                    break;
                case 'G':
                    row = 6;
                    break;
                case 'H':
                    row = 7;
                    break;
                default:
                    Console.WriteLine("BUG IN CHANGE TO");
                    break;
            }
            string moveToLocation = "" + (column - 1) + "|" + row;
            return moveToLocation;
        }
    }
    interface Piece
    {
        void setColor(char color);
        void setName(char color);
        bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow);
        int getTotalMoves();

    }

    class ChessPiece
    {
        char color;
        public ChessPiece(char color)
        {
            setColor(color);
        }

        public void setColor(char color)
        {
            if (color == 'W' || color == 'B' || color == 'E')
            {
                this.color = color;
            }
            else
                Console.WriteLine("You entered an invalid color, choose W for white or B for black");
            return;
        }
        public char getColor()
        {
            return color;
        }
    }
    class Blank : ChessPiece, Piece
    {
        string name;
        public Blank(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board,int column, int move, int row, int colum)
        {
            Console.WriteLine("Empty spot cannot be chosen");
            return false;
        }

        public void setName(char color)
        {
            name = color == 'E' ? "EE" : "";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            int totalMoves = 0;
            return totalMoves;
        }
    }
    class Pawn : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        public Pawn(char color) : base(color)
        {
            setName(color);
        }
        public void setName(char color)
        {
            this.name = color == 'W' ? "WP" : "BP";
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow) // 1 0
        {
            char currentPieceColor = this.getColor();
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            if(currentMinusNextRow != 0 && (currentMinusNextRow == -1 || currentMinusNextRow == 1) && (currentMinusNextColumn == -1 || currentMinusNextColumn == 1))
            {
                if (board[nextColumn, nextRow] is Blank)
                {
                    Console.WriteLine("Pawn can only eat that direction, other than that only move forward");
                    return false;
                }
                else { totalMoves++; return true; }

            }
            else if(totalMoves > 0 && (currentMinusNextRow > 1 || currentMinusNextRow < -1 || currentMinusNextColumn > 1 || currentMinusNextColumn < -1))
            {
                Console.WriteLine("Pawn can only eat one square diagonally");
                return false;
            }
            if (totalMoves == 0)
            {
                if (currentColumn > nextColumn) 
                {
                    bool anyPieceBlocking = false;
                    for (int i = currentColumn - 1; !anyPieceBlocking && i >= nextColumn; i--)
                    {
                        if(!(board[i, currentRow] is Blank))
                        {
                            anyPieceBlocking = true;
                            Console.WriteLine("There is other piece blocking you from doing that move");
                            return false;
                        }
                    }
                }
                if(currentColumn < nextColumn)
                {
                    bool anyPieceBlocking = false;
                    for (int i = currentColumn + 1; !anyPieceBlocking && i <= nextColumn; i++)
                    {
                        if (!(board[i, currentRow] is Blank))
                        {
                            anyPieceBlocking = true;
                            Console.WriteLine("There is other piece blocking you from doing that move");
                            return false;
                        }
                    }
                }
                if (currentPieceColor == 'W')
                    if (currentMinusNextRow == 0 && (currentMinusNextColumn == 1 || currentMinusNextColumn == 2))
                    {
                        totalMoves++;
                        return true;
                    }
                if (currentPieceColor == 'B')
                    if (currentMinusNextRow == 0 && (currentMinusNextColumn == -1 || currentMinusNextColumn == -2))
                    {
                        totalMoves++;
                        return true;
                    }
            }
            if(totalMoves > 0)
            {
                if (!(board[nextColumn, nextRow] is Blank))
                {
                    Console.WriteLine("Cannot do that move, there is other piece blocking you");
                    return false;
                }
                if (currentPieceColor == 'W')
                    if (currentMinusNextColumn == 1 && currentMinusNextRow == 0)
                    {
                        totalMoves++;
                        return true;
                    }
                if (currentPieceColor == 'B')
                    if (currentMinusNextColumn == -1 && currentMinusNextRow == 0)
                    {
                        totalMoves++;
                        return true;
                    }
            }
            if(totalMoves == 0)
                Console.WriteLine("Pawn can move 1 or two squares in its first move");
            if (totalMoves > 0)
                Console.WriteLine("Pawn can only move one square");
            return false;

        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }

    }
    
    class Knight : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        public Knight(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            if (currentMinusNextColumn == 1 && currentMinusNextRow == -2||currentMinusNextColumn == 1 && currentMinusNextRow == 2 ||currentMinusNextColumn == -2 && currentMinusNextRow == 1 || currentMinusNextColumn == 2 && currentMinusNextRow == 1 || currentMinusNextColumn == 2 && currentMinusNextRow == -1 || currentMinusNextColumn == -2 && currentMinusNextRow == -1 || currentMinusNextColumn == -1 && currentMinusNextRow == 2 || currentMinusNextColumn == -1 && currentMinusNextRow == -2)
            {
                totalMoves++;
                return true;
            }
            Console.WriteLine("Knight cant do that move, it can only move in L shape");
            return false;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WN" : "BN";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class Rook : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        public Rook(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            if (currentColumn > nextColumn && currentMinusNextRow == 0)
            {
                for (int i = currentColumn - 1; isPossible &&  i > nextColumn;i--)
                {
                    if((board[i, currentRow] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentRow > nextRow && currentMinusNextColumn == 0)
            {
                for(int i = currentRow - 1;isPossible && i > nextRow; i--)
                {
                    if(!(board[currentColumn, i] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentColumn < nextColumn && currentMinusNextRow == 0)
            {
                for(int i = currentColumn + 1;isPossible && i < nextColumn; i++)
                {
                    if(!(board[i, currentRow] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentRow > nextRow && currentMinusNextColumn == 0)
            {
                for(int i = currentRow - 1; i > nextRow; i--)
                {
                    if(!(board[currentColumn, i] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }

            if(!(isPossible))
                Console.WriteLine("Rook cant move over other piece");

            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WR" : "BR";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class Bishop : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        public Bishop(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            // --------------------------------------------------CHECKING FOR BLACK--------------------------------------
            if (currentColumn > nextColumn && currentRow > nextRow)
            {
                for (int i = currentColumn - 1, g = currentRow - 1; isPossible && i > nextColumn && g > nextRow; i--, g--)
                {
                    if (!(board[i, g] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentColumn > nextColumn && currentRow < nextRow)
            {
                for(int i = currentColumn - 1, g = currentRow + 1; isPossible && i > nextColumn && g < nextRow; i--, g++)
                {
                    if(!(board[i, g] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            //----------------------------------------------CHECKING FOR WHITE ----------------------------
            if (currentColumn < nextColumn && currentRow < nextRow)
            {
                for (int i = currentColumn + 1, g = currentRow + 1; isPossible && i < nextColumn && g < nextRow; i++, g++)
                {
                    if (!(board[i, g] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentColumn < nextColumn && currentRow > nextRow)
            {
                for (int i = currentColumn + 1, g = currentRow - 1; isPossible && i < nextColumn && g > nextRow; i++, g--)
                {
                    if (!(board[i, g] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            //------------------
            if (isPossible)
            {
                int tempNextColumn = currentMinusNextColumn > 0 ? currentMinusNextColumn : -(currentMinusNextColumn);
                int tempNextRow = currentMinusNextRow > 0 ? currentMinusNextRow : -(currentMinusNextRow);
                if(!(tempNextColumn == tempNextRow))
                {
                    Console.WriteLine("Bishop can only move any number of squares diagonally");
                    isPossible = false;
                    return isPossible;
                }
            }

            if (!(isPossible))
            {
                Console.WriteLine("Bishop cannot go over other piece");
            }
            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WB" : "BB";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class King : ChessPiece, Piece
    {
        int totalMoves;
        string name;
        public King(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            if(board[nextColumn, nextRow] is Rook && board[nextColumn, nextRow].getColor() == this.getColor())
            {
                if(currentRow > nextRow && currentMinusNextColumn == 0)
                {
                    for(int i = currentRow - 1; isPossible && i > nextRow; i--)
                    {
                        if(!(board[currentColumn, i] is Blank))
                        {
                            isPossible = false;
                            Console.WriteLine("Castling isn't possible, there are pieces inbetween");
                            return isPossible;
                        }
                    }
                }
                if(currentRow < nextRow && currentMinusNextColumn == 0)
                {
                    for(int i = currentRow + 1; isPossible && i < nextRow; i++)
                    {
                        if(!(board[currentColumn, i] is Blank))
                        {
                            isPossible = false;
                            Console.WriteLine("Castling isn't possible, there are pieces inbetween");
                            return isPossible;
                        }
                    }
                }


                if(((Rook)board[nextColumn, nextRow]).getTotalMoves() == 0)
                {
                    if(this.getTotalMoves() == 0)
                    {
                        isPossible = true;
                        totalMoves++;
                        return isPossible;
                    }
                    else
                    {
                        Console.WriteLine("King already moved, castling is only possible if it was its first move");
                        isPossible = false;
                        return isPossible;
                    }
                }
                else
                {
                    Console.WriteLine("Rook already moved, castling is only possible if it was its first move");
                    isPossible = false;
                    return isPossible;
                }
            }
            
            if (currentMinusNextRow > 1 || currentMinusNextRow < -1 || currentMinusNextColumn > 1 || currentMinusNextColumn < -1)
            {
                Console.WriteLine("King can only move one square in any direction");
                isPossible = false;
            }
            if(isPossible && board[nextColumn, nextRow].getColor() != this.getColor())
            {
                totalMoves++;
                isPossible = true;
                return isPossible;
            }
            if (isPossible && !(board[nextColumn, nextRow] is Blank))
            {
                Console.WriteLine("King cannot go on top of other piece");
                isPossible = false;
            }


            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WK" : "BK";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class Queen : ChessPiece, Piece
    {
        string name;
        int totalMoves;
        public Queen(char color) : base(color)
        {
            setName(color);
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }

        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            bool diagonally = false;
            bool leftright = false;
            bool updown = false;
            char color = this.getColor();

            if (!diagonally)
            {
                int tempNextColumn = currentMinusNextColumn > 0 ? currentMinusNextColumn : -(currentMinusNextColumn);
                int tempNextRow = currentMinusNextRow > 0 ? currentMinusNextRow : -(currentMinusNextRow);
                if (tempNextColumn == tempNextRow)
                {
                    diagonally = true;
                }
            }
            if (!leftright)
            {
                if(currentMinusNextColumn == 0)
                {
                    leftright = true;
                }
            }
            if (!updown)
            {
                if(currentMinusNextRow == 0)
                {
                    updown = true;
                }
            }
            if(!diagonally && !leftright && !updown)
            {
                Console.WriteLine("Queen can move diagonally, left, right bottom up only");
                isPossible = false;
                return isPossible;
            }

            if (board[nextColumn, nextRow].getColor() == this.getColor())
            {
                Console.WriteLine("You can't move your queen on top of your other piece");
                return false;
            }
            if (color == 'B')
            {
                if (diagonally)
                {
                    if (currentColumn > nextColumn && currentRow < nextRow)
                    {
                        for (int i = currentColumn - 1, g = currentRow + 1; isPossible && i > nextColumn && g < currentRow; i--, g++)
                        {
                            if (!(board[i, g] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                    if (currentColumn > nextColumn && currentRow > nextRow)
                    {
                        for (int i = currentColumn - 1, g = currentRow - 1; isPossible && i > nextColumn && g > nextRow; i--, g--)
                        {
                            if (!(board[i, g] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                }
                //--
                if (leftright)
                {
                    if (currentRow < nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow + 1; isPossible && i < nextRow; i++)
                        {
                            if (!(board[currentColumn, i] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                    if (currentRow > nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow - 1; isPossible && i > nextRow; i--)
                        {
                            if (!(board[currentColumn, i] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                }
                if (updown)
                {
                    if (currentColumn < nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn + 1; isPossible && i < nextColumn; i++)
                        {
                            if (!(board[i, currentRow] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                    if (currentColumn > nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn - 1; isPossible && i > nextColumn; i--)
                        {
                            if (!(board[i, currentRow] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                }
                
                //-----
            }
            if(color == 'W')
            {
                if (diagonally)
                {
                    if (currentColumn < nextColumn && currentRow < nextRow)
                    {
                        for (int i = currentColumn + 1, g = currentRow + 1; i < nextColumn && g < currentRow; i++, g++)
                        {
                            if (!(board[i, g] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                    if (currentColumn < nextColumn && currentRow > nextRow)
                    {
                        for (int i = currentColumn + 1, g = currentRow - 1; !diagonally && i < nextColumn && g > nextRow; i++, g--)
                        {
                            if (!(board[i, g] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                }
                if (leftright)
                {
                    if (currentRow < nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow + 1; isPossible && i < nextRow; i++)
                        {
                            if (!(board[currentColumn, i] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                    if (currentRow > nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow - 1; isPossible && i > nextRow; i--)
                        {
                            if (!(board[currentColumn, i] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                }
                if (updown)
                {
                    if (currentColumn < nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn + 1; isPossible && i < nextColumn; i++)
                        {
                            if (!(board[i, currentRow] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                    if (currentColumn > nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn - 1; isPossible && i > nextColumn; i--)
                        {
                            if (!(board[i, currentRow] is Blank))
                            {
                                Console.WriteLine("Queen cannot go over other piece");
                                isPossible = false;
                            }
                        }
                    }
                }
            }
            if (isPossible)
                totalMoves++;
            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WQ" : "BQ";
        }
        public override string ToString()
        {
            return name;
        }
    }
}
