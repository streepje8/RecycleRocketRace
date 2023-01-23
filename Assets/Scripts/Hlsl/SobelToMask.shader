Shader "Unlit/SobelToMask"
{
    Properties
    {
        _MainTex("Sobel Input", 2D) = "white" {}
        _InputWidth("Input Width", Float) = 1
        _InputHeight("Input Height", Float) = 1
        _ActivationValue("Activation Value", Float) = 0.7
        _MinActivationHits("Min Activation Hits", int) = 3
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _InputWidth;
            float _InputHeight;
            float _ActivationValue;
            int _MinActivationHits;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //WARNING THIS SHADER IS SLOW AS FUCK, so i decided to not use it
                /*
                int hits = 0;
                for(float x = i.uv.x; x >= 0; x -= 1 / _InputWidth)
                {
                    if(tex2D(_MainTex, float2(x,i.uv.y)).r > _ActivationValue)
                    {
                        hits++;
                        if(hits >= _MinActivationHits) break;
                    }
                }
                if(!hits >= _MinActivationHits) return fixed4(0,0,0,1);
                hits = 0;
                for(float x = i.uv.x; x >= 0; x += 1 / _InputWidth)
                {
                    if(tex2D(_MainTex, float2(x,i.uv.y)).r > _ActivationValue)
                    {
                        hits++;
                        if(hits >= _MinActivationHits) break;
                    }
                }
                if(!hits >= _MinActivationHits) return fixed4(0,0,0,1);
                hits = 0;
                for(float y = i.uv.y; y >= 0; y -= 1 / _InputHeight)
                {
                    if(tex2D(_MainTex, float2(i.uv.x,y)).r > _ActivationValue)
                    {
                        hits++;
                        if(hits >= _MinActivationHits) break;
                    }
                }
                if(!hits >= _MinActivationHits) return fixed4(0,0,0,1);
                hits = 0;
                for(float y = i.uv.y; y >= 0; y += 1 / _InputHeight)
                {
                    if(tex2D(_MainTex, float2(i.uv.x,y)).r > _ActivationValue)
                    {
                        hits++;
                        if(hits >= _MinActivationHits) break;
                    }
                }
                if(!hits >= _MinActivationHits) return fixed4(0,0,0,1);
                */
                return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}