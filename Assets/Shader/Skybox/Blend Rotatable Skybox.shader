Shader "Custom/Skybox/Blend Rotatable Skybox"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (0.5, 0.5, 0.5, 1)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        _FixedRotation ("Fixed Rotation", Range(0, 360)) = 0
        _FixedRotationAxis ("Fixed Rotation Axis", Vector) = (0, 0, 1)  // Ä¬ÈÏ Z Öá
        _MainRotation ("Main Rotation", Range(0, 360)) = 0
        _MainRotationAxis ("Main Rotation Axis", Vector) = (0, 1, 0)  // Ä¬ÈÏ Y Öá
        [NoScaleOffset] _Tex ("Cubemap (HDR)", CUBE) = "black" {}
        [NoScaleOffset] _Tex_Blend ("Cubemap Blend (HDR)", CUBE) = "black" {}
        _CubemapTransition ("Cubemap Transition", Range(0, 1)) = 0
        [Gamma] _TintColor ("Cubemap Tint Color", Color) = (0.5, 0.5, 0.5, 1)
        _SkyboxHeightOffset ("Skybox Height Offset", Float) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" "PreviewType"="Skybox" }
        LOD 0

        CGINCLUDE
        #include "UnityCG.cginc"

        half4 _Tint;
        half _Exposure;
        float _FixedRotation;
        float3 _FixedRotationAxis;
        float _MainRotation;
        float3 _MainRotationAxis;
        samplerCUBE _Tex;
        samplerCUBE _Tex_Blend;
        half _CubemapTransition;
        half4 _TintColor;
        float _SkyboxHeightOffset;

        float4x4 rotationMatrix(float3 axis, float angle)
        {
            axis = normalize(axis);
            float s = sin(angle);
            float c = cos(angle);
            float oc = 1.0 - c;

            return float4x4(
                oc * axis.x * axis.x + c, oc * axis.x * axis.y - axis.z * s, oc * axis.x * axis.z + axis.y * s, 0,
                oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c, oc * axis.y * axis.z - axis.x * s, 0,
                oc * axis.x * axis.z - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c, 0,
                0, 0, 0, 1
            );
        }

        struct appdata_t
        {
            float4 vertex : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
            float3 worldPos : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert(appdata_t v)
        {
            v2f o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            float mainAngle = _MainRotation * UNITY_PI / 180.0;
            float4x4 mainRotMat = rotationMatrix(_MainRotationAxis, mainAngle);
            float3 mainRotatedVertex = mul(mainRotMat, v.vertex).xyz;

            float angle = _FixedRotation * UNITY_PI / 180.0;
            float4x4 rotMat = rotationMatrix(_FixedRotationAxis, angle);
            float3 rotatedVertex = mul(rotMat, mainRotatedVertex).xyz;

            rotatedVertex.z += _SkyboxHeightOffset;

            o.vertex = UnityObjectToClipPos(rotatedVertex);
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            return o;
        }

        half4 frag(v2f i) : SV_Target
        {
            half4 cubemapColor = texCUBE(_Tex, normalize(i.worldPos));
            half4 blendedColor = texCUBE(_Tex_Blend, normalize(i.worldPos));
            half3 finalColor = lerp(cubemapColor.rgb, blendedColor.rgb, _CubemapTransition) * _TintColor.rgb * _Exposure;
            return half4(finalColor, 1.0);
        }
        ENDCG

        Pass
        {
            Name "Unlit"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    Fallback "Skybox/Cubemap"
}