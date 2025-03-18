Shader "Custom/Mask/TransparentMask"
{
    Properties
    {
        _StencilRef ("Stencil Reference", Int) = 1
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0.0,1.0)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Background" }
        LOD 100

        ZWrite Off
        ColorMask 0

        Stencil
        {
            Ref [_StencilRef]
            Comp Always
            Pass Replace
        }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1)
            };

            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MaskTex);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 maskCol = tex2D(_MaskTex, i.uv);
                clip(maskCol.a - _Cutoff); 

                UNITY_LIGHT_ATTENUATION(atten, i, i.pos);
                return fixed4(0, 0, 0, 0) * atten;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}