		sampler2D _FurMetallicMap;
		sampler2D _MetallicGlossMap;
		
		#include "IFCommonVariables.cginc"

        float _Cutoff;
		half _FurMetallic;
		half _Metallic;
        float _SmoothFactor;

		void surfSkin (Input IN, inout SurfaceOutputStandard o) {
			if (_SkinMode == 0) {
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _FurColor;
				o.Albedo = c.rgb;
			
				if (_UseFurSecondMap != 0) {
					float4 m = tex2D(_FurMetallicMap, IN.uv_MainTex);
					o.Metallic = m.r;
					o.Smoothness = m.a; 
				} else {
					o.Metallic = _FurMetallic;
					o.Smoothness = _FurGlossiness;
				}

				o.Normal = UnpackNormal (tex2D (_FurBumpMap, IN.uv_MainTex)) * _FurBumpScale;
			} else {
				fixed4 c = tex2D (_SkinTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
			
				if (_UseSkinSecondMap != 0) {
					float4 m = tex2D(_MetallicGlossMap, IN.uv_MainTex);
					o.Metallic = m.r;
					o.Smoothness = m.a; 
				} else {
					o.Metallic = _Metallic;
					o.Smoothness = _Glossiness;
				}

				o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex)) * _BumpScale;
			}
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {	
			#include "IFAlphaSurface.cginc"
            
			if (_UseFurSecondMap != 0) {
				float4 m = tex2D(_FurMetallicMap, IN.uv_MainTex);
				o.Metallic = m.r;
				o.Smoothness = m.a * n.r; 
			} else {
				o.Metallic = _FurMetallic;
				o.Smoothness = _FurGlossiness * n.r;
			}

            if (_UseLighting) {
                o.Metallic += (1.0 - NOISEFACTOR) * 0.04;
                o.Metallic  += 0.1 * pow(rim, _RimPower);
                o.Metallic = clamp(o.Metallic, 0, 1);
                o.Smoothness += (1.0 - NOISEFACTOR) * _SmoothFactor;
            }

            o.Smoothness *= CURRENTLAYER;
            o.Smoothness = clamp(o.Smoothness, 0, 1);
        }
		
		#include "IFCommonVert.cginc"
		
