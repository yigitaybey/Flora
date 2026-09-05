Shader "Custom/UVAura"
{
    Properties
    {
        [HDR] _Color ("Aura Rengi (Parlaklık için alttaki Intensity'yi artır)", Color) = (0.6, 0.0, 1.0, 1.0)
        _Radius ("Büyüklük (Sönümlenme Sınırı)", Float) = 0.5
        _Falloff ("Yumuşaklık Derecesi", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        
        // Additive Blending (Üst üste bindiğinde parlaması için)
        Blend SrcAlpha One
        ZWrite Off
        Cull Off // Objenin içini/arkasını da göster

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionOS : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _Radius;
                float _Falloff;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                // Objeyi ekrana çizmek için pozisyonu dönüştür
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Objenin kendi içindeki (Local) koordinatlarını fragman shader'a gönder
                OUT.positionOS = IN.positionOS.xyz;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Objenin kendi merkezine (0,0,0) olan 3 Boyutlu uzaklığı.
                // Bu sayede Yusuf'un asseti hangi yöne dönük olursa olsun kusursuz bir yuvarlak (küre) şeklinde yayılır!
                float dist = length(IN.positionOS.xyz);
                
                // Uzaklığa göre gradient (geçiş) hesapla. Merkezde 1, sınıra gelince 0 olur.
                float gradient = saturate(1.0 - (dist / _Radius));
                
                // Falloff değeri ile geçişi yumuşat
                gradient = pow(gradient, _Falloff);
                
                // Rengi gradient ile çarparak pürüzsüzce sönümle
                half4 finalColor = _Color * gradient;
                return finalColor;
            }
            ENDHLSL
        }
    }
}
