#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Asset to store project settings data.
/// </summary>
public class L2DGlobalSettings : ScriptableObject
{
    private static string l2DSettingsPath;
    private static string L2DSettingsPath
    {
        get
        {
            l2DSettingsPath = null;
            if (l2DSettingsPath == null)
            {
                List<string> paths = new List<string>();
                string[] result = AssetDatabase.FindAssets("L2DGlobalSettings");
                foreach (string guid in result)
                {
                    paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                }

                for (int i = 0; i < paths.Count; i++)
                {
                    if (!paths[i].Contains(".cs"))
                        continue;

                    l2DSettingsPath = paths[i];
                    l2DSettingsPath = l2DSettingsPath.TrimEnd(new char[] { '.','c','s'});
                    l2DSettingsPath = l2DSettingsPath + ".asset";
                }
            }

            return l2DSettingsPath;
        }
    }
    

    private static L2DGlobalSettings instance;
    public static L2DGlobalSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetOrCreateSettings();
            }
            return instance;
        }
    }

    [SerializeField]
    public LayerMask ShadowCastingLayers;

    [SerializeField]
    public LayerMask LightingLayers;

    [SerializeField]
    public LayerMask RequiresLightLayers;

    private static L2DGlobalSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<L2DGlobalSettings>(L2DSettingsPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<L2DGlobalSettings>();
            AssetDatabase.CreateAsset(settings, L2DSettingsPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    public static SerializedObject GetNewSerializedSettings()
    {
        return new SerializedObject(Instance);
    }

    
}
#endif