using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public class JBowlingScorer : IBowlingScorer
    {
        protected int[] scores = new int[10];

        public int Score(string input)
        {
            return 10;
        }
    }
}
