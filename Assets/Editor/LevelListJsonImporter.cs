using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelListJsonImporter : EditorWindow
{
    private TextAsset jsonFile;
    private LevelListSO targetSO;

    [MenuItem("Tools/Import LevelList from JSON")]
    public static void ShowWindow()
    {
        GetWindow<LevelListJsonImporter>("Import Level JSON");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level JSON Importer", EditorStyles.boldLabel);

        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);
        targetSO = (LevelListSO)EditorGUILayout.ObjectField("Target LevelListSO", targetSO, typeof(LevelListSO), false);

        if (GUILayout.Button("Import"))
        {
            if (jsonFile == null || targetSO == null)
            {
                Debug.LogError("❌ JSON dosyası veya hedef SO atanmadı.");
                return;
            }

            try
            {
                var levels = JsonHelper.FromJson<LevelData>(jsonFile.text);
                targetSO.levels = levels;

                EditorUtility.SetDirty(targetSO);
                AssetDatabase.SaveAssets();

                Debug.Log($"✅ {levels.Count} level başarıyla yüklendi.");
            }
            catch
            {
                Debug.LogError("❌ JSON formatı geçersiz.");
            }
        }
    }
}