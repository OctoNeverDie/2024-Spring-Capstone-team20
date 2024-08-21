using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshManager : MonoBehaviour
{
    private static readonly string basePath = "Meshes/NPC";

    // 모든 걸 합친
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary = new Dictionary<NPCDefine.MeshType, List<Mesh>>();
    // 정상인것만 저장하는
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary_norm = new Dictionary<NPCDefine.MeshType, List<Mesh>>();

    private void Awake()
    {
        LoadMeshes();
    }

    private void LoadMeshes()
    {
        // set every-mesh dictionary
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            string folderPath = $"{basePath}/{category.ToString()}";
            Mesh[] meshes = Resources.LoadAll<Mesh>(folderPath);

            if (meshes.Length > 0)
            {
                if (!NPCMeshDictionary.ContainsKey(category))
                {
                    NPCMeshDictionary[category] = new List<Mesh>();
                }

                NPCMeshDictionary[category].AddRange(meshes);
                Debug.Log($"Loaded {meshes.Length} meshes for category '{category}'.");
            }
        }

        // set normal-mesh dictionary
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            string folderPath = $"{basePath}/{category.ToString()}/Normal";
            Mesh[] meshes = Resources.LoadAll<Mesh>(folderPath);

            if (meshes.Length > 0)
            {
                if (!NPCMeshDictionary_norm.ContainsKey(category))
                {
                    NPCMeshDictionary_norm[category] = new List<Mesh>();
                }

                NPCMeshDictionary_norm[category].AddRange(meshes);
                Debug.Log($"Loaded {meshes.Length} Normal meshes for category '{category}'.");
            }
            else
            {
                if (!NPCMeshDictionary_norm.ContainsKey(category))
                {
                    NPCMeshDictionary_norm[category] = new List<Mesh>();
                }
                Debug.Log($"Loaded zero Normal meshes for category '{category}'.");
            }
        }
    }

}
