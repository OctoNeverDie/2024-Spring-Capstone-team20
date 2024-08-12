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

    // Resources 폴더 내의 기본 경로 설정
    private static readonly string basePath = "NPC/Meshes";

    // Mesh를 카테고리 Enum과 매핑할 Dictionary
    private Dictionary<MeshCategory, List<Mesh>> meshesByCategory = new Dictionary<MeshCategory, List<Mesh>>();

    // 모든 Mesh를 로드하고 Dictionary를 구성합니다.
    private void Start()
    {
        LoadMeshes();
    }

    // Resources 폴더 내의 모든 Mesh를 로드하고 Dictionary에 저장합니다.
    private void LoadMeshes()
    {
        // 모든 카테고리 폴더를 검색합니다.
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

    // 특정 카테고리의 모든 Mesh를 반환합니다.
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
