// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DLNK Shaders/ASE/TopBottomSimple"
{
	Properties
	{
		_Color("Color", Color) = (0.8207547,0.8207547,0.8207547,0)
		_MainTex("Albedo", 2D) = "white" {}
		_BumpMap("BumpMap", 2D) = "bump" {}
		_BumpScale("BumpScale", Float) = 1
		_MetallicGlossMap("MetallicGlossMap", 2D) = "white" {}
		_Metallic("Metallic", Float) = 0
		_Glossiness("Glossiness", Float) = 0.5
		_OcclusionMap("Occlusion Map", 2D) = "white" {}
		_OcclusionStrength("Occlusion Strength", Float) = 0
		_DetailMask("DetailMask", 2D) = "white" {}
		_DetailAlbedoMap("DetailAlbedoMap", 2D) = "white" {}
		_DetailNormalMap("DetailNormalMap", 2D) = "bump" {}
		_DetailNormalMapScale("DetailNormalMapScale", Float) = 1
		_Ammount("Ammount", Float) = 0.5
		_ColorTop("ColorTop", Color) = (0.8490566,0.8450516,0.8450516,0)
		_AlbedoTop("AlbedoTop", 2D) = "white" {}
		_BumpMapTop("BumpMapTop", 2D) = "bump" {}
		_BumpScaleTop("BumpScaleTop", Float) = 1
		_MetalnessTop("MetalnessTop", 2D) = "white" {}
		_GlossinessTop("GlossinessTop", Float) = 0.5
		_MetallicTop("MetallicTop", Float) = 0
		_OcclusionTop("Occlusion Top", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		UNITY_DECLARE_TEX2D_NOSAMPLER(_BumpMap);
		uniform float4 _BumpMap_ST;
		SamplerState sampler_BumpMap;
		uniform float _BumpScale;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailNormalMap);
		uniform float4 _DetailNormalMap_ST;
		SamplerState sampler_DetailNormalMap;
		uniform float _DetailNormalMapScale;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailMask);
		SamplerState sampler_DetailMask;
		uniform float4 _DetailMask_ST;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_BumpMapTop);
		uniform float4 _BumpMapTop_ST;
		SamplerState sampler_BumpMapTop;
		uniform float _BumpScaleTop;
		uniform float _Ammount;
		uniform float4 _Color;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MainTex);
		uniform float4 _MainTex_ST;
		SamplerState sampler_MainTex;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailAlbedoMap);
		uniform float4 _DetailAlbedoMap_ST;
		SamplerState sampler_DetailAlbedoMap;
		uniform float4 _ColorTop;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_AlbedoTop);
		uniform float4 _AlbedoTop_ST;
		SamplerState sampler_AlbedoTop;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicGlossMap);
		uniform float4 _MetallicGlossMap_ST;
		SamplerState sampler_MetallicGlossMap;
		uniform float _Metallic;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetalnessTop);
		uniform float4 _MetalnessTop_ST;
		SamplerState sampler_MetalnessTop;
		uniform float _MetallicTop;
		uniform float _Glossiness;
		uniform float _GlossinessTop;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_OcclusionMap);
		SamplerState sampler_OcclusionMap;
		uniform float4 _OcclusionMap_ST;
		uniform float _OcclusionTop;
		uniform float _OcclusionStrength;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 tex2DNode4 = UnpackScaleNormal( SAMPLE_TEXTURE2D( _BumpMap, sampler_BumpMap, uv_BumpMap ), _BumpScale );
			float2 uv_DetailNormalMap = i.uv_texcoord * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
			float2 uv_DetailMask = i.uv_texcoord * _DetailMask_ST.xy + _DetailMask_ST.zw;
			float4 tex2DNode43 = SAMPLE_TEXTURE2D( _DetailMask, sampler_DetailMask, uv_DetailMask );
			float3 lerpResult46 = lerp( tex2DNode4 , BlendNormals( tex2DNode4 , UnpackScaleNormal( SAMPLE_TEXTURE2D( _DetailNormalMap, sampler_DetailNormalMap, uv_DetailNormalMap ), _DetailNormalMapScale ) ) , tex2DNode43.a);
			float2 uv_BumpMapTop = i.uv_texcoord * _BumpMapTop_ST.xy + _BumpMapTop_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 lerpResult11 = lerp( lerpResult46 , BlendNormals( UnpackScaleNormal( SAMPLE_TEXTURE2D( _BumpMapTop, sampler_BumpMapTop, uv_BumpMapTop ), _BumpScaleTop ) , tex2DNode4 ) , saturate( ( normalize( (WorldNormalVector( i , ase_worldPos )) ).y * _Ammount ) ));
			o.Normal = lerpResult11;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode2 = SAMPLE_TEXTURE2D( _MainTex, sampler_MainTex, uv_MainTex );
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float4 lerpResult44 = lerp( ( _Color * tex2DNode2 ) , ( tex2DNode2 * SAMPLE_TEXTURE2D( _DetailAlbedoMap, sampler_DetailAlbedoMap, uv_DetailAlbedoMap ) * _Color ) , tex2DNode43.a);
			float2 uv_AlbedoTop = i.uv_texcoord * _AlbedoTop_ST.xy + _AlbedoTop_ST.zw;
			float temp_output_16_0 = saturate( ( normalize( (WorldNormalVector( i , lerpResult11 )) ).y * _Ammount ) );
			float4 lerpResult18 = lerp( lerpResult44 , ( _ColorTop * SAMPLE_TEXTURE2D( _AlbedoTop, sampler_AlbedoTop, uv_AlbedoTop ) ) , temp_output_16_0);
			o.Albedo = lerpResult18.rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			float4 tex2DNode6 = SAMPLE_TEXTURE2D( _MetallicGlossMap, sampler_MetallicGlossMap, uv_MetallicGlossMap );
			float2 uv_MetalnessTop = i.uv_texcoord * _MetalnessTop_ST.xy + _MetalnessTop_ST.zw;
			float4 tex2DNode7 = SAMPLE_TEXTURE2D( _MetalnessTop, sampler_MetalnessTop, uv_MetalnessTop );
			float4 lerpResult14 = lerp( ( tex2DNode6 * _Metallic ) , ( tex2DNode7 * _MetallicTop ) , temp_output_16_0);
			o.Metallic = lerpResult14.r;
			float lerpResult19 = lerp( ( tex2DNode6.a * _Glossiness ) , ( tex2DNode7.a * _GlossinessTop ) , temp_output_16_0);
			o.Smoothness = lerpResult19;
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			float4 tex2DNode48 = SAMPLE_TEXTURE2D( _OcclusionMap, sampler_OcclusionMap, uv_OcclusionMap );
			float lerpResult51 = lerp( tex2DNode48.r , ( tex2DNode48.r + ( 1.0 - _OcclusionTop ) ) , temp_output_16_0);
			o.Occlusion = saturate( ( lerpResult51 + ( 1.0 - _OcclusionStrength ) ) );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Standard"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
0;0;1920;1019;1555.097;-75.84776;1.313544;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;42;-1357.823,-114.2855;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;38;-1486.523,121.6645;Inherit;False;Property;_DetailNormalMapScale;DetailNormalMapScale;12;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1180.168,304.4252;Inherit;False;Property;_BumpScale;BumpScale;3;0;Create;True;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-1166.3,-98.40002;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;8;-1143.769,52.22521;Inherit;False;Property;_Ammount;Ammount;13;0;Create;True;0;0;False;0;False;0.5;1.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-1500.186,207.7091;Inherit;True;Property;_DetailNormalMap;DetailNormalMap;11;0;Create;True;0;0;False;0;False;-1;None;3fa706993e148d340b93d582fc2b05d9;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-958.4177,244.5252;Inherit;True;Property;_BumpMap;BumpMap;2;0;Create;True;0;0;False;0;False;-1;None;197658984fe1d92409976d98d4d02cd9;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1180.168,409.7252;Inherit;False;Property;_BumpScaleTop;BumpScaleTop;17;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-927.9686,-60.87477;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;43;-180.5155,-33.60611;Inherit;True;Property;_DetailMask;DetailMask;9;0;Create;True;0;0;False;0;False;-1;None;23eb7bb3b03349e4b8cb70d2ac08c44a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;39;-598.3251,196.3061;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;5;-958.6176,430.6252;Inherit;True;Property;_BumpMapTop;BumpMapTop;16;0;Create;True;0;0;False;0;False;-1;None;d7c884eb1ccd7a54a85b976afa91fc66;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;46;-278.3319,271.5661;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;10;-769.3687,49.62525;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;47;-573.8795,384.5309;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;11;-60.83846,367.4368;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;15;-578.2676,15.82523;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;52;-43.20768,1138.505;Inherit;False;Property;_OcclusionTop;Occlusion Top;21;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-389.118,48.3252;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;57;61.87585,1259.351;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-161.4265,896.813;Inherit;True;Property;_OcclusionMap;Occlusion Map;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-729.7998,-565.7997;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;False;0;False;-1;None;84839d10a54143e49bc6829c23a25b5d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-1027.37,-566.9212;Inherit;True;Property;_DetailAlbedoMap;DetailAlbedoMap;10;0;Create;True;0;0;False;0;False;-1;None;06b5f4cdfd9a3f3468f5001df14b4708;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;56;253.6533,1125.369;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;-658.8688,-745.9743;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;False;0.8207547,0.8207547,0.8207547,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;16;-393.0181,141.9252;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;56.62176,790.4156;Inherit;False;Property;_OcclusionStrength;Occlusion Strength;8;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-341.6853,-499.8063;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-849.9683,923.2254;Inherit;False;Property;_GlossinessTop;GlossinessTop;19;0;Create;True;0;0;False;0;False;0.5;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-733.8995,-187.3;Inherit;True;Property;_AlbedoTop;AlbedoTop;15;0;Create;True;0;0;False;0;False;-1;None;42379daac53b2cb4c87f4f4d537e1007;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-666.1178,732.2252;Inherit;True;Property;_MetalnessTop;MetalnessTop;18;0;Create;True;0;0;False;0;False;-1;None;fd3916fb3489d604d88ea31e890a247c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-847.3686,842.6254;Inherit;False;Property;_MetallicTop;MetallicTop;20;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-848.6683,665.8253;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-352.0436,-647.963;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-664.6179,544.8253;Inherit;True;Property;_MetallicGlossMap;MetallicGlossMap;4;0;Create;True;0;0;False;0;False;-1;None;06b5f4cdfd9a3f3468f5001df14b4708;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;-662.7689,-372.8746;Inherit;False;Property;_ColorTop;ColorTop;14;0;Create;True;0;0;False;0;False;0.8490566,0.8450516,0.8450516,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;59;109.1635,699.7813;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-851.2681,746.4254;Inherit;False;Property;_Glossiness;Glossiness;6;0;Create;True;0;0;False;0;False;0.5;0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;51;198.4846,911.2618;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;269.4158,718.1709;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-318.2681,853.6758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-315.668,756.8263;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;44;-101.0026,-561.2209;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-315.6683,641.775;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-314.3682,543.6255;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-393.6689,-225.9746;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;14;-149.2681,556.6255;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;35.12896,-251.0268;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;19;-147.9685,691.8256;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;54;294.3732,609.1467;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;247.1372,17.24213;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;DLNK Shaders/ASE/TopBottomSimple;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;Standard;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;42;0
WireConnection;35;5;38;0
WireConnection;4;5;12;0
WireConnection;9;0;1;2
WireConnection;9;1;8;0
WireConnection;39;0;4;0
WireConnection;39;1;35;0
WireConnection;5;5;13;0
WireConnection;46;0;4;0
WireConnection;46;1;39;0
WireConnection;46;2;43;4
WireConnection;10;0;9;0
WireConnection;47;0;5;0
WireConnection;47;1;4;0
WireConnection;11;0;46;0
WireConnection;11;1;47;0
WireConnection;11;2;10;0
WireConnection;15;0;11;0
WireConnection;17;0;15;2
WireConnection;17;1;8;0
WireConnection;57;0;52;0
WireConnection;56;0;48;1
WireConnection;56;1;57;0
WireConnection;16;0;17;0
WireConnection;37;0;2;0
WireConnection;37;1;34;0
WireConnection;37;2;30;0
WireConnection;28;0;30;0
WireConnection;28;1;2;0
WireConnection;59;0;53;0
WireConnection;51;0;48;1
WireConnection;51;1;56;0
WireConnection;51;2;16;0
WireConnection;58;0;51;0
WireConnection;58;1;59;0
WireConnection;27;0;7;4
WireConnection;27;1;24;0
WireConnection;21;0;6;4
WireConnection;21;1;23;0
WireConnection;44;0;28;0
WireConnection;44;1;37;0
WireConnection;44;2;43;4
WireConnection;26;0;7;0
WireConnection;26;1;25;0
WireConnection;20;0;6;0
WireConnection;20;1;22;0
WireConnection;29;0;31;0
WireConnection;29;1;3;0
WireConnection;14;0;20;0
WireConnection;14;1;26;0
WireConnection;14;2;16;0
WireConnection;18;0;44;0
WireConnection;18;1;29;0
WireConnection;18;2;16;0
WireConnection;19;0;21;0
WireConnection;19;1;27;0
WireConnection;19;2;16;0
WireConnection;54;0;58;0
WireConnection;0;0;18;0
WireConnection;0;1;11;0
WireConnection;0;3;14;0
WireConnection;0;4;19;0
WireConnection;0;5;54;0
ASEEND*/
//CHKSM=0E5DB934F45F9CE3D9270982ED695E9280EC347B