Shader "Example/URPUnlitShaderBasic"
{

    Properties
    {
		//텍스쳐 프로퍼티
		[MainTexture] _BaseMap("Base Map", 2D) = "white" {}
		_OutlineNoise("Outline Noise Map", 2D) = "white" {}
		_OutlineDistance("Outline Distance", Float) = 0.1
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
	}
 
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
			Name "MainPass"
			Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            



            struct Attributes
            {
                float4 positionOS   : POSITION;         
				float2 uv			: TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
				float2 uv			: TEXCOORD0;
            };            

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			CBUFFER_START(UnityPerMaterial)
				float4 _BaseMap_ST;
				half4 _BaseColor;
			CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
         
            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
				return color;
            }
            ENDHLSL
        }
		
		Pass
        {
            Name "CharacterOutline"
			Blend SrcAlpha OneMinusSrcAlpha
            Tags { "LightMode" = "Outline" "RenderType" = "Transparent" "Queue" = "Transparent"} 
            ZWrite Off
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
				float2 uv			: TEXCOORD0;
            };

			TEXTURE2D(_OutlineNoise);
			SAMPLER(sampler_OutlineNoise);

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
				float2 uv			: TEXCOORD0;
            };

            half4 _OutlineColor;
            half _OutlineDistance;

			CBUFFER_START(UnityPerMaterial)
				float4 _OutlineNoise_ST;
			CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = mul(UNITY_MATRIX_M, IN.normalOS.xyz);
                positionWS += normalWS * _OutlineDistance;
                OUT.positionHCS = TransformWorldToHClip(positionWS);
				OUT.uv = TRANSFORM_TEX(IN.uv, _OutlineNoise);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
				half4 Output = SAMPLE_TEXTURE2D(_OutlineNoise, sampler_OutlineNoise, IN.uv);
				Output.a = -Output.r;
                return Output;
            }
            ENDHLSL
        }

		Pass
        {
            Name "CharacterOutline2"
			Blend SrcAlpha OneMinusSrcAlpha
            Tags { "LightMode" = "Outline" "RenderType" = "Transparent" "Queue" = "Transparent"} 
            ZWrite Off
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
				float2 uv			: TEXCOORD0;
            };

			TEXTURE2D(_OutlineNoise);
			SAMPLER(sampler_OutlineNoise);

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
				float2 uv			: TEXCOORD0;
            };

            half4 _OutlineColor;
            half _OutlineDistance;

			CBUFFER_START(UnityPerMaterial)
				float4 _OutlineNoise_ST;
			CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = mul(UNITY_MATRIX_M, IN.normalOS.xyz);
                positionWS += normalWS * (_OutlineDistance + 0.02);
                OUT.positionHCS = TransformWorldToHClip(positionWS);
				OUT.uv = TRANSFORM_TEX(IN.uv, _OutlineNoise);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
				half4 Output = SAMPLE_TEXTURE2D(_OutlineNoise, sampler_OutlineNoise, IN.uv);
				Output.a = -Output.r;
                return Output;
            }
            ENDHLSL
        }
    }
}