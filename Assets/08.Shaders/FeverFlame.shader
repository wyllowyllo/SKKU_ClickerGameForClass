Shader "Custom/FeverFlame"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _SpriteRect  ("Sprite UV Rect", Vector) = (0, 0, 1, 1)
        _FlameWidth  ("Flame Width",   Range(0.01, 0.3)) = 0.08
        _Intensity   ("Intensity",     Range(0.5, 5))     = 2
        _Speed       ("Flame Speed",   Range(0.5, 8))     = 3
        _NoiseScale  ("Noise Scale",   Range(2, 30))      = 10
        _RainbowSpeed("Rainbow Speed", Range(0.2, 5))     = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"           = "Transparent-1"
            "RenderType"      = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType"     = "Plane"
        }

        Blend SrcAlpha One
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _SpriteRect; // (minU, minV, maxU, maxV)
            float _FlameWidth;
            float _Intensity;
            float _Speed;
            float _NoiseScale;
            float _RainbowSpeed;

            // -------- vertex --------
            v2f vert(appdata v)
            {
                v2f o;

                // 스프라이트 UV 영역의 중심
                float2 uvCenter = (_SpriteRect.xy + _SpriteRect.zw) * 0.5;

                float expand = _FlameWidth * 2.5;
                float sc = 1.0 + expand;

                // 메쉬 버텍스 확장
                v.vertex.xy *= sc;

                // UV를 스프라이트 중심 기준으로 확장 → 바깥 UV는 rect 범위 초과
                o.uv = uvCenter + (v.uv - uvCenter) * sc;

                o.pos   = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            // -------- noise --------
            float2 hash2(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453);
            }

            float gnoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);

                float a = dot(hash2(i), f);
                float b = dot(hash2(i + float2(1, 0)), f - float2(1, 0));
                float c = dot(hash2(i + float2(0, 1)), f - float2(0, 1));
                float d = dot(hash2(i + float2(1, 1)), f - float2(1, 1));

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y) * 0.5 + 0.5;
            }

            float fbm(float2 p)
            {
                float v = 0.0;
                float amp = 0.5;
                for (int j = 0; j < 3; j++)
                {
                    v += amp * gnoise(p);
                    p *= 2.0;
                    amp *= 0.5;
                }
                return v;
            }

            // -------- color --------
            float3 hsv2rgb(float3 c)
            {
                float3 p = abs(frac(c.xxx + float3(0.0, 2.0/3.0, 1.0/3.0)) * 6.0 - 3.0);
                return c.z * lerp(float3(1,1,1), saturate(p - 1.0), c.y);
            }

            // -------- helpers --------
            float inRect(float2 uv)
            {
                float2 lo = step(_SpriteRect.xy, uv);
                float2 hi = step(uv, _SpriteRect.zw);
                return lo.x * lo.y * hi.x * hi.y;
            }

            float safeAlpha(float2 uv)
            {
                float2 clamped = clamp(uv, _SpriteRect.xy, _SpriteRect.zw);
                return tex2D(_MainTex, clamped).a * inRect(uv);
            }

            // -------- fragment --------
            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.uv;

                float srcAlpha = safeAlpha(uv);

                // 12방향으로 근처 알파 탐색
                float expanded = 0.0;
                #define NUM_SAMPLES 12
                #define TWO_PI 6.28318

                for (int k = 0; k < NUM_SAMPLES; k++)
                {
                    float ang = (float)k * TWO_PI / (float)NUM_SAMPLES;
                    float2 dir = float2(cos(ang), sin(ang));

                    float n = fbm(uv * _NoiseScale
                                  + float2(0, _Time.y * _Speed)
                                  + dir * 3.0);
                    float w = _FlameWidth * (0.4 + 0.6 * n);

                    expanded = max(expanded, safeAlpha(uv + dir * w));
                }

                // 외곽선
                float flame = expanded * saturate(1.0 - srcAlpha);

                // 일렁임
                float flicker = fbm(uv * _NoiseScale * 0.7
                                    + _Time.y * _Speed * float2(0.3, 1.0));
                flame *= (0.5 + 0.5 * flicker);

                // 무지개
                float hue = frac(_Time.y * _RainbowSpeed
                                 + uv.x * 0.5
                                 + uv.y * 0.3);
                float3 col = hsv2rgb(float3(hue, 0.85, 1.0));

                float alpha = saturate(flame * _Intensity);
                return fixed4(col * _Intensity, alpha) * IN.color;
            }
            ENDCG
        }
    }
}
