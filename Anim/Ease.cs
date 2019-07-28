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

            public static Del InOut(float power, float inflexion = .5f) => x => x < inflexion
                ? Pow(inflexion, 1f - power) * Pow(x, power)
                : 1f - Pow(1f - x, 1f - power) * Pow(1f - x, power);
            public static Del InOut2 = x => x < .5f ? (x = x * 2f) * x / 2f : 1f - (x = 2f * (1f - x)) * x / 2f;
            public static Del InOut3 = x => x < .5f ? (x = x * 2f) * x * x / 2f : 1f - (x = 2f * (1f - x)) * x * x / 2f;
            public static Del InOut4 = x => x < .5f ? (x = x * 2f) * x * x * x / 2f : 1f - (x = 2f * (1f - x)) * x * x * x / 2f;
            public static Del InOut5 = x => x < .5f ? (x = x * 2f) * x * x * x * x / 2f : 1f - (x = 2f * (1f - x)) * x * x * x * x / 2f;
        }
    }
}
