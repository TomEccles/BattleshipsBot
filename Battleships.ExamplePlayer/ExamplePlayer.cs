namespace Battleships.ExamplePlayer
{
    using Battleships.ExamplePlayer.Firers;
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;

    public class ExamplePlayer : IBattleshipsBot
    {
        private int hits;
        private OpponentsBoard board;
        private Square lastShot;
        private SixThreeTwoFirer stripyFirer;
        private SinkingFirer sinkingFirer;

        public string Name
        {
            get { return "Bot 1"; }
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            hits = 0;
            lastShot = null;
            stripyFirer = new SixThreeTwoFirer();
            sinkingFirer = new SinkingFirer();
            board = new OpponentsBoard();
            return new RandomPlacer().GetShipPositions();
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            lastShot = nextTarget;
            if (nextTarget == null) throw new Exception("Didn't find target");
            return nextTarget;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            Console.Write("(" + this.lastShot.x + "," + this.lastShot.y + ")" + ":" + wasHit);
            Console.WriteLine();
            board.print();

            if (wasHit)
            {

                hits++;
                board.processHit(this.lastShot);
                List<Square> sinkingShip = board.getContiguousHits(this.lastShot);
                bool sunk = isSunk(board, sinkingShip);
                int size = sinkingShip.Count;
                if (sunk)
                {
                    board.addMissesAboveAndBelow(sinkingShip);
                    board.addMissesToLeftAndRight(sinkingShip);
                    board.addMissesDiagonally(sinkingShip);
                    board.sunk(size);
                }
                else if (size >= 2)
                {
                    if (sinkingShip[0].y == sinkingShip[1].y)
                    {
                        board.addMissesAboveAndBelow(sinkingShip);
                        board.addMissesDiagonally(sinkingShip);
                    }
                    else
                    {
                        board.addMissesToLeftAndRight(sinkingShip);
                        board.addMissesDiagonally(sinkingShip);
                    }
                }

                if (hits == 17)
                {
                    // We win!
                    foreach(Square sq in board.getSquares(SquareState.Unknown))
                    {
                        board.addMissIfNotHit(sq);
                    }
                }

            }
            else
            {
                Square sq = this.lastShot;
                board.addMissIfNotHit(sq);
                foreach(Square nbr in OpponentsBoard.neighbours(sq))
                {
                    if(board.state(nbr) == SquareState.Hit)
                    {
                        List<Square> sinkingShip = board.getContiguousHits(nbr);
                        if (isSunk(board, sinkingShip))
                        {
                            board.sunk(sinkingShip.Count);
                            board.addMissesAboveAndBelow(sinkingShip);
                            board.addMissesToLeftAndRight(sinkingShip);
                            board.addMissesDiagonally(sinkingShip);
                        }
                    }
                }
            }
            hits++;;
            hits--;
        }

        private bool isSunk(OpponentsBoard board, List<Square> sinkingShip)
        {
            if (sinkingShip.Count < 2) return false;
            if (sinkingShip.Count == board.maxRemaining()) return true;

            Square first = sinkingShip[0];
            Square last = sinkingShip[sinkingShip.Count -1];

            bool horizontal = first.y == last.y;
            // logger(horizontal ? "Horizontal" : "Vertical");


            if (horizontal)
            {
                bool leftOk = !first.hasLeft() || board.state(first.left()) == SquareState.Miss;
                bool rightOk = !last.hasRight() || board.state(last.right()) == SquareState.Miss;
                //logger(leftOk ? "Left is done" : "Could still be left");
                //logger(rightOk ? "Right is done" : "Could still be right");
                return leftOk && rightOk;
            }
            else
            {
                bool downOk = !first.hasDown() || board.state(first.down()) == SquareState.Miss;
                bool upOk = !last.hasUp() || board.state(last.up()) == SquareState.Miss;
                //logger(downOk ? "Down is done" : "Could still be down");
                //logger(upOk ? "Up is done" : "Could still be up");
                return downOk && upOk;
            }
        }

        public void HandleOpponentsShot(IGridSquare square) {}

        private static MyShipPosition GetShipPosition(int startRow, int startColumn, int endRow, int endColumn)
        {
            return new MyShipPosition(new Square(startRow, startColumn), new Square(endRow, endColumn));
        }

        private Square GetNextTarget()
        {
            Square sq = sinkingFirer.selectTarget(board);
            if (sq != null) return sq;
            return stripyFirer.selectTarget(board);
        }
    }
}