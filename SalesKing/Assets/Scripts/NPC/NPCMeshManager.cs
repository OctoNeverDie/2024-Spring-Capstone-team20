using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshManager : MonoBehaviour
{
    public enum MeshCategory
    {
        Backpack,
        Body, 
        Eyebrow,
        Fullbody,
        Glasses,
        Glove, 
        Hair, 
        Hat, 
        Mustache, 
        Outerwear, 
        Pants, 
        Shoe
    }

    // Resources ���� ���� �⺻ ��� ����
    private static readonly string basePath = "NPC/Meshes";

    // Mesh�� ī�װ� Enum�� ������ Dictionary
    private Dictionary<MeshCategory, List<Mesh>> meshesByCategory = new Dictionary<MeshCategory, List<Mesh>>();

    // ��� Mesh�� �ε��ϰ� Dictionary�� �����մϴ�.
    private void Start()
    {
        LoadMeshes();
    }

    // Resources ���� ���� ��� Mesh�� �ε��ϰ� Dictionary�� �����մϴ�.
    private void LoadMeshes()
    {
        // ��� ī�װ� ������ �˻��մϴ�.
        foreach (MeshCategory category in System.Enum.GetValues(typeof(MeshCategory)))
        {
            string folderPath = $"{basePath}/{category.ToString()}";
            Mesh[] meshes = Resources.LoadAll<Mesh>(folderPath);

            if (meshes.Length > 0)
            {
                if (!meshesByCategory.ContainsKey(category))
                {
                    meshesByCategory[category] = new List<Mesh>();
                }

                meshesByCategory[category].AddRange(meshes);
                Debug.Log($"Loaded {meshes.Length} meshes for category '{category}'.");
            }
        }
    }

    // Ư�� ī�װ��� ��� Mesh�� ��ȯ�մϴ�.
    public List<Mesh> GetMeshesByCategory(MeshCategory category)
    {
        if (meshesByCategory.TryGetValue(category, out List<Mesh> meshes))
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
