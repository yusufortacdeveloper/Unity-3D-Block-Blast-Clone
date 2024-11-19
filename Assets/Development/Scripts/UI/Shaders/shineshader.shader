Shader "Custom/UIShineShaderWithDuration"
{
    Properties
    {
        _Color("Shine Color", Color) = (1.0, 1.0, 1.0, 1.0)  // Parlakl�k rengi
        _ShineWidth("Shine Width", Range(0.01, 1.0)) = 0.2   // Parlakl�k geni�li�i
        _Speed("Shine Speed", Range(0.1, 5.0)) = 1.0         // Parlakl�k h�z�
        _ShineDuration("Shine Duration", Range(0.1, 5.0)) = 1.0 // Parlakl�k s�resi
        _ShinePause("Shine Pause", Range(0.1, 5.0)) = 1.0     // Parlakl�k duraklama s�resi
        _MainTex("Texture", 2D) = "white" {}                 // UI texture
        _UseCornerShine("Use Corner Shine", Float) = 0.0      // 0: Yatay shine, 1: K��eden k��eye shine
        _UseShineDuration("Use Shine Duration", Float) = 0.0   // 0: S�rekli shine, 1: Shine s�resi
    }
        SubShader
        {
            LOD 100
   Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off    // Derinlik yaz�m�n� kapat
        ZTest Always  // Her zaman �izim yap, derinlik testi yok
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
                float _ShineDuration; // Parlakl�k s�resi
                float _ShinePause;    // Parlakl�k duraklama s�resi
                float _UseCornerShine; // 0 veya 1, shine efektini kontrol etmek i�in
                float _UseShineDuration; // 0 veya 1, shine s�resi se�ene�i

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

                // Shine s�resi ve duraklama kontrol�
                float totalCycleTime = _ShineDuration + _ShinePause;
                float shineTime = fmod(_Time.y, totalCycleTime); // Zaman� d�ng�sel hale getir
                float shinePos;

                // E�er shine s�resi aktifse
                if (_UseShineDuration == 1.0)
                {
                    // Shine efektinin s�relerine g�re hesapla
                    if (shineTime < _ShineDuration)
                    {
                        shinePos = shineTime / _ShineDuration; // 0 ile 1 aras�nda de�i�ir
                    }
                    else
                    {
                        shinePos = 1.0; // Shine duraklama s�resinde
                    }
                }
                else
                {
                    shinePos = frac(_Time.y * _Speed); // S�rekli shine i�in
                }

                float shineMask;

                if (_UseCornerShine == 1.0)
                {
                    // K��eden k��eye shine efekti: sol alt k��eden sa� �st k��eye do�ru hareket
                    float shineCoord = (i.uv.x + i.uv.y) * 0.5; // x ve y'yi kar��t�rarak k��egen efekti
                    shineMask = smoothstep(shinePos - _ShineWidth, shinePos, shineCoord) *
                                smoothstep(shinePos + _ShineWidth, shinePos, shineCoord);
                }
                else
                {
                    // Yatay shine efekti: sol kenardan sa� kenara hareket
                    shineMask = smoothstep(shinePos - _ShineWidth, shinePos, i.uv.x) *
                                smoothstep(shinePos + _ShineWidth, shinePos, i.uv.x);
                }

                // Parlakl�k rengiyle texture'� harmanlama
                fixed4 shineEffect = _Color * shineMask;

                // Sonu� olarak dokuyu parlakl�k efektiyle birle�tiriyoruz
                return texColor + shineEffect;
            }
            ENDCG
        }
        }
            FallBack "UI/Default"
}
