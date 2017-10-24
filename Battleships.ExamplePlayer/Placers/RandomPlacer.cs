using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ExamplePlayer
{
    class RandomPlacer
    {
        private static Random r = new Random();
        public IEnumerable<MyShipPosition> GetShipPositions()
        {
            List<MyShipPosition> positions = new List<MyShipPosition>();
            foreach (int length in new List<int> { 5, 4, 3, 3, 2 })
            {
                positions.Add(GetNewShip(positions, length));

            }
            return positions;
        }

        private MyShipPosition GetNewShip(List<MyShipPosition> positionsSoFar, int length)
        {
            while (true)
            {
                Square startSquare = Square.randomSquare();
                bool horizontal = r.Next(2) == 1;
                MyShipPosition shipAttempt;
                if (horizontal)
                {
                    shipAttempt =
                           new MyShipPosition(
                                   startSquare.x,
                                   startSquare.y,
                                   startSquare.x,
                                   startSquare.y + length - 1);
                }
                else
                {
                    shipAttempt =
                            new MyShipPosition(
                                   startSquare.x,
                                   startSquare.y,
                                   startSquare.x + length - 1,
                                   startSquare.y);
                }
                if (isLegal(shipAttempt, positionsSoFar))
                {
                    return shipAttempt;
                }
            }
        }


        private bool isLegal(MyShipPosition shipAttempt, List<MyShipPosition> shipsSoFar)
        {
            if (shipAttempt.outOfBounds())
            {
                return false;
            }
            if (shipAttempt.bordersAny(shipsSoFar))
            {
                return false;
            }
            return true;
        }
    }
}
