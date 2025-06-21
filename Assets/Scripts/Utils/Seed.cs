using System;
using UnityEngine;

namespace Utils {

    /// <summary>
    /// Seed is a utility class that provides a way to generate a random seed for procedural generation.
    /// It can be used to ensure consistent results across different runs of the application.
    /// </summary>
    [Serializable]
    public class Seed
    {
        [SerializeField] private int _seed;
        public int Value => _seed;

        public Seed()
        {
            _seed = GenerateSeed();
        }

        /// <summary>
        /// Initializes the seed if it is not already set.
        /// </summary>
        public void InitIfUnset()
        {
            if (_seed == 0)
                _seed = GenerateSeed();
        }

        /// <summary>
        /// Generates a new random seed based on the current time, a new GUID, and the system tick count.
        /// This method combines these values to produce a unique integer seed.
        /// The generated seed can be used for random number generation in procedural content generation.
        /// </summary>
        /// <returns>The generated seed</returns>
        private int GenerateSeed()
        {
            unchecked
            {
                int timeSeed = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
                int guidSeed = Guid.NewGuid().GetHashCode();
                int tickCount = Environment.TickCount;
                return timeSeed ^ guidSeed ^ tickCount;
            }
        }

        /// <summary>
        /// Generates a hash based on the x and y coordinates and a seed value.
        /// Simple 32-bit hash function (based on MurmurHash3 finalizer)
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <param name="seed">Int seed for hasing</param>
        /// <returns>int the 32-bit hash.</returns>
        private int Hash(int x, int y, int seed)
        {
            unchecked
            {
                uint h = (uint)(x * 374761393 + y * 668265263 + seed * 2654435761);
                h ^= h >> 13;
                h *= 1274126177;
                h ^= h >> 16;
                return (int)h;
            }
        }

        /// <summary>
        /// Generates a random value based on the x and y coordinates, and a range defined by min and max.
        /// This method uses a hash function to create a pseudo-random number based on the coordinates and seed.
        /// The resulting value is then constrained to the specified range.
        /// The method is useful for generating consistent random values in procedural generation scenarios,
        /// where the same coordinates will always yield the same random value.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <param name="min">Min value returned</param>
        /// <param name="max">Max value returned</param>
        /// <returns>A pseudo random value between min and max</returns>
        public int GetRandomValue(int x, int y, int min, int max)
        {
            int hash = Hash(x, y, _seed);
            uint value = XorShift32((uint)hash);
            return (int)(min + (value % (max - min)));
        }

        /// <summary>
        /// XorShift32 is a simple and fast pseudo-random number generator (PRNG) that uses bitwise operations to generate a new random value based on the input seed.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private uint XorShift32(uint x)
        {
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            return x;
        }

        /// <summary>
        /// Generates a new seed and updates the internal seed value.
        /// Useful for resetting the random number generator or when you want to change the procedural generation results.
        /// </summary>
        public void GenerateNewSeed()
        {
            _seed = GenerateSeed();
        }

        public int GetSeed() => _seed;
    }

}