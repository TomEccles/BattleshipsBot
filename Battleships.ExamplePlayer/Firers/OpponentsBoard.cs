using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.ExamplePlayer
{
    public class OpponentsBoard
    {
        public static int side = 10;
        internal SquareState[,] board;
        private List<int> remainingShips;

        public OpponentsBoard()
        {
            board = new SquareState[side, side];
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    board[i, j] = SquareState.Unknown;
                }
            }
            remainingShips = new List<int> { 5, 4, 3, 3, 2 };
        }

        internal double numberFitting(Square square)
        {
            int fits = 0;
            foreach (int length in remainingShips)
            {
                for (int displacement = 0; displacement < length; displacement++)
                {
                    if (fitsHorizontally(square, length, displacement)) fits += 1;
                    if (fitsVertically(square, length, displacement)) fits += 1;
                }
            }
            return fits;
        }

        private bool fitsVertically(Square square, int length, int displacement)
        {
            int squaresBelow = displacement;
            int squaresAbove = length - displacement - 1;
            if (!square.hasDown(squaresBelow)) return false;
            if (!square.hasUp(squaresAbove)) return false;
            for (int i = 0; i <= squaresBelow; i++)
            {
                if (state(square.down(i)) != SquareState.Unknown) return false;
            }
            for (int i = 1; i <= squaresAbove; i++)
            {
                if (state(square.up(i)) != SquareState.Unknown) return false;
            }
            return true;
        }

        private bool fitsHorizontally(Square square, int length, int displacement)
        {
            int squaresToLeft = displacement;
            int squaresToRight = length - displacement - 1;
            if (!square.hasLeft(squaresToLeft)) return false;
            if (!square.hasRight(squaresToRight)) return false;
            for (int i = 0; i <= squaresToLeft; i++)
            {
                if (state(square.left(i)) != SquareState.Unknown) return false;
            }
            for (int i = 1; i <= squaresToRight; i++)
            {
                if (state(square.right(i)) != SquareState.Unknown) return false;
            }
            return true;
        }

        internal void addMissesDiagonally(List<Square> squares)
        {
            foreach(Square sq in squares) {
                if (sq.hasUp())
                {
                    addMissesToLeftAndRight(sq.up());
                }
                if (sq.hasDown())
                {
                    addMissesToLeftAndRight(sq.down());
                }

                
            }
        }

        internal List<Square> getContiguousHits(Square sq)
        {
            bool horizontal = false;
            if (sq.hasLeft() && (state(sq.left()) == SquareState.Hit)) horizontal = true;
            if (sq.hasRight() && (state(sq.right()) == SquareState.Hit)) horizontal = true;

            List<Square> answer = new List<Square>();
            if (horizontal)
            {
                Square current = sq;
                while (current.hasLeft() && state(current.left()) == SquareState.Hit)
                {
                    current = current.left();
                }

                answer.Add(current);
                while (current.hasRight() && state(current.right()) == SquareState.Hit)
                {
                    current = current.right();
                    answer.Add(current);
                }
            }
            else
            {
                Square current = sq;
                while (current.hasDown() && state(current.down()) == SquareState.Hit)
                {
                    current = current.down();
                }

                answer.Add(current);
                while (current.hasUp() && state(current.up()) == SquareState.Hit)
                {
                    current = current.up();
                    answer.Add(current);
                }
            }
            return answer;
        }

        internal void sunk(int size)
        {
            remainingShips.Remove(size);
        }

        internal void addMissesToLeftAndRight(List<Square> squares)
        {
            foreach (Square sq in squares)
            {
                addMissesToLeftAndRight(sq);
            }
        }

        private void addMissesToLeftAndRight(Square sq)
        {
            if (sq.hasRight())
            {
                addMissIfNotHit(sq.right());
            }
            if (sq.hasLeft())
            {
                addMissIfNotHit(sq.left());
            }
        }

        internal void addMissesAboveAndBelow(List<Square> squares)
        {
            foreach(Square sq in squares)
            {
                addMissesAboveAndBelow(sq);
            }
        }

        private void addMissesAboveAndBelow(Square sq)
        {
            if (sq.hasUp())
            {
                addMissIfNotHit(sq.up());
            }
            if (sq.hasDown())
            {
                addMissIfNotHit(sq.down());
            }
        }

        internal int maxRemaining()
        {
            return remainingShips.Max();
        }

        internal void addMissIfNotHit(Square s)
        {
            if (state(s) != SquareState.Hit) board[s.x, s.y] = SquareState.Miss;
        }

        internal void processHit(Square s)
        {
            board[s.x, s.y] = SquareState.Hit;
        }


        internal List<Square> getSquares(SquareState state)
        {
            List<Square> matches = new List<Square>();
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    if (board[i, j] == state) matches.Add(new Square(i, j));
                }
            }
            return matches;
        }

        internal static List<Square> neighbours(Square sq)
        {
            List<Square> neighbours = new List<Square>();
            if (sq.x != 0) neighbours.Add(new Square(sq.x - 1, sq.y));
            if (sq.x != side - 1) neighbours.Add(new Square(sq.x + 1, sq.y));
            if (sq.y != 0) neighbours.Add(new Square(sq.x, sq.y - 1));
            if (sq.y != side - 1) neighbours.Add(new Square(sq.x, sq.y + 1));
            return neighbours;
        }

        internal SquareState state(Square sq)
        {
            return board[sq.x, sq.y];
        }

        internal bool hasTwoBoat()
        {
            return remainingShips.Contains(2);
        }

        internal void print()
        {
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    string c = "-";
                    if (board[i, j] == SquareState.Hit) c = "h";
                    if (board[i, j] == SquareState.Miss) c = "m";
                    Console.Write(c + ",");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }
    }
}