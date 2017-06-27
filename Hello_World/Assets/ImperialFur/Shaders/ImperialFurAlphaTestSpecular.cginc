        sampler2D _FurSpecularMap;
        sampler2D _SpecGlossMap;

        #include "IFCommonVariables.cginc"

        float _Cutoff;
        fixed3 _FurSpecularColor;
        fixed3 _SpecularColor;
        float _SmoothFactor;
        float _SpecularShift;


		void surfSkin (Input IN, inout SurfaceOutputStandardSpecular o) {
            if (_SkinMode == 0) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _FurColor;
                o.Albedo = c.rgb;

                if (_UseFurSecondMap != 0) {
                    float4 m = tex2D(_FurSpecularMap, IN.uv_MainTex);
                    o.Specular = m.rgb;
                    o.Smoothness = m.a;
                }
                else {
                    o.Specular = _FurSpecularColor;
                    o.Smoothness = _FurGlossiness;
                }

                o.Normal = UnpackNormal(tex2D(_FurBumpMap, IN.uv_MainTex)) * _FurBumpScale;
            }
            else {
                fixed4 c = tex2D(_SkinTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;

                if (_UseSkinSecondMap != 0) {
                    float4 m = tex2D(_SpecGlossMap, IN.uv_MainTex);
                    o.Specular = m.rgb;
                    o.Smoothness = m.a;
                }
                else {
                    o.Specular = _SpecularColor;
                    o.Smoothness = _Glossiness;
                }

                o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
            }
		}

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {	
			#include "IFAlphaSurface.cginc"

            if (_UseFurSecondMap != 0) {
                float4 m = tex2D(_FurSpecularMap, IN.uv_MainTex);
                o.Specular = m.rgb;
                o.Smoothness = m.a;
            }
            else {
                o.Specular = _FurSpecularColor;
                o.Smoothness = _FurGlossiness;
            }

            if (_UseLighting) {
                float3 specColor;
                if (_DebugLighting)
                    specColor = lerp(float3(1, 0, 0), o.Specular, 0.5);
                else
                    specColor = lerp(o.Albedo, o.Specular, 0.5);
                o.Specular = lerp(specColor, o.Specular, clamp(NOISEFACTOR / alphaFactor + _SpecularShift, 0, 1));
                o.Specular += specColor * pow(rim, _RimPower);
                o.Smoothness += (1.0 - NOISEFACTOR) * _SmoothFactor;
            }

            o.Smoothness *= CURRENTLAYER;
            o.Specular = clamp(o.Specular, 0, 1);
            o.Smoothness = clamp(o.Smoothness, 0, 1);
        }
		
		#include "IFCommonVert.cginc"
		
