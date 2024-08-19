using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshManager : MonoBehaviour
{

    // Resources ���� ���� �⺻ ��� ����
    private static readonly string basePath = "Meshes/NPC";

    // Mesh�� ī�װ� Enum�� ������ Dictionary

    // ��� �� ��ģ
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary = new Dictionary<NPCDefine.MeshType, List<Mesh>>();
    // �����ΰ͸� �����ϴ�
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary_norm = new Dictionary<NPCDefine.MeshType, List<Mesh>>();

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
