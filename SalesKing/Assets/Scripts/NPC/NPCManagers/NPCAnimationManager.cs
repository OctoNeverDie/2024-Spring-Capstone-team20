using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    private static readonly string basePath = "Animations/NPC";
    public Dictionary<NPCDefine.AnimType, List<Animation>> NPCMeshDictionary = new Dictionary<NPCDefine.AnimType, List<Animation>>();

    private void Awake()
    {
        LoadAnimations();
    }

    private void LoadAnimations()
    {
        /*
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
        */
    }

    /*
    public List<Mesh> GetMeshesByCategory(NPCDefine.MeshType category)
    {
        if (NPCMeshDictionary.TryGetValue(category, out List<Mesh> meshes))
        {
            return meshes;
        }
        else
        {
            Debug.LogWarning($"No meshes found for category '{category}'.");
            return new List<Mesh>();
        }
    }
    */
}
