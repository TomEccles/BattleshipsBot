using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ExamplePlayer.Firers
{
    class SixThreeTwoFirer
    {
        private StripingFirer sixStriper;
        private StripingFirer threeStriper;
        private StripingFirer twoStriper;
        private Random r = new Random();

        public SixThreeTwoFirer()
        {
            int best = r.Next(0, 5);
            sixStriper = new StripingFirer(6, best);
            threeStriper = new StripingFirer(3, best);
            twoStriper = new StripingFirer(2, best);
        }

        public Square selectTarget(OpponentsBoard board)
        {
            Square s = sixStriper.selectTarget(board);
            if (s != null) return s;

            if (board.hasTwoBoat())
            {
                s = twoStriper.selectTarget(board);
                return s;
            }

            StripingFirer nearerCompletion =
                    twoStriper.remaining(board) < threeStriper.remaining(board) ? twoStriper : threeStriper;

            return nearerCompletion.selectTarget(board);
        }
    }
}
