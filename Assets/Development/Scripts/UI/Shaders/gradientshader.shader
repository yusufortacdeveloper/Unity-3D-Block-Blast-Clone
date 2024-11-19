Shader "Custom/UIGradientShader"
{
    Properties
    {
        _Color1("Start Color", Color) = (0.0, 0.0, 1.0, 1.0) // Mavi ba�lang�� rengi
        _Color2("End Color", Color) = (0.0, 1.0, 1.0, 1.0)   // A��k mavi biti� rengi
        _Speed("Gradient Speed", Range(0.1, 10.0)) = 1.0     // Renk ge�i� h�z�n� ayarlar
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

            float4 _Color1;
            float4 _Color2;
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
                // Zaman bazl� de�i�im i�in
                float lerpFactor = (sin(_Time.y * _Speed) + 1.0) / 2.0;

            // Zamanla de�i�en gradient rengi
            fixed4 gradientColor = lerp(_Color1, _Color2, lerpFactor);

            return gradientColor;
        }
        ENDCG
    }
    }
        FallBack "UI/Default"
}
