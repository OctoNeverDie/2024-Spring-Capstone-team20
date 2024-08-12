using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    //public Dictionary<NPCDefine.MeshType, Mesh> customedMesh = new Dictionary<NPCDefine.MeshType, Mesh>(); // 새롭게 적용할 메쉬
    public Dictionary<NPCDefine.MeshType, GameObject> thisMesh = new Dictionary<NPCDefine.MeshType, GameObject>(); // 새롭게 적용할 메쉬

    void Awake()
    {
        //SetCustomedMeshDefault();
        SetNPCBody();
    }

    /*
    private void SetCustomedMeshDefault()
    {
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            // Dictionary에 키가 없으면 Add, 있으면 값을 설정
            if (customedMesh.ContainsKey(category))
            {
                // customedMesh[category] = Managers.NPC.Mesh.NPCMeshDictionary[category][0]; // 키가 있을 때 값을 설정
                customedMesh[category] = null;
            }
            else
            {
                customedMesh.Add(category, null); // 키가 없을 때 추가
            }
        }
    }
    */

    private void SetNPCBody()
    {
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject GO = transform.GetChild(i).gameObject;
                if (category.ToString() == GO.name)
                {
                    thisMesh.Add(category, GO);
                }
            }
        }
    }

    public void AssignCustomMesh(NPCDefine.MeshType type, int index)
    {
        SkinnedMeshRenderer meshRenderer = thisMesh[type].GetComponent<SkinnedMeshRenderer>();
        if(index == -1)
        {
            meshRenderer.sharedMesh = null;
        }
        else
        {
            meshRenderer.sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[type][index];
        }
    }
}
