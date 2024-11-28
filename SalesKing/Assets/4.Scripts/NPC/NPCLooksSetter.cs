using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static NPCDefine;

public class NPCLooksSetter : MonoBehaviour
{
    public NPCDefine.BackpackType BackpackType;
    public NPCDefine.BodyType BodyType;
    public NPCDefine.EyebrowType EyebrowType;
    public NPCDefine.FullBodyType FullBodyType;
    public NPCDefine.GlassesType GlassesType;
    public NPCDefine.GloveType GloveType;
    public NPCDefine.HairType HairType;
    public NPCDefine.HatType HatType;
    public NPCDefine.MustacheType MustacheType;
    public NPCDefine.OuterwearType OuterwearType;
    public NPCDefine.PantsType PantsType;
    public NPCDefine.ShoeType ShoeType;


    public Dictionary<NPCDefine.MeshType, GameObject> thisMesh = new Dictionary<NPCDefine.MeshType, GameObject>(); // 새롭게 적용할 메쉬

    void Awake()
    {
        SetNPCBody();
    }

    private void SetNPCBody()
    {
        
        GameObject meshTransform = transform.Find("Mesh").gameObject;

        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            for (int i = 0; i < meshTransform.transform.childCount; i++)
            {
                GameObject GO = meshTransform.transform.GetChild(i).gameObject;
                if (category.ToString() == GO.name)
                {
                    thisMesh.Add(category, GO);
                }
            }
        }
        
    }

    public void AssignAllMeshes()
    {
        AssignNPCMesh(NPCDefine.MeshType.Backpack, BackpackType);
        AssignNPCMesh(NPCDefine.MeshType.Body, BodyType);
        AssignNPCMesh(NPCDefine.MeshType.Eyebrow, EyebrowType);
        AssignNPCMesh(NPCDefine.MeshType.FullBody, FullBodyType);
        AssignNPCMesh(NPCDefine.MeshType.Glasses, GlassesType);
        AssignNPCMesh(NPCDefine.MeshType.Glove, GloveType);
        AssignNPCMesh(NPCDefine.MeshType.Hair, HairType);
        AssignNPCMesh(NPCDefine.MeshType.Hat, HatType);
        AssignNPCMesh(NPCDefine.MeshType.Mustache, MustacheType);
        AssignNPCMesh(NPCDefine.MeshType.Outerwear, OuterwearType);
        AssignNPCMesh(NPCDefine.MeshType.Pants, PantsType);
        AssignNPCMesh(NPCDefine.MeshType.Shoe, ShoeType);
    }

    public void AssignNPCMesh<TKey>(NPCDefine.MeshType meshType, TKey key) where TKey : Enum
    {
        // Mesh 가져오기
        List<Mesh> meshes = NPCManager.Mesh.GetMeshes(meshType, key);
        if (meshes == null || meshes.Count == 0)
        {
            Debug.LogWarning($"No meshes found for MeshType: {meshType}, Key: {key}");
            return;
        }

        // SkinnedMeshRenderer 설정
        SkinnedMeshRenderer meshRenderer = thisMesh[meshType].GetComponent<SkinnedMeshRenderer>();

        // 첫 번째 Mesh를 SkinnedMeshRenderer에 할당
        meshRenderer.sharedMesh = meshes[0]; // 필요하면 임의로 선택 가능
        Debug.Log($"Mesh assigned for MeshType: {meshType}, Key: {key}");
    }


}
