Shader "Unlit/ProcessedToMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width("Image width", Int) = 1920
        _Height("Image height", Int) = 1080
        _Treshhold("Local Treshhold", Range(0.0,1.0)) = 0.8
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
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

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                //local adaptive treshholding
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 pixelSize = 1 / float2(_Width, _Height);
                col += tex2D(_MainTex, i.uv + pixelSize);
                col += tex2D(_MainTex, i.uv - pixelSize);
                col += tex2D(_MainTex, i.uv + pixelSize * float2(0, 1));
                col += tex2D(_MainTex, i.uv + pixelSize * float2(1, 0));
                col += tex2D(_MainTex, i.uv + pixelSize * float2(1, 1));
                col += tex2D(_MainTex, i.uv + pixelSize * float2(0, -1));
                col += tex2D(_MainTex, i.uv + pixelSize * float2(-1, 0));
                col += tex2D(_MainTex, i.uv + pixelSize * float2(-1, -1));
                col /= 9;
                col.r = col.r > _Treshhold;
                col.bg = 0;
                col.a = 1;
                float borderOffset = 100.0;
                col.r *= (i.uv.x > pixelSize * borderOffset && i.uv.x < _Width - pixelSize * borderOffset && i.uv.y >
                    pixelSize * borderOffset && i.uv.y < _Height - pixelSize * borderOffset);
                return col;
            }
            ENDCG
        }
    }
}