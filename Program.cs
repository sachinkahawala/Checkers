using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter tht size of the board");
            string strSizeOfBoard = Console.ReadLine();
            int sizeOfBoard = int.Parse(strSizeOfBoard);
            Board newBoard = new Board(sizeOfBoard,true);
            newBoard.printBoard();
            newBoard.availableMoves("B");
            string selectionString = Console.ReadLine();
            int selection = int.Parse(selectionString);
            newBoard.move(selection);
            newBoard.printBoard();
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
        
    }
    public class Board
    {
        public static Dictionary<int, Tuple<string, int, int, int, int>> movesDictinary;
        public static int sizeOfBoard;
        public static Piece[,] board;
        public static int totalMovesAvailable;
        public static int countOfPieces;
        public Board(int sizeOfBoard1,bool isTest)
        {
            sizeOfBoard = sizeOfBoard1;
            board = new Piece[sizeOfBoard,sizeOfBoard];
            movesDictinary = new Dictionary<int, Tuple<string, int, int, int, int>>();
            totalMovesAvailable = 0;
            if (isTest == false)
            {
                for (int i = 0; i < sizeOfBoard; i++)
                {
                    for (int j = 0; j < sizeOfBoard; j++)
                    {
                        if (i <= sizeOfBoard / 2 - 2)
                        {
                            if ((i + j) % 2 == 1)
                            {
                                countOfPieces++;
                                board[i, j] = new Piece("W", "W" + (countOfPieces).ToString(), i, j, false, false);

                            }
                        }
                        else if (i >= sizeOfBoard / 2 + 1)
                        {
                            if ((i + j) % 2 == 1)
                            {
                                countOfPieces++;
                                board[i, j] = new Piece("B", "B" + (countOfPieces - 12).ToString(), i, j, false, false);
                            }
                        }
                    }

                }
            }
            else
            {
                board[4, 5] = new Piece("B", "B" + 7.ToString(), 4, 5, false, false);
                board[5, 4] = new Piece("W", "W" + 6.ToString(), 5, 4, false, false);
                board[3, 4] = new Piece("W", "W" + 5.ToString(), 3, 4, false, false);
                board[3, 6] = new Piece("W", "W" + 4.ToString(), 3, 6, false, false);
                board[1, 2] = new Piece("W", "W" + 3.ToString(), 1, 2, false, false);
                countOfPieces = 5;
            }
        }
        
        public void printBoard()
        {
            for (int i = -1; i < sizeOfBoard; i++)
            {
                for (int j = -1; j < sizeOfBoard; j++)
                {
                    if (i == -1 && j == -1)
                    {
                        Console.Write(" X \\ Y   ");
                    }
                    else if (i == -1)
                    {
                        
                        Console.Write(" "+(j).ToString()+"     ");
                    }
                    else if (j == -1)
                    {
                        Console.Write(" " + (i).ToString() + "     ");
                    }
                    else if (board[i, j] != null)
                    {
                        
                        Console.Write("|"+String.Format(" {0,-3} ",  board[i, j].returnLable())+"|");
                        
                    }
                    else
                    {
                        Console.Write("|     |");
                    }
                }
                Console.WriteLine();
            }
        }
        public void move(int selection)
        {
            Tuple<string, int, int, int, int> movement= movesDictinary[selection];
            board[movement.Item4, movement.Item5] = board[movement.Item2, movement.Item3];
            board[movement.Item2, movement.Item3] = null;
        }
        public void availableMoves(string colorOfThePlayerOfTheRound)
        {
            Console.WriteLine("Select One of following");
            for (int i = 0; i < sizeOfBoard; i++) {
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    if (board[i, j] != null && board[i, j].returnColor() == colorOfThePlayerOfTheRound)
                    {
                        board[i, j].generateMoves();
                        board[i, j].check4sidesToCapture(board[i, j].getX(), board[i, j].getY(),9999,9999);
                        Console.WriteLine(String.Join(",", board[i, j].returnCaptureArray()));
                        


                    }
                }
            } 
            foreach (var item in movesDictinary)
            {
                Console.WriteLine((item.Key).ToString()+") Move " +item.Value.Item1+" to "+(item.Value.Item4).ToString()+" , "+(item.Value.Item5).ToString());
            }
        }
        public static Dictionary<int, Tuple<string, int, int, int, int>> returnDictionaryOfMoves() {
            return movesDictinary;
        }
        public static int sizeOfTheBoard() {
            return sizeOfBoard;

        }
        public static int getTotalAvailableMoves() {
            return totalMovesAvailable;
        }
        public static void setTotalAvailableMoves() {
            totalMovesAvailable++;
        }
    }
    public class Piece
    {
        string color;
        int positionX;
        int positionY;
        bool captured;
        bool isKing;
        string lable;
        string opponentColor;
        string[] movesList;
        int moveCount;
        List<string> captureArray;
        public Piece(string color, string lable, int positionX, int positionY, bool captured, bool isKing) {
            this.color = color;
            this.positionX = positionX;
            this.positionY = positionY;
            this.captured = captured;
            this.isKing = isKing;
            this.lable = lable;
            movesList = new string[30];
            captureArray = new List<string>();
            moveCount = 0;
            if (color == "W")
                opponentColor = "B";
            else
                opponentColor = "W";
            
        }
        public void generateMoves()
        {
            if (color == "B")
            {
                if (positionX - 1 >= 0)
                {
                    if (0 <= positionY - 1)
                    {
                        if (Board.board[positionX - 1, positionY - 1] == null)
                        {
                            Board.setTotalAvailableMoves();
                            Board.returnDictionaryOfMoves().Add(Board.getTotalAvailableMoves(), new Tuple<string,int, int,int,int>(lable, positionX, positionY, positionX - 1, positionY - 1));
                            movesList[moveCount++] = "Move " + lable + " to " + (positionX - 1).ToString() + "," + (positionY - 1).ToString();
                            
                        }
                    }
                    if (positionY + 1 < Board.sizeOfTheBoard())
                    {
                        if (Board.board[positionX - 1, positionY + 1] == null)
                        {
                            Board.setTotalAvailableMoves();
                            Board.returnDictionaryOfMoves().Add(Board.getTotalAvailableMoves(), new Tuple<string, int, int, int, int>(lable, positionX, positionY, positionX - 1, positionY + 1));
                            movesList[moveCount++] = "Move " + lable + " to " + (positionX - 1).ToString() + "," + (positionY + 1).ToString();
                            
                        }
                    }
                }
            }
            else if(color=="W")
            {
                if (positionX + 1 < Board.sizeOfTheBoard())
                {
                    if (0 <= positionY - 1)
                    {
                        if (Board.board[positionX + 1, positionY - 1] == null)
                        {
                            Board.setTotalAvailableMoves();
                            Board.returnDictionaryOfMoves().Add(Board.getTotalAvailableMoves(), new Tuple<string, int, int, int, int>(lable, positionX, positionY, positionX + 1, positionY - 1));
                            movesList[moveCount++] = "Move " + lable + " to " + (positionX + 1).ToString() + "," + (positionY - 1).ToString();

                        }
                    }
                    if (positionY + 1 < Board.sizeOfTheBoard())
                    {
                        if (Board.board[positionX + 1, positionY + 1] == null)
                        {
                            Board.setTotalAvailableMoves();
                            Board.returnDictionaryOfMoves().Add(Board.getTotalAvailableMoves(), new Tuple<string, int, int, int, int>(lable, positionX, positionY, positionX + 1, positionY + 1));
                            movesList[moveCount++] = "Move " + lable + " to " + (positionX + 1).ToString() + "," + (positionY + 1).ToString();

                        }
                    }
                }
            }
        }
        public void check4sidesToCapture(int pX, int pY , int prevX,int prevY )
        {

            //Console.WriteLine((pX).ToString()+" "+(pY).ToString() + " " + (prevX).ToString() + " " + (prevY).ToString());
            //Console.WriteLine(Board.board[pX - 1, pY - 1] != null && Board.board[pX - 1, pY - 1].color == opponentColor);
            //Console.WriteLine(Board.board[pX - 2, pY - 2]==null);
            //Console.WriteLine((pX - 2)!= prevX && (pY - 2)!= prevY);
            //Console.WriteLine();
            if (!(pX - 2== prevX && pY - 2== prevY )&& (pX - 2)>=0 && (pY - 2)>=0 && Board.board[pX - 1, pY - 1]!=null && Board.board[pX - 1, pY - 1].color == opponentColor && Board.board[pX - 2, pY - 2]==null )
            {
                captureArray.Add("1");
                check4sidesToCapture(pX - 2, pY - 2, pX, pY);
            }
            if (!(pX - 2 == prevX && pY + 2 == prevY) && pX - 2 >= 0 && pY + 2 < Board.sizeOfTheBoard() && Board.board[pX - 1, pY + 1] != null && Board.board[pX - 1, pY + 1].color == opponentColor && Board.board[pX - 2, pY + 2] == null)
            {
                captureArray.Add("2");
                check4sidesToCapture(pX - 2 , pY + 2, pX, pY);
            }
            if (!(pX + 2 == prevX && pY - 2 == prevY) && pX + 2 < Board.sizeOfTheBoard() && pY - 2 >= 0 && Board.board[pX + 1, pY - 1] != null && Board.board[pX + 1, pY - 1].color == opponentColor && Board.board[pX + 2, pY - 2] == null)
            {
                captureArray.Add("4");
                check4sidesToCapture(pX + 2, pY - 2, pX, pY);
            }
            if (!(pX + 2 == prevX && pY + 2 == prevY )&& pX + 2 < Board.sizeOfTheBoard() && pY + 2 < Board.sizeOfTheBoard() && Board.board[pX + 1, pY + 1] != null && Board.board[pX + 1, pY + 1].color == opponentColor && Board.board[pX + 2, pY + 2] == null)
            {
                captureArray.Add("3");
                check4sidesToCapture( pX +2 , pY + 2, pX, pY);
            }
            captureArray.Add("0");

        }
        public List<string> returnCaptureArray()
        {
            return captureArray;
        }
        public int returnMoveCount() {
            return moveCount;
        }
        public string[] returnMoves() {
            return movesList;
        }
        public string returnColor()
        {
            return color;
        }
        public string returnLable()
        {
            return lable;
        }
        public int getX() {
            return positionX;
        }
        public int getY()
        {
            return positionY;
        }

    }
}
