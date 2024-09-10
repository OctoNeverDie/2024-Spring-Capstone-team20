using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    public Dictionary<NPCDefine.MeshType, GameObject> thisMesh = new Dictionary<NPCDefine.MeshType, GameObject>(); // 새롭게 적용할 메쉬

    void Awake()
    {
        SetNPCBody();
    }

    void Start()
    {
        
    }

    private void SetNPCBody()
    {
        GameObject meshTransform = transform.Find("Mesh").gameObject;
        //Debug.Log(meshTransform.ToString());
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

    public void AssignCustomMesh(NPCDefine.MeshType type, NPCDefine.LookState look)
    {
        if (look == NPCDefine.LookState.Normal)
        {
            int options = Managers.NPC.Mesh.NPCMeshDictionary_norm[type].Count;
            int index = Random.Range(0, options);
            if (options <= 0) index = -1;

            // normal이라면... 가방, 장갑, 등등등... 없애기
            if(type == NPCDefine.MeshType.Backpack) index = -1;
            if(type == NPCDefine.MeshType.Glove) index = -1;
            if(type == NPCDefine.MeshType.Hat) index = -1;
            if(type == NPCDefine.MeshType.FullBody) index = -1;
            if(type == NPCDefine.MeshType.Mustache) index = -1;
            if(type == NPCDefine.MeshType.Glasses) index = -1;

            SkinnedMeshRenderer meshRenderer = thisMesh[type].GetComponent<SkinnedMeshRenderer>();
            if (index == -1)
            {
                meshRenderer.sharedMesh = null;
            }
            else
            {
                meshRenderer.sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary_norm[type][index];
            }
        }
        else
        {
            int options = Managers.NPC.Mesh.NPCMeshDictionary[type].Count;
            int index = Random.Range(0, options);

            SkinnedMeshRenderer meshRenderer = thisMesh[type].GetComponent<SkinnedMeshRenderer>();
            if (index == -1)
            {
                meshRenderer.sharedMesh = null;
            }
            else
            {
                meshRenderer.sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[type][index];
            }
        }
        
    }
}
