using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScore
{
    public class JBowlingScorer : IBowlingScorer
    {
        protected int[] scores = new int[10];

        public int SimpleValue(string input, int index)
        {
            if (index < 0) return 0;
            if (index >= input.Length) return 0;

            var ch = input[index];

            switch (ch)
            {
                case 'X':
                    return 10;
                case '/':
                    return 10 - SimpleValue(input, index - 1);
                case '-':
                    return 0;
                default:
                    if ("0123456789".Contains(ch))
                    {
                        return "0123456789".IndexOf(ch);
                    }
                    throw new ArgumentException("Cannot parse "+ch, "input");
            }
        }

        public int Value(string input, int index)
        {
            if (index < 0) return 0;
            if (index >= input.Length) return 0;

            var ch = input[index];

            switch (ch)
            {
                case 'X':
                    return SimpleValue(input, index) + SimpleValue(input, index + 1) + SimpleValue(input, index + 2);
                case '/':
                    return SimpleValue(input, index) + SimpleValue(input, index + 1);
                case '-':
                    return SimpleValue(input, index);
                default:
                    return SimpleValue(input, index);
            }
        }

        public int Score(string input)
        {
            var results = new List<KeyValuePair<char, int>>();

            var total = 0;
            for(int i = 0; i < input.Length; ++i)
            {
                var val = Value(input, i);
                results.Add(new KeyValuePair<char, int>(input[i], val));
                total += val;
            }
            return total;
        }
    }
}
