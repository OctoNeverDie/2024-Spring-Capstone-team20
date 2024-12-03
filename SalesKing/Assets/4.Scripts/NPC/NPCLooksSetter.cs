using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static MuhanNpcDataManager;
using static NPCDefine;

public class NPCLooksSetter : MonoBehaviour
{
    public bool isMuhan = false;

    public BackpackType BackpackType;
    public BodyType BodyType;
    public EyebrowType EyebrowType;
    public FullBodyType FullBodyType;
    public GlassesType GlassesType;
    public GloveType GloveType;
    public HairType HairType;
    public HatType HatType;
    public MustacheType MustacheType;
    public OuterwearType OuterwearType;
    public PantsType PantsType;
    public ShoeType ShoeType;


    public Dictionary<MeshType, GameObject> thisMesh = new Dictionary<NPCDefine.MeshType, GameObject>(); // 새롭게 적용할 메쉬

    void Start()
    {
        if (isMuhan) SetNPCBody();
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
    public void LoadMuhanMeshesFromSO(int muhanNpcID)
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
            //Debug.LogWarning($"No NpcLooks found for NpcID {muhanNpcID}");
        }

        AssignNPCMesh(MeshType.Backpack, npcLooks.BackpackType);
        AssignNPCMesh(MeshType.Eyebrow, npcLooks.EyebrowType);
        AssignNPCMesh(MeshType.FullBody, npcLooks.FullbodyType);
        AssignNPCMesh(MeshType.Glasses, npcLooks.GlassesType);
        AssignNPCMesh(MeshType.Glove, npcLooks.GloveType);
        AssignNPCMesh(MeshType.Hair, npcLooks.HairType);
        AssignNPCMesh(MeshType.Hat, npcLooks.HatType);
        AssignNPCMesh(MeshType.Mustache, npcLooks.MustacheType);
        AssignNPCMesh(MeshType.Outerwear, npcLooks.OuterwearType);
        AssignNPCMesh(MeshType.Pants, npcLooks.PantsType);
        AssignNPCMesh(MeshType.Shoe, npcLooks.ShoeType);
    }

    /// <summary>
    /// 데모보여주기용.
    /// mesh 정보가 MuhanNpcDataManager에서 만들어지는 즉시 입혀짐.
    /// </summary>
    /// <param name="muhanNpcID"></param>
    public void AssignMuhanMeshes(NpcLooks npcLooks)
    {
        AssignNPCMesh(MeshType.Backpack, npcLooks.BackpackType);
        AssignNPCMesh(MeshType.Eyebrow, npcLooks.EyebrowType);
        AssignNPCMesh(MeshType.FullBody, npcLooks.FullbodyType);
        AssignNPCMesh(MeshType.Glasses, npcLooks.GlassesType);
        AssignNPCMesh(MeshType.Glove, npcLooks.GloveType);
        AssignNPCMesh(MeshType.Hair, npcLooks.HairType);
        AssignNPCMesh(MeshType.Hat, npcLooks.HatType);
        AssignNPCMesh(MeshType.Mustache, npcLooks.MustacheType);
        AssignNPCMesh(MeshType.Outerwear, npcLooks.OuterwearType);
        AssignNPCMesh(MeshType.Pants, npcLooks.PantsType);
        AssignNPCMesh(MeshType.Shoe, npcLooks.ShoeType);

        //if(npcLooks.FullbodyType!=FullBodyType.None) 
    }

    public void AssignNPCMesh<TKey>(MeshType meshType, TKey key) where TKey : Enum
    {
        // Mesh 가져오기
        List<Mesh> meshes = NPCManager.Mesh.GetMeshes(meshType, key);
        if (meshes == null || meshes.Count == 0)
        {
            return;
        }

        // SkinnedMeshRenderer 설정
        SkinnedMeshRenderer meshRenderer = thisMesh[meshType].GetComponent<SkinnedMeshRenderer>();

        // 첫 번째 Mesh를 SkinnedMeshRenderer에 할당
        // 랜덤으로 Mesh 선택
        int randomIndex = UnityEngine.Random.Range(0, meshes.Count);
        meshRenderer.sharedMesh = meshes[randomIndex];
    }

    public void 


}
