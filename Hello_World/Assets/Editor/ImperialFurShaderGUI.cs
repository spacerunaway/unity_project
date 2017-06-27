﻿using System;
using UnityEngine;
using UnityEditor;

public class ImperialFurShaderGUI : ShaderGUI
{
    private enum RimLightMode
    {
        None,
        Natural,
        AmbientLight,
        UserDefined
    }

    private enum SkinMode
    {
        Disabled,
        Enabled
    }

    private enum ShadowMode
    {
        Depth,
        UVShift
    }

    private static class Styles
    {
        public static GUIStyle optionsButton = "PaneOptions";
        public static GUIContent uvSetLabel = new GUIContent("UV Set");
        public static GUIContent[] uvSetOptions = new GUIContent[] {
            new GUIContent ("UV channel 0"),
            new GUIContent ("UV channel 1")
        };
        public static string emptyTootip = "";
        public static GUIContent furText = new GUIContent("Albedo", "Albedo (RGB) and Heightmap (A)");
        public static GUIContent controlText = new GUIContent("Control", "Height Map (R) Strength Map (G)");
        public static GUIContent noiseText = new GUIContent("Noise", "Noise");
        public static GUIContent strandText = new GUIContent("Max Strand Length", "");
        public static GUIContent densityText = new GUIContent("Strand Density", "");
        public static GUIContent fadeText = new GUIContent("Edge Fade", "");
        public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Fur Transparency (A)");
        public static GUIContent specularMapText = new GUIContent("Specular", "Specular (RGB) and Smoothness (A)");
        public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and Smoothness (A)");
        public static GUIContent smoothnessText = new GUIContent("Smoothness", "");
        public static GUIContent glossinessText = new GUIContent("Glossiness", "");
        public static GUIContent normalMapText = new GUIContent("Normal Map", "");
        public static GUIContent cutoffText = new GUIContent("Alpha Cutoff", "");
        public static string whiteSpaceString = " ";
        public static string primaryMapsText = "Fur Properties";
        public static string secondaryMapsText = "Skin Properties";
        public static string lightText = "Lighting Properties";
        public static string shadowStrengthText = "Shadow Strength";
        public static string skinModeText = "Skin Texture";
        public static string shadowModeText = "Shadow Style";
        public static string rimLightModeText = "Rim Light Mode";
        public static string rimColorText = "Rim Color";
        public static string rimWidthText = "Rim Width";
        public static string rimIntensityText = "Rim Factor";
        public static string specularShiftText = "Specular Shift";
        public static string smoothFactorText = "Smoothness Factor";
        public static string biasFactorText = "Bias Factor";
        public static string useLightingText = "Use Added Specular";
        public static string debugLightingText = "Show Added Specular Debug View";
        public static string biasHeightText = "Account for Heightmap";
        public static readonly string[] shadowNames = Enum.GetNames(typeof(ShadowMode));
        public static readonly string[] rimLightNames = Enum.GetNames(typeof(RimLightMode));
        public static readonly string[] skinNames = Enum.GetNames(typeof(SkinMode));
    }

    MaterialProperty rimLightMode = null;
    MaterialProperty skinMode = null;
    MaterialProperty furColor = null;
    MaterialProperty rimColor = null;
    MaterialProperty rimIntensity = null;
    MaterialProperty furMap = null;
    MaterialProperty furMetallicMap = null;
    MaterialProperty furMetallic = null;
    MaterialProperty furSmoothness = null;
    MaterialProperty noiseMap = null;
    MaterialProperty controlMap = null;
    MaterialProperty strandDensity = null;
    MaterialProperty strandLength = null;
    MaterialProperty shadowStrength = null;
    MaterialProperty edgeFade = null;
    MaterialProperty skinColor = null;
    MaterialProperty skinMap = null;
    MaterialProperty furSpecular = null;
    MaterialProperty furSpecularMap = null;
    MaterialProperty furSpecularColor = null;
    MaterialProperty specular = null;
    MaterialProperty specularMap = null;
    MaterialProperty specularColor = null;
    MaterialProperty metallicMap = null;
    MaterialProperty metallic = null;
    MaterialProperty smoothness = null;
    MaterialProperty bumpScale = null;
    MaterialProperty bumpMap = null;
    MaterialProperty useFurSecondMap = null;
    MaterialProperty useStrengthMap = null;
    MaterialProperty useBiasMap = null;
    MaterialProperty useSkinSecondMap = null;
    MaterialProperty useHeightMap = null;
    MaterialProperty useFurAlbedo = null;
    MaterialProperty useSkinAlbedo = null;
    MaterialProperty cutoff = null;
    MaterialProperty useLighting = null;
    MaterialProperty debugLighting = null;
    MaterialProperty smoothFactor = null;
    MaterialProperty specularShift = null;
    MaterialProperty biasFactor = null;
    MaterialProperty biasHeightMap = null;

    MaterialEditor m_MaterialEditor;

    bool useHeight;
    bool useBias;
    bool useStrength;

    public void FindProperties(MaterialProperty[] props)
    {
        skinMode = FindProperty("_SkinMode", props);
        rimLightMode = FindProperty("_RimLightMode", props);
        furColor = FindProperty("_FurColor", props);
        rimColor = FindProperty("_RimColor", props);
        rimIntensity = FindProperty("_RimPower", props);
        furSpecularMap = FindProperty("_FurSpecularMap", props, false);
        furSpecularColor = FindProperty("_FurSpecularColor", props, false);
        furSpecular = FindProperty("_FurSpecular", props, false);
        furMetallicMap = FindProperty("_FurMetallicMap", props, false);
        furMetallic = FindProperty("_FurMetallic", props, false);
        furSmoothness = FindProperty("_FurGlossiness", props, false);
        skinColor = FindProperty("_Color", props, false);
        furMap = FindProperty("_MainTex", props);
        skinMap = FindProperty("_SkinTex", props, false);
        noiseMap = FindProperty("_NoiseTex", props, false);
        controlMap = FindProperty("_ControlTex", props, false);
        specularMap = FindProperty("_SpecGlossMap", props, false);
        specular = FindProperty("_Specular", props, false);
        specularColor = FindProperty("_SpecularColor", props, false);
        metallicMap = FindProperty("_MetallicGlossMap", props, false);
        metallic = FindProperty("_Metallic", props, false);
        smoothness = FindProperty("_Glossiness", props, false);
        bumpScale = FindProperty("_BumpScale", props, false);
        bumpMap = FindProperty("_BumpMap", props);
        strandDensity = FindProperty("_StrandThickness", props);
        strandLength = FindProperty("_MaxHairLength", props);
        edgeFade = FindProperty("_EdgeFade", props);
        shadowStrength = FindProperty("_ShadowStrength", props, false);
        useFurSecondMap = FindProperty("_UseFurSecondMap", props, false);
        useSkinSecondMap = FindProperty("_UseSkinSecondMap", props, false);
        useStrengthMap = FindProperty("_UseStrengthMap", props, false);
        useHeightMap = FindProperty("_UseHeightMap", props, false);
        useBiasMap = FindProperty("_UseBiasMap", props, false);
        useFurAlbedo = FindProperty("_UseFurAlbedo", props, false);
        useSkinAlbedo = FindProperty("_UseSkinAlbedo", props, false);
        cutoff = FindProperty("_Cutoff", props, false);
        useLighting = FindProperty("_UseLighting", props, false);
        debugLighting = FindProperty("_DebugLighting", props, false);
        specularShift = FindProperty("_SpecularShift", props, false);
        smoothFactor = FindProperty("_SmoothFactor", props, false);
        biasFactor = FindProperty("_BiasFactor", props, false);
        biasHeightMap = FindProperty("_BiasHeightMap", props, false);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;

        ShaderPropertiesGUI(material);

        SetMaterialKeywords(material);
    }

    public void ShaderPropertiesGUI(Material material)
    {
        bool cameraIsDeferred = (Camera.main.renderingPath == RenderingPath.DeferredShading);
        bool settingsIsDeferred = (Camera.main.renderingPath == RenderingPath.UsePlayerSettings && PlayerSettings.renderingPath == RenderingPath.DeferredShading);
        if (!cameraIsDeferred && !settingsIsDeferred && cutoff != null)
        {
            GUIStyle warningStyle = new GUIStyle();
            warningStyle.normal.textColor = Color.red;
            warningStyle.fontStyle = FontStyle.Bold;
            warningStyle.wordWrap = true; 
            GUILayout.Label("WARNING: Alpha Tested shaders require deferred shading to work correctly.", warningStyle);
        }
        
        // Use default labelWidth
        EditorGUIUtility.labelWidth = 0f;

        // Primary properties
        GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
        DoFurArea(material);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label(Styles.secondaryMapsText, EditorStyles.boldLabel);
        DoSkinArea(material);
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label(Styles.lightText, EditorStyles.boldLabel);
        DoLightArea(material);

        EditorGUILayout.Space();

    }

    void DoFurArea(Material material)
    {
        if (useFurAlbedo != null) {
            if (furMap.textureValue != null)
                m_MaterialEditor.TexturePropertySingleLine(Styles.furText, furMap);
            else
                m_MaterialEditor.TexturePropertySingleLine(Styles.furText, furMap, furColor);
        } else {
            m_MaterialEditor.TexturePropertySingleLine(Styles.furText, furMap, furColor);
        }

        if (furSpecularMap != null && furSpecularColor != null)
        {
            if (furSpecularMap.textureValue == null)
                m_MaterialEditor.TexturePropertyTwoLines(Styles.specularMapText, furSpecularMap, furSpecularColor, Styles.smoothnessText, furSmoothness);
            else
                m_MaterialEditor.TexturePropertySingleLine(Styles.specularMapText, furSpecularMap);
        }
        else if (furMetallicMap != null && furMetallic != null)
        {
            if (furMetallicMap.textureValue == null)
                m_MaterialEditor.TexturePropertyTwoLines(Styles.metallicMapText, furMetallicMap, furMetallic, Styles.smoothnessText, furSmoothness);
            else
                m_MaterialEditor.TexturePropertySingleLine(Styles.metallicMapText, furMetallicMap);
        }
        else if (furSpecular != null)
        {
            if (furSpecularMap.textureValue == null)
                m_MaterialEditor.TexturePropertyTwoLines(Styles.specularMapText, furSpecularMap, furSpecular, Styles.glossinessText, furSmoothness);
            else
                m_MaterialEditor.TexturePropertySingleLine(Styles.specularMapText, furSpecularMap);
        }

        m_MaterialEditor.TexturePropertySingleLine(Styles.noiseText, noiseMap);
        m_MaterialEditor.TexturePropertySingleLine(Styles.controlText, controlMap);

        DoControlArea();

        EditorGUILayout.Space();

        if (cutoff != null)
            m_MaterialEditor.RangeProperty(cutoff, Styles.cutoffText.text);
        m_MaterialEditor.RangeProperty(strandDensity, Styles.densityText.text);
        m_MaterialEditor.RangeProperty(strandLength, Styles.strandText.text);
        if (cutoff != null)
            edgeFade.floatValue = 0;
        else
            m_MaterialEditor.RangeProperty(edgeFade, Styles.fadeText.text);
    }

    void DoControlArea() {
        if (controlMap.textureValue != null) {
            EditorGUIUtility.labelWidth = 140f;
            EditorGUI.indentLevel++;

            useHeight = (useHeightMap.floatValue == 1.0);
            useHeight = EditorGUILayout.Toggle("Use Height Map (R)", useHeight);
            if (useHeight)
                useHeightMap.floatValue = 1.0f;
            else
                useHeightMap.floatValue = 0f;

            useStrength = (useStrengthMap.floatValue == 1.0);
            useStrength = EditorGUILayout.Toggle("Use Stiffness Map (G)", useStrength);
            if (useStrength)
                useStrengthMap.floatValue = 1.0f;
            else
                useStrengthMap.floatValue = 0f;

            useBias = (useBiasMap.floatValue == 1.0);
            useBias = EditorGUILayout.Toggle("Use Bias Map (B)", useBias);
            bool biasHeight = false;
            if (useBias)
            {
                useBiasMap.floatValue = 1.0f;
                m_MaterialEditor.RangeProperty(biasFactor, Styles.biasFactorText);
                biasHeight = (biasHeightMap.floatValue == 1.0);
                biasHeight = EditorGUILayout.Toggle("Account for Height Map", biasHeight);
            }
            else
                useBiasMap.floatValue = 0f;

            if (biasHeight)
                biasHeightMap.floatValue = 1.0f;
            else
                biasHeightMap.floatValue = 0f;

            EditorGUIUtility.labelWidth = 0f;
            EditorGUI.indentLevel--;
        }
    }

    void DoLightArea(Material material)
    {
        m_MaterialEditor.RangeProperty(shadowStrength, Styles.shadowStrengthText);

        EditorGUI.showMixedValue = rimLightMode.hasMixedValue;
        var mode = (RimLightMode)rimLightMode.floatValue;

        bool lightUse = false;
        bool lightDebug = false;
        if (useLighting != null)
        {
            if (useLighting.floatValue > 0.5f)
                lightUse = true;

            if (debugLighting.floatValue > 0.5f)
                lightDebug = true;
        }

        EditorGUI.BeginChangeCheck();

        if (useLighting != null)
        {
            lightUse = EditorGUILayout.Toggle("Use Added Specular", lightUse);
            if (lightUse)
            {
                if (specularMap != null && specularColor != null)
                {
                    lightDebug = EditorGUILayout.Toggle("Debug Added Specular", lightDebug);
                    m_MaterialEditor.RangeProperty(specularShift, Styles.specularShiftText);
                }
                m_MaterialEditor.RangeProperty(smoothFactor, Styles.smoothFactorText);
            }
        }
        mode = (RimLightMode)EditorGUILayout.Popup(Styles.rimLightModeText, (int)mode, Styles.rimLightNames);
        if (mode != RimLightMode.None || cutoff != null) {
			m_MaterialEditor.RangeProperty (rimIntensity, Styles.rimIntensityText);
			if (mode == RimLightMode.UserDefined)
				m_MaterialEditor.ColorProperty (rimColor, Styles.rimColorText);
		}

		if (EditorGUI.EndChangeCheck ()) {
			m_MaterialEditor.RegisterPropertyChangeUndo ("Rim Light Mode");
			rimLightMode.floatValue = (float)mode;
            if (useLighting != null)
            {
                if (lightUse)
                    useLighting.floatValue = 1.0f;
                else
                    useLighting.floatValue = 0.0f;

                if (lightDebug)
                    debugLighting.floatValue = 1.0f;
                else
                    debugLighting.floatValue = 0.0f;
            }
		}
			
		EditorGUI.showMixedValue = false;
	}

	void DoSkinArea (Material material)
	{
		EditorGUI.showMixedValue = skinMode.hasMixedValue;
		var mode = (SkinMode)skinMode.floatValue;
			
		EditorGUI.BeginChangeCheck ();
		mode = (SkinMode)EditorGUILayout.Popup (Styles.skinModeText, (int)mode, Styles.skinNames);
		if (mode != SkinMode.Disabled) {
			if (useSkinAlbedo != null) {
				if (skinMap.textureValue != null)
					m_MaterialEditor.TexturePropertySingleLine (Styles.albedoText, skinMap);
				else
					m_MaterialEditor.TexturePropertySingleLine (Styles.albedoText, skinMap, skinColor);
			} else {
				m_MaterialEditor.TexturePropertySingleLine (Styles.albedoText, skinMap, skinColor);
			}

			if (specularMap != null && specularColor != null) {
				if (specularMap.textureValue == null)
					m_MaterialEditor.TexturePropertyTwoLines (Styles.specularMapText, specularMap, specularColor, Styles.smoothnessText, smoothness);
				else
					m_MaterialEditor.TexturePropertySingleLine (Styles.specularMapText, specularMap);
					
			} else if (metallicMap != null && metallic != null) {
				if (metallicMap.textureValue == null)
					m_MaterialEditor.TexturePropertyTwoLines (Styles.metallicMapText, metallicMap, metallic, Styles.smoothnessText, smoothness);
				else
					m_MaterialEditor.TexturePropertySingleLine (Styles.metallicMapText, metallicMap);
			} else if (specular != null) {
				if (specularMap.textureValue == null)
					m_MaterialEditor.TexturePropertyTwoLines (Styles.specularMapText, specularMap, specular, Styles.glossinessText, smoothness);
				else
					m_MaterialEditor.TexturePropertySingleLine (Styles.specularMapText, specularMap);
			}

			if (useSkinAlbedo == null)
				m_MaterialEditor.TexturePropertySingleLine (Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
			else
				m_MaterialEditor.TexturePropertySingleLine (Styles.normalMapText, bumpMap);
		}

		if (EditorGUI.EndChangeCheck ()) {
			m_MaterialEditor.RegisterPropertyChangeUndo ("Skin Texture");
			skinMode.floatValue = (float)mode;
		}
			
		EditorGUI.showMixedValue = false;

	}

	void SetMaterialKeywords (Material material)
	{
		if (useFurAlbedo != null) {
			if (material.GetTexture("_MainTex"))
				useFurAlbedo.floatValue = 1.0f;
			else 
				useFurAlbedo.floatValue = 0.0f;
		}

		if (useSkinAlbedo != null) {
			if (material.GetTexture("_SkinTex"))
				useSkinAlbedo.floatValue = 1.0f;
			else 
				useSkinAlbedo.floatValue = 0.0f;
		}

		if ((furSpecularMap != null && furSpecularColor != null) || furSpecular != null) {
			if (material.GetTexture ("_FurSpecularMap"))
				useFurSecondMap.floatValue = 1.0f;
			else
				useFurSecondMap.floatValue = 0;
		} else if (furMetallicMap != null && furMetallic != null) {
			if (material.GetTexture ("_FurMetallicMap"))
				useFurSecondMap.floatValue = 1.0f;
			else
				useFurSecondMap.floatValue = 0;
		}

		if ((specularMap != null && specularColor != null) || specular != null) {
			if (material.GetTexture ("_SpecGlossMap"))
				useSkinSecondMap.floatValue = 1.0f;
			else
				useSkinSecondMap.floatValue = 0;
		} else if (metallicMap != null && metallic != null) {
			if (material.GetTexture ("_MetallicGlossMap"))
				useSkinSecondMap.floatValue = 1.0f;
			else
				useSkinSecondMap.floatValue = 0;
		}
	}

	void MaterialChanged (Material material)
	{
		SetMaterialKeywords (material);
	}

	void SetKeyword (Material m, string keyword, bool state)
	{
		if (state)
			m.EnableKeyword (keyword);
		else
			m.DisableKeyword (keyword);
	}
}
	
