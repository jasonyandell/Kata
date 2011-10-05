using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public interface IBowlingScorer : IScorer
    {
        int Score(string input);
    }
}
