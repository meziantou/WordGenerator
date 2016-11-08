using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGenerator
{
    public class WordGenerator
    {
        private bool _sorted = false;
        private readonly Random _random = new Random();
        private readonly IDictionary<string, WeightCollection<char>> _weights = new Dictionary<string, WeightCollection<char>>();

        public int Order { get; set; } = 5;

        public void AddWord(string word)
        {
            _sorted = false;

            string previous = "";
            foreach (var c in word.Select(char.ToLowerInvariant))
            {
                WeightCollection<char> weight;
                if (!_weights.TryGetValue(previous, out weight))
                {
                    weight = new WeightCollection<char>();
                    _weights.Add(previous, weight);
                }

                weight.Add(c);
                previous += c;
                if (previous.Length > Order)
                {
                    previous = previous.Substring(1);
                }
            }
        }

        public string Generate(int length)
        {
            if (!_sorted)
            {
                foreach (var weight in _weights)
                {
                    weight.Value.Sort();
                }

                _sorted = true;
            }

            string previousChar = "";
            string result = null;
            for (int i = 0; i < length; i++)
            {
                char generated = Random(previousChar);
                previousChar += generated;
                if (previousChar.Length > Order)
                {
                    previousChar = previousChar.Substring(1);
                }
                result += generated;
            }

            return result;
        }

        private char Random(string previousChar)
        {
            WeightCollection<char> weight;
            if (_weights.TryGetValue(previousChar, out weight))
            {
                double randomValue = _random.NextDouble() * weight.TotalWeight;
                foreach (var item in weight)
                {
                    if (randomValue < item.Weight)
                        return item.Item;

                    randomValue -= item.Weight;
                }
            }

            if (previousChar.Length > 0)
            {
                return Random(previousChar.Substring(1));
            }

            throw new ArgumentException($"Char '{previousChar}' is not expected.");
        }
    }
}