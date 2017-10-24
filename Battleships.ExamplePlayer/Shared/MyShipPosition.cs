using System;
using System.Collections.Generic;
using Battleships.Player.Interface;

namespace Battleships.ExamplePlayer
{
    internal class MyShipPosition : IShipPosition
    {
        private Square start;
        private Square end;

        IGridSquare IShipPosition.StartingSquare => start;

        IGridSquare IShipPosition.EndingSquare => end;

        public MyShipPosition(Square startingSquare, Square endingSquare)
        {
            this.start = startingSquare;
            this.end = endingSquare;
        }

        public MyShipPosition(int row1, int col1, int row2, int col2)
        {
            this.start = new Square(row1, col1);
            this.end = new Square(row2, col2);
        }

        internal bool outOfBounds()
        {
            return start.outOfBounds() || end.outOfBounds();
        }

        internal bool bordersAny(List<MyShipPosition> shipsSoFar)
        {
            foreach (MyShipPosition other in shipsSoFar) {
                if (borders(other)) return true;
            }
            return false;
        }

        private bool borders(MyShipPosition other)
        {
            return
                other.end.x >= this.start.x - 1 &&
                this.end.x >= other.start.x - 1 &&
                other.end.y >= this.start.y - 1 &&
                this.end.y >= other.start.y - 1;
        }
    }
}