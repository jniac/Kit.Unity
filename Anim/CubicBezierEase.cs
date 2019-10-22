#pragma warning disable RECS0018 // Comparaison des nombres à virgule flottante avec l’opérateur d’égalité

namespace Kit
{
    using System;

    /// <summary>
    /// Cubic bezier ease.
    /// From Gre solution: https://github.com/gre/bezier-easing/blob/master/src/index.js
    /// Online test: https://dotnetfiddle.net/iLo8rn
    /// </summary>
    public static class CubicBezierEase
    {
        const int NEWTON_ITERATIONS = 4;
        const float NEWTON_MIN_SLOPE = .001f;
        const float SUBDIVISION_PRECISION = .0000001f;
        const int SUBDIVISION_MAX_ITERATIONS = 10;

        const int kSplineTableSize = 11;
        const float kSampleStepSize = 1f / (kSplineTableSize - 1f);

        static float A(float aA1, float aA2) => 1f - 3f * aA2 + 3f * aA1;
        static float B(float aA1, float aA2) => 3f * aA2 - 6f * aA1;
        static float C(float aA1) => 3f * aA1;

        // Returns x(t) given t, x1, and x2, or y(t) given t, y1, and y2.
        static float CalcBezier(float aT, float aA1, float aA2) =>
            ((A(aA1, aA2) * aT + B(aA1, aA2)) * aT + C(aA1)) * aT;

        // Returns dx/dt given t, x1, and x2, or dy/dt given t, y1, and y2.
        static float GetSlope(float aT, float aA1, float aA2) =>
            3f * A(aA1, aA2) * aT * aT + 2f * B(aA1, aA2) * aT + C(aA1);

        static float BinarySubdivide(float aX, float aA, float aB, float mX1, float mX2)
        {
            float currentX, currentT;
            int i = 0;

            do
            {
                currentT = aA + (aB - aA) / 2f;
                currentX = CalcBezier(currentT, mX1, mX2) - aX;

                if (currentX > 0)
                {
                    aB = currentT;
                }
                else
                {
                    aA = currentT;
                }
            }
            while (Math.Abs(currentX) > SUBDIVISION_PRECISION && ++i < SUBDIVISION_MAX_ITERATIONS);

            return currentT;
        }

        static float NewtonRaphsonIterate(float aX, float aGuessT, float mX1, float mX2)
        {
            for (int i = 0; i < NEWTON_ITERATIONS; ++i)
            {
                float currentSlope = GetSlope(aGuessT, mX1, mX2);

                if (currentSlope == 0)
                    return aGuessT;

                float currentX = CalcBezier(aGuessT, mX1, mX2) - aX;
                aGuessT -= currentX / currentSlope;
            }

            return aGuessT;
        }

        static float LinearEasing(float x) => x;

        public static Func<float, float> Get(float mX1, float mY1, float mX2, float mY2, bool clamp = true)
        {
            const float EPSILON = .000001f;
            if (Math.Abs(mX1 - mY1) < EPSILON && Math.Abs(mX2 - mY2) < EPSILON)
                return LinearEasing;

            float[] sampleValues = new float[kSplineTableSize];
            for (var i = 0; i < kSplineTableSize; ++i)
                sampleValues[i] = CalcBezier(i * kSampleStepSize, mX1, mX2);

            float getTForX(float aX)
            {
                float intervalStart = 0f;
                int currentSample = 1;
                int lastSample = kSplineTableSize - 1;

                for (; (currentSample != lastSample) && sampleValues[currentSample] <= aX; ++currentSample)
                    intervalStart += kSampleStepSize;

                --currentSample;

                float dist = (aX - sampleValues[currentSample])
                    / (sampleValues[currentSample + 1] - sampleValues[currentSample]);
                float guessForT = intervalStart + dist * kSampleStepSize;

                float initialSlope = GetSlope(guessForT, mX1, mX2);
                if (initialSlope >= NEWTON_MIN_SLOPE)
                    return NewtonRaphsonIterate(aX, guessForT, mX1, mX2);

                if (initialSlope == 0.0)
                    return guessForT;

                return BinarySubdivide(aX, intervalStart, intervalStart + kSampleStepSize, mX1, mX2);
            }

            if (clamp)
                return x => x <= 0 ? 0 : x >= 1 ? 1 : CalcBezier(getTForX(x), mY1, mY2);

            return x => CalcBezier(getTForX(x), mY1, mY2);
        }
    }
}

#pragma warning restore RECS0018 // Comparaison des nombres à virgule flottante avec l’opérateur d’égalité
