Shader "Unlit/FinalProcessor"
{
    Properties
    {
        [MainTexture]_WebcamTex("Webcam Texture", 2D) = "white" {}
        _ColorTex("Color processed Texture", 2D) = "white" {}
        _SobelTex("Sobel processed Texture", 2D) = "white" {}
        _FeatherSize("Mash feather size", Int) = 20
        [Toggle]_SobelEdge("Use Sobel Detection Instead Of Color Detection", Int) = 0
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _WebcamTex;
            sampler2D _ColorTex;
            sampler2D _SobelTex;
            float4 _WebcamTex_ST;
            int _FeatherSize;
            int _SobelEdge;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _WebcamTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y = 1 - uv.y;
                fixed4 col = tex2D(_WebcamTex, uv);
                fixed4 colpro = tex2D(_ColorTex, uv);
                fixed4 sobpro = tex2D(_SobelTex, uv);
                float score = (1 - sobpro.b) + col.a;
                col.a = (score / 2 > 0.8);
                float Pi = 6.28318530718; // Pi*2

                float Directions = 16.0;
                float Quality = 3.0;
                float2 Radius = _FeatherSize / float2(1920, 1080);
                fixed4 blursob = tex2D(_SobelTex, uv);
                for (float d = 0.0; d < Pi; d += Pi / Directions)
                {
                    for (float i = 1.0 / Quality; i <= 1.0; i += 1.0 / Quality)
                    {
                        blursob += tex2D(_SobelTex, uv + float2(cos(d), sin(d)) * Radius * i);
                    }
                }
                blursob /= Quality * Directions - 15.0;
                col.a *= 1 - blursob.b;
                return _SobelEdge ? col : colpro;
            }
            ENDCG
        }
    }
}