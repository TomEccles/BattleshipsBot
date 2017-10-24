using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ExamplePlayer.Firers
{
    class MostShipScorer
    {
        public double score(OpponentsBoard board, Square square)
        {
            return board.numberFitting(square);
        }
    }
}
