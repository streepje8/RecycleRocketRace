Shader "Unlit/SobelProcessor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Directions("Directions", Float) = 16.0
        _Width("Input width", int) = 1920
        _Height("Input height", int) = 1080
        _Treshhold("Treshhold", Range(0.0,1.0)) = 0.8
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Directions;
            int _Width;
            int _Height;
            float _Treshhold;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //From https://github.com/amilajack/gaussian-blur/blob/master/src/9.glsl
            fixed4 frag(v2f j) : SV_Target
            {
                float2 off1 = float2(1.3846153846, 1.3846153846) * _Directions;
                float2 off2 = float2(3.2307692308, 3.2307692308) * _Directions;
                float2 resolution = float2(_Width, _Height);
                fixed4 col = tex2D(_MainTex, j.uv) * 0.2270270270;
                col += tex2D(_MainTex, j.uv + (off1 / resolution)) * 0.3162162162;
                col += tex2D(_MainTex, j.uv - (off1 / resolution)) * 0.3162162162;
                col += tex2D(_MainTex, j.uv + (off2 / resolution)) * 0.0702702703;
                col += tex2D(_MainTex, j.uv - (off2 / resolution)) * 0.0702702703;
                return col > _Treshhold;
            }
            ENDCG
        }
    }
}