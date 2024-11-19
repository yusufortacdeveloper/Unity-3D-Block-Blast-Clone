Shader "Custom/UIShineShaderWithDuration"
{
    Properties
    {
        _Color("Shine Color", Color) = (1.0, 1.0, 1.0, 1.0)  // Parlaklýk rengi
        _ShineWidth("Shine Width", Range(0.01, 1.0)) = 0.2   // Parlaklýk geniþliði
        _Speed("Shine Speed", Range(0.1, 5.0)) = 1.0         // Parlaklýk hýzý
        _ShineDuration("Shine Duration", Range(0.1, 5.0)) = 1.0 // Parlaklýk süresi
        _ShinePause("Shine Pause", Range(0.1, 5.0)) = 1.0     // Parlaklýk duraklama süresi
        _MainTex("Texture", 2D) = "white" {}                 // UI texture
        _UseCornerShine("Use Corner Shine", Float) = 0.0      // 0: Yatay shine, 1: Köþeden köþeye shine
        _UseShineDuration("Use Shine Duration", Float) = 0.0   // 0: Sürekli shine, 1: Shine süresi
    }
        SubShader
        {
            LOD 100
   Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off    // Derinlik yazýmýný kapat
        ZTest Always  // Her zaman çizim yap, derinlik testi yok
        Cull Off

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
                float4 _Color;
                float _ShineWidth;
                float _Speed;
                float _ShineDuration; // Parlaklýk süresi
                float _ShinePause;    // Parlaklýk duraklama süresi
                float _UseCornerShine; // 0 veya 1, shine efektini kontrol etmek için
                float _UseShineDuration; // 0 veya 1, shine süresi seçeneði

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // UI texture'unu al
                    fixed4 texColor = tex2D(_MainTex, i.uv);

                // Shine süresi ve duraklama kontrolü
                float totalCycleTime = _ShineDuration + _ShinePause;
                float shineTime = fmod(_Time.y, totalCycleTime); // Zamaný döngüsel hale getir
                float shinePos;

                // Eðer shine süresi aktifse
                if (_UseShineDuration == 1.0)
                {
                    // Shine efektinin sürelerine göre hesapla
                    if (shineTime < _ShineDuration)
                    {
                        shinePos = shineTime / _ShineDuration; // 0 ile 1 arasýnda deðiþir
                    }
                    else
                    {
                        shinePos = 1.0; // Shine duraklama süresinde
                    }
                }
                else
                {
                    shinePos = frac(_Time.y * _Speed); // Sürekli shine için
                }

                float shineMask;

                if (_UseCornerShine == 1.0)
                {
                    // Köþeden köþeye shine efekti: sol alt köþeden sað üst köþeye doðru hareket
                    float shineCoord = (i.uv.x + i.uv.y) * 0.5; // x ve y'yi karýþtýrarak köþegen efekti
                    shineMask = smoothstep(shinePos - _ShineWidth, shinePos, shineCoord) *
                                smoothstep(shinePos + _ShineWidth, shinePos, shineCoord);
                }
                else
                {
                    // Yatay shine efekti: sol kenardan sað kenara hareket
                    shineMask = smoothstep(shinePos - _ShineWidth, shinePos, i.uv.x) *
                                smoothstep(shinePos + _ShineWidth, shinePos, i.uv.x);
                }

                // Parlaklýk rengiyle texture'ý harmanlama
                fixed4 shineEffect = _Color * shineMask;

                // Sonuç olarak dokuyu parlaklýk efektiyle birleþtiriyoruz
                return texColor + shineEffect;
            }
            ENDCG
        }
        }
            FallBack "UI/Default"
}
