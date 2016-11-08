using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGenerator
{
    public class WordGenerator
    {
        private const char DefaultChar = '\0';

        private bool _sorted = false;
        private Random _random = new Random();
        private readonly IDictionary<char, WeightCollection<char>> _weights = new Dictionary<char, WeightCollection<char>>();

        public void AddWord(string word)
        {
            _sorted = false;

            char previous = DefaultChar;
            foreach (var c in word.Select(char.ToLowerInvariant))
            {
                WeightCollection<char> weight;
                if (!_weights.TryGetValue(previous, out weight))
                {
                    weight = new WeightCollection<char>();
                    _weights.Add(previous, weight);
                }

                weight.Add(c);
                previous = c;
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

            char previousChar = DefaultChar;
            string result = null;
            for (int i = 0; i < length; i++)
            {
                previousChar = Random(previousChar);
                result += previousChar;
            }

            return result;
        }

        private char Random(char previousChar)
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

            throw new ArgumentException($"Char '{previousChar}' is not expected.");
        }
    }
}