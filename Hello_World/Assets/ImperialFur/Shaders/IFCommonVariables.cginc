		sampler2D _MainTex;
		sampler2D _SkinTex;
		sampler2D _ControlTex;
		sampler2D _NoiseTex;
		sampler2D _FurBumpMap;
		sampler2D _BumpMap;
		
	    struct Input {
      		fixed alpha;
          	float2 uv_MainTex;
//          	float3 worldRefl;
          	half3 viewDir;
      	};

  	 	uniform float3 Displacement;
		half4 _RimColor;
		fixed4 _Color;
		fixed4 _FurColor;		
		half _FurGlossiness;
		half _Glossiness;
	    half _RimPower;
 	 	half _RimLightMode;
 	 	half _StrandThickness;
        fixed _ShadowStrength;
		fixed _MaxHairLength;
 	 	fixed _SkinMode;
 	 	half _BumpScale;
 	 	half _FurBumpScale; 	 	 	 	
 	 	fixed _UseFurSecondMap;
 	 	fixed _UseSkinSecondMap;
 	 	fixed _UseStrengthMap;
 	 	fixed _UseHeightMap;
 	 	fixed _UseBiasMap;
 	 	float _EdgeFade;
        float _UseLighting;
        float _DebugLighting;
        float _BiasFactor;
        float _BiasHeightMap;

