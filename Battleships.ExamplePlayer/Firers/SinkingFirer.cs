using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ExamplePlayer.Firers
{
    class SinkingFirer
    {
        public Square selectTarget(OpponentsBoard board)
        {
            List<Square> squares = findPossibleHits(board);
            if (!squares.Any()) return null;
            return squares.First();
        }

        // This is pretty dumb. Particularly, it's up to you to mark impossible squares as miss before you call this.
        private List<Square> findPossibleHits(OpponentsBoard board)
        {
            List<Square> possibilities = new List<Square>();
            for (int i = 0; i < OpponentsBoard.side; i++)
                for (int j = 0; j < OpponentsBoard.side; j++)
                {
                    {
                        Square sq = new Square(i, j);
                        if (board.state(sq) != SquareState.Hit) continue;
                        possibilities.AddRange(
                                OpponentsBoard.neighbours(sq)
                                .Where(square => board.state(square) == SquareState.Unknown)
                                .ToList());
                    }
                }
            return possibilities;
        }
    }
}
