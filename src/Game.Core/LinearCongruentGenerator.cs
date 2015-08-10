namespace Game.Core
{
    public class LinearCongruentGenerator
    {
        private readonly long _increment;
        private readonly long _modulus;
        private readonly long _multiplier;
        private long _state;

        public LinearCongruentGenerator(long seed, long modulus = 4294967296, long multiplier = 1103515245,
            long increment = 12345)
        {
            _state = seed;
            _modulus = modulus;
            _multiplier = multiplier;
            _increment = increment;
        }

        public int NextInt()
        {
            var rand = (_state & int.MaxValue) >> 16;

            _state = ((_multiplier*_state) + _increment)%_modulus;

            return (int) rand;
        }
    }
}