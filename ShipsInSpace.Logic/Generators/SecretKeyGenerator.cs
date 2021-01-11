using System;
using System.Linq;

namespace ShipsInSpace.Logic.Generators
{
    public class SecretKeyGenerator : IGenerator
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int Length = 8;
        private readonly Random _random;

        public SecretKeyGenerator()
        {
            _random = new Random();
        }

        public string Generate()
        {
            return new(Enumerable.Repeat(Characters, Length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}