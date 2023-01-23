Shader "Unlit/WebcamKey"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Comparison("Texture", 2D) = "white" {}
        _Treshold("Treshold", Range(0.0,0.1)) = 0.001
        _SobelTreshhold("Sobel Treshhold", Range(0.0,1.0)) = 0.4
        _Power("Power", Range(0.0,10.0)) = 2
        _MinWHMaxWH("MinWHMaxWH", Vector) = (0,0,1,1)
        _DeltaX ("Delta X", Float) = 0.01
        _DeltaY ("Delta Y", Float) = 0.01
        [Toggle]_SobelMode("Sobel mode", Int) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
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
            sampler2D _Comparison;
            float _Treshold;
            float _Power;
            float4 _MinWHMaxWH;
            float _SobelTreshhold;
            int _SobelMode;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //From https://gist.github.com/mattatz/d14898c16008e3ad1abe
            float _DeltaX;
            float _DeltaY;

            float sobel(sampler2D tex, float2 uv)
            {
                float2 delta = float2(_DeltaX, _DeltaY);

                float4 hr = float4(0, 0, 0, 0);
                float4 vt = float4(0, 0, 0, 0);

                hr += (1 - tex2D(tex, (uv + float2(-1.0, -1.0) * delta))) * 1.0;
                hr += (1 - tex2D(tex, (uv + float2(0.0, -1.0) * delta))) * 0.0;
                hr += (1 - tex2D(tex, (uv + float2(1.0, -1.0) * delta))) * -1.0;
                hr += (1 - tex2D(tex, (uv + float2(-1.0, 0.0) * delta))) * 2.0;
                hr += (1 - tex2D(tex, (uv + float2(0.0, 0.0) * delta))) * 0.0;
                hr += (1 - tex2D(tex, (uv + float2(1.0, 0.0) * delta))) * -2.0;
                hr += (1 - tex2D(tex, (uv + float2(-1.0, 1.0) * delta))) * 1.0;
                hr += (1 - tex2D(tex, (uv + float2(0.0, 1.0) * delta))) * 0.0;
                hr += (1 - tex2D(tex, (uv + float2(1.0, 1.0) * delta))) * -1.0;

                vt += (1 - tex2D(tex, (uv + float2(-1.0, -1.0) * delta))) * 1.0;
                vt += (1 - tex2D(tex, (uv + float2(0.0, -1.0) * delta))) * 2.0;
                vt += (1 - tex2D(tex, (uv + float2(1.0, -1.0) * delta))) * 1.0;
                vt += (1 - tex2D(tex, (uv + float2(-1.0, 0.0) * delta))) * 0.0;
                vt += (1 - tex2D(tex, (uv + float2(0.0, 0.0) * delta))) * 0.0;
                vt += (1 - tex2D(tex, (uv + float2(1.0, 0.0) * delta))) * 0.0;
                vt += (1 - tex2D(tex, (uv + float2(-1.0, 1.0) * delta))) * -1.0;
                vt += (1 - tex2D(tex, (uv + float2(0.0, 1.0) * delta))) * -2.0;
                vt += (1 - tex2D(tex, (uv + float2(1.0, 1.0) * delta))) * -1.0;

                return sqrt(hr * hr + vt * vt);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (!(i.uv.x < _MinWHMaxWH.z && i.uv.y < _MinWHMaxWH.w && i.uv.x > _MinWHMaxWH.x && i.uv.y > _MinWHMaxWH
                    .y))
                {
                    return 0;
                }
                fixed4 webcamColor = tex2D(_MainTex, i.uv);
                fixed4 solidFrameColor = tex2D(_Comparison, i.uv);
                float webcamV = max(webcamColor.r, max(webcamColor.g, webcamColor.b)) / 255;
                float solidV = max(solidFrameColor.r, max(solidFrameColor.g, solidFrameColor.b)) / 255;
                float webcamS = 1 - min(webcamColor.r, min(webcamColor.g, webcamColor.b)) / (webcamV * 255);
                float solidS = 1 - min(solidFrameColor.r, min(solidFrameColor.g, solidFrameColor.b)) / (solidV * 255);
                float rDiff = solidFrameColor.r - webcamColor.r;
                float gDiff = solidFrameColor.g - webcamColor.g;
                float bDiff = solidFrameColor.b - webcamColor.b;
                float totalDiff = (abs((rDiff + gDiff + bDiff) / 3) + (abs(webcamV - solidV) / 2 * 500) + abs(
                    webcamS - solidS)) / 3;
                float sobelCam = sobel(_MainTex, i.uv);
                float sobelComp = sobel(_Comparison, i.uv);
                float sobelDiff = sobelCam - (sobelComp * (1 - totalDiff > _Treshold));
                if (sobelDiff < _SobelTreshhold) sobelDiff = 0;
                sobelDiff = pow(sobelDiff, 2);
                if (_SobelMode) return fixed4(sobelDiff, sobelDiff, sobelDiff, 1);
                //return fixed4((abs(webcamV - solidV) / 2 * 500),0,0,1);
                //pow(totalDiff,_Power) > _Treshold
                return fixed4(webcamColor.r, webcamColor.g, webcamColor.b, (pow(totalDiff, _Power) > _Treshold));
            }
            ENDCG
        }
    }
}