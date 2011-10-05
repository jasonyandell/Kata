using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public class FSharpBowlingScorer : IBowlingScorer
    {
        public int Score(string input)
        {
            return BowlingScore.FSharp.Score(input);
        }
    }
}
