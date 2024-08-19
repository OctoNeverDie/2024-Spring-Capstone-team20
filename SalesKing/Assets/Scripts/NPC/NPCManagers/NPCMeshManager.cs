using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshManager : MonoBehaviour
{

    // Resources 폴더 내의 기본 경로 설정
    private static readonly string basePath = "Meshes/NPC";

    // Mesh를 카테고리 Enum과 매핑할 Dictionary

    // 모든 걸 합친
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary = new Dictionary<NPCDefine.MeshType, List<Mesh>>();
    // 정상인것만 저장하는
    public Dictionary<NPCDefine.MeshType, List<Mesh>> NPCMeshDictionary_norm = new Dictionary<NPCDefine.MeshType, List<Mesh>>();

    // 모든 Mesh를 로드하고 Dictionary를 구성합니다.
    private void Awake()
    {
        LoadMeshes();
    }

    // Resources 폴더 내의 모든 Mesh를 로드하고 Dictionary에 저장합니다.
    private void LoadMeshes()
    {
        // 모든 카테고리 폴더를 검색합니다.
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
