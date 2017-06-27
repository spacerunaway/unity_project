﻿Shader "Imperial Fur/Simple/Metallic Skin/ 1 Shell" {
    Properties {
        _FurColor ("Fur Color", Color) = (1,1,1,1)
        _MainTex ("Fur Texture (RGB)", 2D) = "white" { }
        _ControlTex ("Control Texture (RGB)", 2D) = "white" { }               
        _NoiseTex ("Noise Texture (RGB)", 2D) = "white" { }
        _FurSpecular ("Specular", Color) = (0.2,0.2,0.2,1.0)
		_FurSpecularMap("Specular", 2D) = "white" {}
		_FurGlossiness("Smoothness", Range(0.0, 1.0)) = 0.5
                        
        _Color ("Color", Color) = (1,1,1,1)
		_SkinTex ("Skin Albedo (RGB)", 2D) = "white" {}

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

		_BumpScale("Normal Scale", Float) = 1.0
		[Normal]_BumpMap("Normal Map", 2D) = "bump" {}	
		
        _StrandThickness ("Strand Density", Range(0,10)) = 1
        _MaxHairLength ("Max Strand Length", Range(0,0.5)) = 0.08
        _EdgeFade ("Edge Fade", Range(0,1)) = 0.4
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.75
        
       	_RimWidth ("Rim Width", Range(0.1,1)) = 0.4
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _BiasFactor("Bias Factor", Range(0.0,2.0)) = 1.0

     	_Reflectivity ("Reflectivity", Range(0,1)) = 0.1
        
        _Cube("Reflection Map", Cube) = "" {}
              
        [HideInInspector] _RimLightMode ("__rimlightmode", Float) = 0.0
        [HideInInspector] _ReflectionMode ("__reflectionmode", Float) = 0.0
        
        [HideInInspector] _UseFurSecondMap ("__usefursecondmap", Float) = 0.0
        [HideInInspector] _UseHeightMap ("__useheightmap", Float) = 0.0
        [HideInInspector] _UseBiasMap ("__usebiasmap", Float) = 0.0
        [HideInInspector] _UseSkinSecondMap ("__useskinsecondmap", Float) = 0.0
        [HideInInspector] _UseStrengthMap ("__usestrengthmap", Float) = 0.0
        [HideInInspector] _BiasHeightMap ("__biasheightmap", Float) = 0.0
    }
	
    Category
	{
		ZWrite On
		Tags { "QUEUE"="Transparent" "RenderType"="Opaque" "IgnoreProjector"="True"}	
		Blend SrcAlpha OneMinusSrcAlpha
   SubShader { // Unity chooses the subshader that fits the GPU best
		LOD 200
		  
   		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vertSkin
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _SkinTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;

 	 	float _UseSkinSecondMap;
 	 	float _UseSkinNormal;
		sampler2D _ControlTex;
		fixed _MaxHairLength;
		float _UseBiasMap;
        float _BiasHeightMap;
        float _BiasFactor;

		struct Input {
			float2 uv_SkinTex;
		};

		float _BumpScale;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_SkinTex, IN.uv_SkinTex) * _Color;
			o.Albedo = c.rgb;

			if (_UseSkinSecondMap != 0) {
				float4 m = tex2D(_MetallicGlossMap, IN.uv_SkinTex);
				o.Metallic = m.r;
				o.Smoothness = m.a; 
			} else {
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
			}

			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_SkinTex)) * _BumpScale;
		}
		
		void vertSkin (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);

			if (_UseBiasMap) {
	          	float4 c = tex2Dlod (_ControlTex, float4(v.texcoord.xy,0,0));		
				fixed skinLength = _MaxHairLength;
                if (_BiasHeightMap != 0) {
                    skinLength *= c.r;
                }
				skinLength = skinLength - (skinLength * c.b);
            	v.vertex.xyz -= v.normal * (skinLength * _BiasFactor);
            }
		}         
		ENDCG
   	
   
      Pass { // some shaders require multiple passes	
            CGPROGRAM // here begins the part in Unity's Cg
            #pragma multi_compile_fwdbase
            #pragma vertex vert 
         	#pragma fragment frag 
			#pragma target 3.0
			#define CURRENTLAYER 1.0
			#define NOISEFACTOR 0.05
			#define USESSKIN 1
 			#include "ImperialFurSimple.cginc"
 			ENDCG // here ends the part in Cg 
      }

      
   }
   }
   Fallback "VertexLit"
   CustomEditor "ImperialFurShaderSimpleGUI"
}
