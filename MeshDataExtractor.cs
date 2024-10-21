using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public class MeshDataExtractor : EditorWindow
{
    private string _meshFolderPath = "Assets/FolderPath";
    private StringBuilder _stringBuilder = new ();

    [MenuItem("MeshPolyCounter/Mesh Data Extractor")]
    private static void ShowWindow()
    {
        GetWindow<MeshDataExtractor>("Mesh Data Extractor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Mesh Data Extractor", EditorStyles.boldLabel);

        _meshFolderPath = EditorGUILayout.TextField("Folder Path:", _meshFolderPath);

        if (GUILayout.Button("Extract Mesh Data"))
        {
            ExtractMeshDataFromFolder();
        }
    }

    private void ExtractMeshDataFromFolder()
    {
        string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { _meshFolderPath });
        _stringBuilder.Append("Mesh name — triangles:\n");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

            if (!mesh) continue;
            int[] triangles = mesh.triangles;
                
            _stringBuilder.Append($"{mesh.name} — {triangles.Length / 3} triangles \n");
        }

        string savePath = Path.Combine("Assets", "MeshData.txt");
        
        File.WriteAllText(savePath, _stringBuilder.ToString());

        AssetDatabase.Refresh();
        
        _stringBuilder.Clear();
        
        Debug.Log($"Mesh Data Extraction completed and saved to {savePath}.");
    }
}
