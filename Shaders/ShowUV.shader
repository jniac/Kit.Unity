Shader "Kit/ShowUV"
{
    Properties
    {
        SPACING ("SPACING", Range(1, 32)) = 8
        DOT_SIZE ("DOT_SIZE", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Kit.cginc"

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
            };

            float SPACING, DOT_SIZE;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = float4(i.uv, 1, 1);

                float2 p = (i.uv * SPACING) % 1;

                if (p.x > .5)
                    p.x += -1;
                if (p.y > .5)
                    p.y += -1;

                if (sqLength(p) < DOT_SIZE * DOT_SIZE)
                    // col = float4(1 - i.uv, 1, 1);
                    // col = float4(0, 1, 0, 1);
                    col = 1;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
