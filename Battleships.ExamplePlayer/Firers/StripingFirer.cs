using Battleships.ExamplePlayer.Firers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ExamplePlayer
{
    class StripingFirer
    {
        
        private int best;
        private int modulus;
        private static Random r = new Random();
        private MostShipScorer scorer = new MostShipScorer();


        public StripingFirer(int modulus)
        {
            this.modulus = modulus;
            best = r.Next() % modulus;
        }

        public StripingFirer(int modulus, int best)
        {
            this.modulus = modulus;
            this.best = best;
        }

        public Square selectTarget(OpponentsBoard board)
        {
            List<Square> empties = validSquares(board);

            if (!empties.Any()) return null;
            empties.Sort((sq1, sq2) => scorer.score(board,sq1).CompareTo(scorer.score(board, sq2)));
            empties.Reverse();
            
            return empties.First();
        }

        private List<Square> validSquares(OpponentsBoard board)
        {
            List<Square> empties =
                    board.getSquares(SquareState.Unknown)
                        .Where(good)
                        .ToList();
            empties.Shuffle();
            return empties;
        }

        private bool good(Square sq)
        {
            return ((sq.y + sq.x + best) % modulus) == 0;
        }

        public int remaining(OpponentsBoard board)
        {
            return validSquares(board).Count();
        }
    }
}

