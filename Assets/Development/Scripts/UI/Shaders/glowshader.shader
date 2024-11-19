Shader "Custom/NeonBorderUI"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _NeonColor("Neon Color", Color) = (0, 1, 1, 1)
        _GlowWidth("Glow Width", Range(0.005, 0.05)) = 0.02
        _GlowSpeed("Glow Speed", Range(0.1, 5.0)) = 2.0
        _GlowIntensity("Glow Intensity", Range(0, 5.0)) = 1.5
    }

        SubShader
        {
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
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float2 screenPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _NeonColor;
                float _GlowWidth;
                float _GlowSpeed;
                float _GlowIntensity;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.screenPos = o.vertex.xy / o.vertex.w; // Ekran konumunu hesapla
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Kenarlarý belirlemek için UV koordinatlarýnýn sýnýrlarýný kontrol et
                    float2 border = abs(i.uv - 0.5); // Kenara olan uzaklýk
                    float edgeFactor = max(border.x, border.y);

                    // Kenarlarda parlayan animasyonlu bir glow efekti
                    float glow = sin(_Time.y * _GlowSpeed + edgeFactor * 10.0) * 0.5 + 0.5;
                    float edgeGlow = smoothstep(0.5 - _GlowWidth, 0.5, edgeFactor) * glow * _GlowIntensity;

                    // Ana dokuya kenar glow efektini ekle
                    fixed4 mainColor = tex2D(_MainTex, i.uv);
                    fixed4 neonEffect = _NeonColor * edgeGlow;

                    // Son rengi hesapla
                    return mainColor + neonEffect;
                }
                ENDCG
            }
        }

            FallBack "UI/Default"
}
