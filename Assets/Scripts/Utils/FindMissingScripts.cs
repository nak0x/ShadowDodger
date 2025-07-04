using UnityEngine;
using UnityEditor;

// This file is generated by ChatGPT

public class FindMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Scripts in Scene")]
    public static void FindMissingInCurrentScene()
    {
        GameObject[] go = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int count = 0;
        foreach (GameObject g in go)
        {
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning($"Missing script in GameObject: {g.name}", g);
                    count++;
                }
            }
        }
        Debug.Log($"Total missing scripts: {count}");
    }
}
