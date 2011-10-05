using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public class BowlingScorer : IBowlingScorer
    {
        protected int[] scores = new int[10];

        public int Score(string input)
        {
            int score = 0;
            for(int i=0; i<input.Length; i++) {
                char t = input[i];
                //if (t == 'X')
            }
            return scores.Sum();
        }

        public int ScoreFrame()
        {
            return 0;
        }
    }
}
