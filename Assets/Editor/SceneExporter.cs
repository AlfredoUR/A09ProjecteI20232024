using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

public class SceneExporter : MonoBehaviour
{
    [MenuItem("Tools/Export Scene Info")]
    public static void ExportSceneInfo()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        string path = "Assets/scene_dump_detailed.txt";
        StreamWriter writer = new StreamWriter(path, false);

        foreach (GameObject obj in allObjects)
        {
            writer.WriteLine($"GameObject: {obj.name} (active: {obj.activeSelf})");
            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components)
            {
                if (comp == null) continue;
                writer.WriteLine($"  - Component: {comp.GetType().Name}");

                FieldInfo[] fields = comp.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo field in fields)
                {
                    bool isPublic = field.IsPublic;
                    bool isSerialized = field.GetCustomAttribute<SerializeField>() != null;

                    if (isPublic || isSerialized)
                    {
                        object value = field.GetValue(comp);
                        string valueStr = value != null ? value.ToString() : "null";
                        writer.WriteLine($"      {field.Name}: {valueStr}");
                    }
                }
            }
            writer.WriteLine();
        }

        writer.Close();
        Debug.Log($"Detalls de l’escena exportats a: {path}");
    }
}
