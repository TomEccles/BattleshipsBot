using System;
using Battleships.Player.Interface;

namespace Battleships.ExamplePlayer
{
    class Square : IGridSquare
    {
        public int x;
        public int y;
        private static Random r = new Random();
        private static int side = 10;

        public Square(int row, int col)
        {
            this.x = row;
            this.y = col;
        }

        public char Row => (char)('A' + x);

        public int Column => y + 1;

        internal static Square randomSquare()
        {
            return new Square(r.Next(0, side - 1), r.Next(0, side - 1));
        }

        internal bool outOfBounds()
        {
            return x < 0 || x >= side || y < 0 || y >= side;
        }

        internal bool hasDown(int squares = 1)
        {
            return y >= squares;
        }

        internal bool hasUp(int squares = 1)
        {
            return y < side - squares;
        }

        internal bool hasLeft(int squares = 1)
        {
            return x >= squares;
        }

        internal bool hasRight(int squares = 1)
        {
            return x < side - squares;
        }

        internal Square down(int squares = 1)
        {
            return new Square(x, y - squares);
        }

        internal Square up(int squares = 1)
        {
            return new Square(x, y + squares);
        }

        internal Square left(int squares = 1)
        {
            return new Square(x - squares, y);
        }

        internal Square right(int squares = 1)
        {
            return new Square(x + squares, y);
        }
    }
}