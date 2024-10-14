using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

namespace L2D
{
    static class L2DSettingProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/L2D", SettingsScope.Project)
            {
                label = "L2D",
                guiHandler = (searchContext) =>
                {
                    var settings = L2DGlobalSettings.GetNewSerializedSettings();
                    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField("Lighting Layers should be all the layers of objects that are a light source.", EditorStyles.wordWrappedLabel);
                        EditorGUILayout.PropertyField(settings.FindProperty("LightingLayers"), new GUIContent("Lighting Layers"));
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField("Shadow Casting Layers should be all the layers of objects that should cast a shadow when light hits it.", EditorStyles.wordWrappedLabel);
                        EditorGUILayout.PropertyField(settings.FindProperty("ShadowCastingLayers"), new GUIContent("Shadow Casting Layers"));
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField("Requires Light Layers should be all the layers of objects that are unable to be seen without any light. Effect applied in play mode only.", EditorStyles.wordWrappedLabel);
                        EditorGUILayout.PropertyField(settings.FindProperty("RequiresLightLayers"), new GUIContent("Requires Light Layers"));
                    }

                    if (settings.hasModifiedProperties)
                    {
                        settings.ApplyModifiedProperties();
                        UpdateRendererLayers();
                    }
                },
                
                keywords = new HashSet<string>(new[] { "Light", "Lighting", "Shadow", "L2D", "2D", "Lighting2D" })
            };

            return provider;
        }

        public static void UpdateRendererLayers()
        {

            for (int i = 8; i < 32; i++)
            {
                if (((L2DGlobalSettings.Instance.LightingLayers >> i) & 1) > 0)
                    CreateLayer("Lights", i);
            }
            for (int i = 8; i < 32; i++)
            {
                if (((L2DGlobalSettings.Instance.ShadowCastingLayers >> i) & 1) > 0)
                    CreateLayer("ShadowCasters", i);
            }
            for (int i = 8; i < 32; i++)
            {
                if (((L2DGlobalSettings.Instance.RequiresLightLayers >> i) & 1) > 0)
                    CreateLayer("HiddenObjects", i);
            }


            L2DAssets.Instance.ForwardRendererInstance.opaqueLayerMask = -1 ^ L2DGlobalSettings.Instance.LightingLayers ^ L2DGlobalSettings.Instance.RequiresLightLayers;
            L2DAssets.Instance.ForwardRendererInstance.transparentLayerMask = -1 ^ L2DGlobalSettings.Instance.LightingLayers ^ L2DGlobalSettings.Instance.RequiresLightLayers;
            ((RenderObjects)L2DAssets.Instance.ForwardRendererInstance.rendererFeatures[0]).settings.filterSettings.LayerMask = L2DGlobalSettings.Instance.LightingLayers;
            ((RenderObjects)L2DAssets.Instance.ForwardRendererInstance.rendererFeatures[1]).settings.filterSettings.LayerMask = L2DGlobalSettings.Instance.RequiresLightLayers;
            
            L2DAssets.Instance.LightingRenderer.opaqueLayerMask = 0;
            L2DAssets.Instance.LightingRenderer.transparentLayerMask = 0;
            ((RenderObjects)L2DAssets.Instance.LightingRenderer.rendererFeatures[0]).settings.filterSettings.LayerMask = L2DGlobalSettings.Instance.LightingLayers;
            

            if (GraphicsSettings.renderPipelineAsset != L2DAssets.Instance.L2DRenderPipelineAsset)
            {
                GraphicsSettings.renderPipelineAsset = L2DAssets.Instance.L2DRenderPipelineAsset;
            }
        }


        public static bool CreateLayer(string layerName, int position)
        {
            if (position < 0 || position > 31) return false;

            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            if (!PropertyExists(layersProp, 0, 31, layerName))
            {
                SerializedProperty sp;
                sp = layersProp.GetArrayElementAtIndex(position);

                if (sp.stringValue == "")
                {
                    sp.stringValue = layerName;
                    tagManager.ApplyModifiedProperties();
                    return true;
                }
            }
            return false;
        }

        public static bool CreateLayer(string layerName)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            if (!PropertyExists(layersProp, 0, 31, layerName))
            {
                SerializedProperty sp;
                
                for (int i = 8, j = 31; i < j; i++)
                {
                    sp = layersProp.GetArrayElementAtIndex(i);
                    if (sp.stringValue == "")
                    {
                        sp.stringValue = layerName;
                        tagManager.ApplyModifiedProperties();
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool LayerExists(string layerName)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            return PropertyExists(layersProp, 0, 31, layerName);
        }
        
        private static bool PropertyExists(SerializedProperty property, int start, int end, string value)
        {
            for (int i = start; i < end; i++)
            {
                SerializedProperty t = property.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}