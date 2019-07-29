namespace Kit.Unity
{
    using static UnityEngine.Mathf;

    public partial class Anim
    {
        public static class Ease
        {
            public delegate float Del(float x);

            public static Del Linear = x => x;

            public static Del In(float power) => x => Pow(x, power);
            public static Del In2 = x => x * x;
            public static Del In3 = x => x * x * x;
            public static Del In4 = x => x * x * x * x;
            public static Del In5 = x => x * x * x * x * x;

            public static Del Out(float power) => x => 1f - Pow(1f - x, power);
            public static Del Out2 = x => 1f - (x = 1f - x) * x;
            public static Del Out3 = x => 1f - (x = 1f - x) * x * x;
            public static Del Out4 = x => 1f - (x = 1f - x) * x * x * x;
            public static Del Out5 = x => 1f - (x = 1f - x) * x * x * x * x;

            // https://www.desmos.com/calculator/kikl4d4sed
            public static Del InOut(float power, float inflexion = .5f) => x => x < inflexion
                ? Pow(inflexion, 1f - power) * Pow(x, power)
                : 1f - Pow(1f - x, 1f - power) * Pow(1f - x, power);
            public static Del InOut2 = x => x < .5f ? (x = x * 2f) * x / 2f : 1f - (x = 2f * (1f - x)) * x / 2f;
            public static Del InOut3 = x => x < .5f ? (x = x * 2f) * x * x / 2f : 1f - (x = 2f * (1f - x)) * x * x / 2f;
            public static Del InOut4 = x => x < .5f ? (x = x * 2f) * x * x * x / 2f : 1f - (x = 2f * (1f - x)) * x * x * x / 2f;
            public static Del InOut5 = x => x < .5f ? (x = x * 2f) * x * x * x * x / 2f : 1f - (x = 2f * (1f - x)) * x * x * x * x / 2f;

            // https://www.desmos.com/calculator/o0ffhpeqos
            public static Del Elastic(float freq = 7, float amp = .33f, float power = 2.66f) => x =>
                1f + (1f + (amp - 1f) * (1f - Pow(1f - x, 2f * freq))) * Cos(PI * (1f + x * freq)) * Pow(1f - x, power);

            // https://www.desmos.com/calculator/kypawtbtdi
            static float OvershootBase(float x, float a) => x + x * (x = a - x) * x / (a * a * a);
            public static Del Overshoot(float overshoot = .5f)
            {
                const float amin = 0.1f;
                const float amax = 1f / 3f;
                float a = amax + (amin - amax) * overshoot;
                float x1 = a * (4f + Sqrt(16f - 12 * (a + 1))) / 6f;
                float y1 = OvershootBase(x1, a);

                return x => OvershootBase(x * x1, a) / y1;
            }
        }
    }
}
