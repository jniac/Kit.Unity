
float clamp(float x) 
{
	return x < 0 ? 0 : x > 1 ? 1 : x;
}

float sqLength(float2 v) { return v.x * v.x + v.y * v.y; }
float sqLength(float3 v) { return v.x * v.x + v.y * v.y + v.z * v.z; }
float sqLength(float4 v) { return v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w; }

float manhattan(float2 v)
{
	return v.x < 0 
		? (v.y < 0 ? -v.x - v.y :  -v.x + v.y) 
		: (v.y < 0 ? v.x - v.y :  v.x + v.y);
}


float pcurve(float x, float a, float b)
{
    const float k = pow(a+b,a+b) / (pow(a,a)*pow(b,b));
    return k * pow( x, a ) * pow( 1.0-x, b );
}



float mapUnclamped(float x, float min, float max) 
{
	return (x - min) / (max - min);
}
float map(float x, float min, float max) 
{
	return x <= min ? 0 : x >= max ? 1 : mapUnclamped(x, min, max);
}

float mixUnclamped(float a, float b, float t = .5) 
{
	return a + (b - a) * t;
}
float mix(float a, float b, float t = .5) 
{
	return t < 0 ? a : t > 1 ? b : mixUnclamped(a, b, t);
}

fixed4 mixUnclamped(fixed4 a, fixed4 b, float t = .5) 
{
	return a + (b - a) * t;
}
fixed4 mix(fixed4 a, fixed4 b, float t = .5) 
{
	return t < 0 ? a : t > 1 ? b : mixUnclamped(a, b, t);
}



float easeIn2(float x) { return x * x; }
float easeIn3(float x) { return x * x * x; }
float easeIn4(float x) { return x * x * x * x; }
float easeIn5(float x) { return x * x * x * x * x; }
float easeIn(float x, float p) { return pow(x, p); }

float easeOut2(float x) { return 1 - (x = 1 - x) * x; }
float easeOut3(float x) { return 1 - (x = 1 - x) * x * x; }
float easeOut4(float x) { return 1 - (x = 1 - x) * x * x * x; }
float easeOut5(float x) { return 1 - (x = 1 - x) * x * x * x * x; }
float easeOut(float x, float p) { return 1 - pow(1 - x, p); }

float easeInOut(float x, float p, float i) 
{
	return x == i ? x : x < i 
		? 1 / pow(i, p - 1) * pow(x, p)
		: 1 - 1 / pow(1 - i, p - 1) * pow(1 - x, p);
}











// SHAPES
float circle(in float2 p, float radius) 
{
	radius *= radius;
	
    return 1. - smoothstep(radius - (radius * 0.01),
                         radius + (radius * 0.01),
                         sqLength(p));
}



