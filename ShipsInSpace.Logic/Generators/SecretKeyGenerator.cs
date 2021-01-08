using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipsInSpace.Logic.Generators
{
    public class SecretKeyGenerator : IGenerator
    {
        private readonly Random _random;
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int Length = 8;

        public SecretKeyGenerator()
        {
            _random = new Random();
        }

        public string Generate() => new string(Enumerable.Repeat(Characters, Length).Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
