using System;
using System.Collections.Generic;
using UnityEngine;
using static NPCDefine;

public class NPCMeshManager : MonoBehaviour
{
    private static readonly string basePath = "Meshes/NPC_Parts/";

    public Dictionary<NPCDefine.MeshType, Dictionary<Enum, List<Mesh>>> NPCMeshDictionary;

    void Awake()
    {
        NPCMeshDictionary = new Dictionary<NPCDefine.MeshType, Dictionary<Enum, List<Mesh>>>();
        LoadMeshes();
    }

    private void LoadMeshes()
    {
        foreach (NPCDefine.MeshType category in Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            // 각 Category마다 Key에 해당하는 Mesh를 로드
            switch (category)
            {
                case NPCDefine.MeshType.Backpack:
                    AddCategoryMeshes<BackpackType>(category, basePath + "Backpack/");
                    break;

                case NPCDefine.MeshType.Body:
                    AddCategoryMeshes<BodyType>(category, basePath + "Body/");
                    break;

                case NPCDefine.MeshType.Eyebrow:
                    AddCategoryMeshes<EyebrowType>(category, basePath + "Eyebrow/");
                    break;

                case NPCDefine.MeshType.FullBody:
                    AddCategoryMeshes<FullBodyType>(category, basePath + "FullBody/");
                    break;

                case NPCDefine.MeshType.Glasses:
                    AddCategoryMeshes<GlassesType>(category, basePath + "Glasses/");
                    break;

                case NPCDefine.MeshType.Glove:
                    AddCategoryMeshes<GloveType>(category, basePath + "Glove/");
                    break;

                case NPCDefine.MeshType.Hair:
                    AddCategoryMeshes<HairType>(category, basePath + "Hair/");
                    break;

                case NPCDefine.MeshType.Hat:
                    AddCategoryMeshes<HatType>(category, basePath + "Hat/");
                    break;

                case NPCDefine.MeshType.Mustache:
                    AddCategoryMeshes<MustacheType>(category, basePath + "Mustache/");
                    break;

                case NPCDefine.MeshType.Outerwear:
                    AddCategoryMeshes<OuterwearType>(category, basePath + "Outerwear/");
                    break;

                case NPCDefine.MeshType.Pants:
                    AddCategoryMeshes<PantsType>(category, basePath + "Pants/");
                    break;

                case NPCDefine.MeshType.Shoe:
                    AddCategoryMeshes<ShoeType>(category, basePath + "Shoe/");
                    break;

                default:
                    Debug.LogWarning($"Unknown category: {category}");
                    break;
            }
        }

    }

    private void AddCategoryMeshes<TKey>(NPCDefine.MeshType category, string categoryPath) where TKey : Enum
    {
        if (!NPCMeshDictionary.ContainsKey(category))
        {
            NPCMeshDictionary[category] = new Dictionary<Enum, List<Mesh>>();
        }

        foreach (TKey key in Enum.GetValues(typeof(TKey)))
        {
            // "None" 값은 건너뜀
            if (key.ToString() == "None")
            {
                //Debug.Log($"Skipping 'None' for Category: {category}");
                continue;
            }

            string path = $"{categoryPath}{key.ToString()}";
            //Debug.Log($"Loading meshes for: {category} - Key: {key}, Path: {path}");
            Mesh[] meshes = Resources.LoadAll<Mesh>(path);
            if (meshes.Length > 0)
            {
                NPCMeshDictionary[category][key] = new List<Mesh>(meshes);
                //Debug.Log($"Loaded {meshes.Length} meshes from {path}");
            }
            else
            {
                //Debug.LogWarning($"No meshes found at path: {path}");
            }
        }

    }


    public List<Mesh> GetMeshes(NPCDefine.MeshType category, Enum key)
    {
        if (NPCMeshDictionary.ContainsKey(category) && NPCMeshDictionary[category].ContainsKey(key))
        {
            return NPCMeshDictionary[category][key];
        }

        //Debug.LogWarning($"Meshes not found for Category: {category}, Key: {key}");
        return null;
    }
    
}
