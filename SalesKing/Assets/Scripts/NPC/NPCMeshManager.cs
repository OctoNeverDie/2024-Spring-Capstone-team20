using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshManager : MonoBehaviour
{

    // Resources ���� ���� �⺻ ��� ����
    private static readonly string basePath = "Meshes/NPC";

    // Mesh�� ī�װ� Enum�� ������ Dictionary
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary = new Dictionary<NPCDefine.MeshType, List<Mesh>>();

    // ��� Mesh�� �ε��ϰ� Dictionary�� �����մϴ�.
    private void Awake()
    {
        LoadMeshes();
    }

    // Resources ���� ���� ��� Mesh�� �ε��ϰ� Dictionary�� �����մϴ�.
    private void LoadMeshes()
    {
        // ��� ī�װ� ������ �˻��մϴ�.
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
    }

    // Ư�� ī�װ��� ��� Mesh�� ��ȯ�մϴ�.
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

}
