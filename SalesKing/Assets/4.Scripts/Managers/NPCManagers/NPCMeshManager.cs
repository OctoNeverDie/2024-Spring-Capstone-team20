using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCDefine;
using static TreeEditor.TreeEditorHelper;

public class NPCMeshManager : MonoBehaviour
{
    //private static readonly string basePath = "Meshes/NPC_Parts/";

    //public Dictionary<NPCDefine.MeshType, Dictionary<NPCDefine.MeshType, List<Mesh>>> NPCMeshDictionary = new Dictionary<NPCDefine.MeshType, List<Mesh>>();


    public Dictionary<MeshType, Dictionary<Enum, List<Mesh>>> NPCMeshDictionary;

    void Awake()
    {
        NPCMeshDictionary = new Dictionary<MeshType, Dictionary<Enum, List<Mesh>>>();
        LoadMeshes();
    }


    private void LoadMeshes()
    {
        /*
        foreach (MeshType category in Enum.GetValues(typeof(MeshType)))
        {
            // 각 Category마다 Key에 해당하는 Mesh를 로드
            switch (category)
            {
                case MeshType.Backpack:
                    AddCategoryMeshes<MeshType, BackpackType>(basePath + "Backpack/");
                    break;

                case MeshType.Body:
                    AddCategoryMeshes<MeshType, BodyType>(basePath + "Body/");
                    break;

                case MeshType.Eyebrow:
                    AddCategoryMeshes<MeshType, EyebrowType>(basePath + "Eyebrow/");
                    break;

                case MeshType.FullBody:
                    AddCategoryMeshes<MeshType, FullBodyType>(basePath + "FullBody/");
                    break;

                case MeshType.Glasses:
                    AddCategoryMeshes<MeshType, GlassesType>(basePath + "Glasses/");
                    break;

                case MeshType.Glove:
                    AddCategoryMeshes<MeshType, GloveType>(basePath + "Glove/");
                    break;

                case MeshType.Hair:
                    AddCategoryMeshes<MeshType, HairType>(basePath + "Hair/");
                    break;

                case MeshType.Hat:
                    AddCategoryMeshes<MeshType, HatType>(basePath + "Hat/");
                    break;

                case MeshType.Mustache:
                    AddCategoryMeshes<MeshType, MustacheType>(basePath + "Mustache/");
                    break;

                case MeshType.Outerwear:
                    AddCategoryMeshes<MeshType, OuterwearType>(basePath + "Outerwear/");
                    break;

                case MeshType.Pants:
                    AddCategoryMeshes<MeshType, PantsType>(basePath + "Pants/");
                    break;

                case MeshType.Shoe:
                    AddCategoryMeshes<MeshType, ShoeType>(basePath + "Shoe/");
                    break;

                default:
                    Debug.LogWarning($"Unknown category: {category}");
                    break;
            }
        }
        */

    }

    /*
    private void AddCategoryMeshes<TCategory, TKey>(string categoryPath) where TKey : Enum
    {
        if (!NPCMeshDictionary.ContainsKey((MeshType)Enum.Parse(typeof(MeshType), typeof(TCategory).Name)))
        {
            NPCMeshDictionary[(MeshType)Enum.Parse(typeof(MeshType), typeof(TCategory).Name)] =
                new Dictionary<Enum, List<Mesh>>();
        }

        foreach (TKey key in Enum.GetValues(typeof(TKey)))
        {
            // Resources에서 Mesh 로드
            Mesh[] meshes = Resources.LoadAll<Mesh>($"{categoryPath}/{key.ToString().ToLower()}");

            if (meshes.Length > 0)
            {
                NPCMeshDictionary[(MeshType)Enum.Parse(typeof(MeshType), typeof(TCategory).Name)][key] =
                    new List<Mesh>(meshes);
            }
        }
    }

    public List<Mesh> GetMeshes(MeshType category, Enum key)
    {
        if (NPCMeshDictionary.ContainsKey(category) && NPCMeshDictionary[category].ContainsKey(key))
        {
            return NPCMeshDictionary[category][key];
        }

        Debug.LogWarning($"Meshes not found for Category: {category}, Key: {key}");
        return null;
    }
    */
}
