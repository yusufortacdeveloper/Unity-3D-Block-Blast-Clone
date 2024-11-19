Shader "Custom/FlagWaveShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _WaveFrequency("Wave Frequency", Range(0.1, 10.0)) = 1.0    // Dalga frekansý
        _WaveAmplitude("Wave Amplitude", Range(0.0, 1.0)) = 0.1      // Dalga genliði
        _Speed("Wave Speed", Range(0.1, 5.0)) = 1.0                  // Dalga hýzý
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }
            LOD 100

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float _WaveFrequency;
                float _WaveAmplitude;
                float _Speed;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Zamanla dalgalanma
                    float wave = sin(i.uv.y * _WaveFrequency + _Time.y * _Speed) * _WaveAmplitude;

                // UV koordinatlarýný güncelle
                float2 uv = i.uv + float2(0, wave);

                // Doku rengini al
                fixed4 texColor = tex2D(_MainTex, uv);

                return texColor;
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}
