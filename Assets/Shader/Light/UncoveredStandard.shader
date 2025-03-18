Shader "Custom/Light/UncoveredStandard"
{
    Properties
    {
        [Enum(Opaque,0,Cutout,1,Fade,2,Transparent,3)] _RenderingMode ("Rendering Mode", Float) = 0
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha cutoff", Range(0.0,1.0)) = 0.5
        _Specular ("Specular", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0.0,1.0)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Cutout" "Queue"="AlphaTest" "IgnoreProjector"="True" "PreviewType"="Plane" }
        LOD 400
        ZTest [_ZTest]
        Cull Off
        ColorMask [_ColorWriteMask]

        Stencil
        {
            Ref 1
            Comp Always  // 修改为 Always，确保所有部分都会被渲染
        }

        CGPROGRAM
        #pragma surface surf StandardSpecular alpha:fade keepalpha alphatest:_Cutoff

        sampler2D _MainTex;
        half _Glossiness;
        half4 _Specular;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Specular = _Specular;
            o.Normal = -float3(0,1,1);
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        Pass
        {
            Tags { "LightMode"="ShadowCaster" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;

            v2f vert(appdata_base v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                clip(c.a - _Cutoff);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}