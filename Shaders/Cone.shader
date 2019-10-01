Shader "Kit/Cone"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _P("Center", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        ZWrite Off
        Cull Off 
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Kit.cginc"
            #include "Color.cginc"
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos: POSITION2;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);

                float2 p = i.uv;
                p += -.5;

                p.y += .2 * sin(p.x * 10);

                float d1 = sdBox(p + float2(.2, .2), float2(.3, .1));
                float d2 = sdCircle(p, .3);
                float d = max(d1, d2);

                return mix(Wheat.rgb, Blue.rgb, p.y + .4) * (1 - (d * 10 + _Time.y) % 1);

                if (d1 < 0)
                    return Blue;

                return d2 > 0 ? Red : Wheat;
                return sdCircle(p, .3) * 100;
                return sdPie(p, .2, .2) * 100;
                return sdAnnularShape(sdCircle(p, .3), .1) * 100;

                float3 v = i.worldPos.xyz;

                v *= 10;
                v += .5;
                v %= 1;
                v += -.5;

                float dx = length(cross(float3(1, 0, 0), v));
                float dy = length(cross(float3(0, 1, 0), v));
                float dz = length(cross(float3(0, 0, 1), v));

                float dmax = .1;
                if (dx > dmax && dy > dmax && dz > dmax)
                    discard;

                // col.a = length(p);

                return col;
            }
            ENDCG
        }
    }
}
