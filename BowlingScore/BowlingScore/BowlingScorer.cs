using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public class BowlingScorer : IBowlingScorer
    {
        /*
        protected string input;
        protected int[] scores = new int[10];

        protected IEnumerable<char[]> FrameScores
        {
            get {
                int s = 0;
                char[] frame = new char[3];
                for (int f = 0; f < input.Length; f++) {
                    frame[s] = input[f];
                    if (frame[s]
                }
            }
        }

        public int Score(string input)
        {
            char t = input[0];

            if (char.ToUpper(t) != 'X') 

            string frame = string.Format("{0}", t, );
        }

        public int ScoreFrame()
        {
            return 0;
        }
         */

        public int Score(string input)
        {
            List<string> frames = new List<string>();
            string frame = string.Empty;

            for (int f = 0; f < input.Length; f++) {
                char t = input[f];
                frame += t;
                if (char.ToUpper(t) == 'X' || char.ToUpper(t) == '/') {
                    frames.Add(frame);
                    frame = string.Empty;
                }
            }
            if (frame != string.Empty)
                frames.Add(frame);

            return frames.Select(f => ScoreFrame(f)).Sum();
        }

        public static int ScoreFrame(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            char t = input[0];
            if (char.ToUpper(t) == 'X') {
                return 10;
            } else if (char.ToUpper(t) == '/') {
                return 10;
            } else {
                int score = (int)Char.GetNumericValue(t);
                score += ScoreFrame(input.Substring(1));
                return score;
            }
        }
    }
}
