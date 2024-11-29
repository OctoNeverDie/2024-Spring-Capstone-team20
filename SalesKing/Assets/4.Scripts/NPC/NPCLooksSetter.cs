using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static MuhanNpcDataManager;

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

    /// <summary>
    /// 나중에 무한모드에서 day 로드하면 입혀짐.
    /// 즉, mesh 정보는 MuhanNpcDataManager에 전날 만들어지고,
    /// 다음날 npc가 story so의 id에 따라 mesh를 setting해야 할 때 MuhanNpcDataManager에서 자기 id를 매칭하여 mesh 조합 가져와서 입힘
    /// </summary>
    /// <param name="muhanNpcID"></param>
    public void AssignMuhanDayMeshes(int muhanNpcID)
    {
        NpcLooks npcLooks = MuhanNpcDataManager.Instance.npcs
            .Where(n => n.NpcID == muhanNpcID)
            .Select(n => n.NpcLooks)
            .SingleOrDefault();

        if (npcLooks != null)
        {
            Debug.Log($"NpcLooks for NpcID {muhanNpcID}: " +
                      $"BackpackType = {npcLooks.BackpackType}, " +
                      $"EyebrowType = {npcLooks.EyebrowType}, " +
                      $"FullbodyType = {npcLooks.FullbodyType}, " +
                      $"GlassesType = {npcLooks.GlassesType}, " +
                      $"GloveType = {npcLooks.GloveType}, " +
                      $"HairType = {npcLooks.HairType}, " +
                      $"HatType = {npcLooks.HatType}, " +
                      $"MustacheType = {npcLooks.MustacheType}, " +
                      $"OuterwearType = {npcLooks.OuterwearType}, " +
                      $"PantsType = {npcLooks.PantsType}, " +
                      $"ShoeType = {npcLooks.ShoeType}");
        }
        else
        {
            Debug.LogWarning($"No NpcLooks found for NpcID {muhanNpcID}");
        }

        AssignNPCMesh(NPCDefine.MeshType.Backpack, npcLooks.BackpackType);
        AssignNPCMesh(NPCDefine.MeshType.Eyebrow, npcLooks.EyebrowType);
        AssignNPCMesh(NPCDefine.MeshType.FullBody, npcLooks.FullbodyType);
        AssignNPCMesh(NPCDefine.MeshType.Glasses, npcLooks.GlassesType);
        AssignNPCMesh(NPCDefine.MeshType.Glove, npcLooks.GloveType);
        AssignNPCMesh(NPCDefine.MeshType.Hair, npcLooks.HairType);
        AssignNPCMesh(NPCDefine.MeshType.Hat, npcLooks.HatType);
        AssignNPCMesh(NPCDefine.MeshType.Mustache, npcLooks.MustacheType);
        AssignNPCMesh(NPCDefine.MeshType.Outerwear, npcLooks.OuterwearType);
        AssignNPCMesh(NPCDefine.MeshType.Pants, npcLooks.PantsType);
        AssignNPCMesh(NPCDefine.MeshType.Shoe, npcLooks.ShoeType);
    }

    /// <summary>
    /// 데모보여주기용.
    /// mesh 정보가 MuhanNpcDataManager에서 만들어지는 즉시 입혀짐.
    /// </summary>
    /// <param name="muhanNpcID"></param>
    public void AssignMuhanMeshes(NpcLooks npcLooks)
    {
        AssignNPCMesh(NPCDefine.MeshType.Backpack, npcLooks.BackpackType);
        AssignNPCMesh(NPCDefine.MeshType.Eyebrow, npcLooks.EyebrowType);
        AssignNPCMesh(NPCDefine.MeshType.FullBody, npcLooks.FullbodyType);
        AssignNPCMesh(NPCDefine.MeshType.Glasses, npcLooks.GlassesType);
        AssignNPCMesh(NPCDefine.MeshType.Glove, npcLooks.GloveType);
        AssignNPCMesh(NPCDefine.MeshType.Hair, npcLooks.HairType);
        AssignNPCMesh(NPCDefine.MeshType.Hat, npcLooks.HatType);
        AssignNPCMesh(NPCDefine.MeshType.Mustache, npcLooks.MustacheType);
        AssignNPCMesh(NPCDefine.MeshType.Outerwear, npcLooks.OuterwearType);
        AssignNPCMesh(NPCDefine.MeshType.Pants, npcLooks.PantsType);
        AssignNPCMesh(NPCDefine.MeshType.Shoe, npcLooks.ShoeType);
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
