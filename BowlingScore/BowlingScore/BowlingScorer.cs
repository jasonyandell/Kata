using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public class BowlingScorer : IBowlingScorer
    {
        class Frame
        {
            public string input = string.Empty;
            public int weight = 1;
        }

        public int Score(string input)
        {
            List<Frame> frames = new List<Frame>();
            string frame = string.Empty;
            int frameIndex = 0;

            Action<string> addFrameDelegate = frameInput => {
                frames.Add(new Frame { input = frameInput });
                frame = string.Empty;
                frameIndex++;
            };

            Action<int> incrementWeight = offset => frames[offset].weight++;

            Stack<Action> incrementWeights = new Stack<Action>();

            for (int f = 0; f < input.Length; f++) {
                char t = input[f];
                frame += t;
                if (char.ToUpper(t) == 'X') {
                    int baseIndex = frameIndex;
                    addFrameDelegate(frame);
                    incrementWeights.Push(() => incrementWeight(baseIndex + 1));
                    incrementWeights.Push(() => incrementWeight(baseIndex + 2));
                } else if (char.ToUpper(t) == '/') {
                    int baseIndex = frameIndex;
                    addFrameDelegate(frame);
                    incrementWeights.Push(() => incrementWeight(baseIndex + 1));
                } else if (t == '-') {
                    addFrameDelegate(frame);
                }
            }
            if (frame != string.Empty)
                addFrameDelegate(frame);

            while (incrementWeights.Count > 0) { incrementWeights.Pop()(); }

            return frames.Select(f => f.weight * ScoreFrame(f.input, 0)).Sum();
        }

        public static int ScoreFrame(string input, int tally)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            char t = input[0];
            if (t == '-') {
                return 0;
            } else if (char.ToUpper(t) == 'X') {
                return 10;
            } else if (char.ToUpper(t) == '/') {
                return 10 - tally;
            } else {
                int score = (int)Char.GetNumericValue(t);
                score += ScoreFrame(input.Substring(1), score);
                return score;
            }
        }
    }
}
