Shader "Custom/Torus"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TorusMask ("Torus Mask", 2D) = "white" {}
        _Emission ("Emission", Range(0, 1)) = 0.0
        _TilingAndOffset ("Tiling and Offset", Vector) = (1, 1, 0, 0)
        _ScrollSpeed ("Scroll Speed", Range(0, 1)) = 0.1
        _MaskStrength ("Mask Strength", Range(0, 10)) = 5.0
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 0.1

        _SparkleMask ("Sparkle Mask", 2D) = "white" {}
        _SparkleIntensity ("Sparkle Intensity", Range(0, 10)) = 3.5
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Tags 
            { 
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            sampler2D _TorusMask;
            sampler2D _SparkleMask;
            float4 _Color;
            float _Emission;
            float4 _TilingAndOffset;
            float _MaskStrength;
            float _GlowIntensity;
            float _ScrollSpeed;
            float _SparkleIntensity;
            float _PulseSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply scrolling offset based on time
                float2 scrollOffset = _Time.y * _ScrollSpeed;
                o.uv = (v.uv * _TilingAndOffset.xy + _TilingAndOffset.zw) + scrollOffset;

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 maskColor = tex2D(_TorusMask, i.uv);
                half4 texColor = tex2D(_MainTex, i.uv);
                half4 sparkleMaskColor = tex2D(_SparkleMask, i.uv);

                // Calculate the transparency based on the mask texture and Mask Strength
                half transparency = maskColor.r * _MaskStrength;

                // Calculate the sparkle effect using the sparkle mask
                half sparkleEffect = sparkleMaskColor.r * _SparkleIntensity;

                // Make the sparkle pulse over time
                sparkleEffect *= (0.5 + 0.5 * sin(_Time.y * _PulseSpeed * 2 * 3.14159265));

                // Adjust the brightness of the main texture based on the sparkle effect
                texColor.rgb *= (1 + sparkleEffect);

                half3 albedo = texColor.rgb * _Color.rgb;
                half3 emission = texColor.rgb * _Emission;

                // Apply the transparency to the alpha channel
                texColor.a = transparency;

                // Add the glow effect
                albedo += emission * _GlowIntensity;

                half3 finalColor = albedo;
                return half4(finalColor, texColor.a);
            }
            ENDCG
        }
    }
}
