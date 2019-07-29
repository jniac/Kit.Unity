# Ease

## InOut
https://www.desmos.com/calculator/kikl4d4sed

## Elastic
https://www.desmos.com/calculator/o0ffhpeqos
```csharp
public static Del Elastic(float freq = 7, float amp = .33f, float power = 2.66f) => x =>
{
	float ratio = 1f - Pow(1f - x, 2f * freq);
	amp = 1f + (amp - 1f) * ratio;
	return 1f + amp * Cos(PI * (1f + x * freq)) * Pow(1f - x, power);
};
```

## Overshoot
mouais, ça marche, mais ça pourrait être mieux
https://www.desmos.com/calculator/nrwopjlpbl
