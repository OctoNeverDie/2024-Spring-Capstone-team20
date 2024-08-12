using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    public Dictionary<NPCDefine.MeshType, int> customedMesh = new Dictionary<NPCDefine.MeshType, int>(); // 새롭게 적용할 메쉬

    void Start()
    {
        SetCustomedMeshDefault();
    }

    private void SetCustomedMeshDefault()
    {
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            // Dictionary에 키가 없으면 Add, 있으면 값을 설정
            if (customedMesh.ContainsKey(category))
            {
                customedMesh[category] = 0; // 키가 있을 때 값을 설정
            }
            else
            {
                customedMesh.Add(category, 0); // 키가 없을 때 추가
            }
        }
    }

    public void AssignCustomMesh(NPCDefine.MeshType type, int index)
    {
        customedMesh[type] = index;
        //ApplyCustomedMesh();
    }

    public void ApplyCustomedMesh()
    {
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject GO = transform.GetChild(i).gameObject;
                SkinnedMeshRenderer MR = GO.GetComponent<SkinnedMeshRenderer>();

                // 해당 카테고리의 게임 오브젝트에 대해서
                if(category.ToString() == GO.name)
                {
                    MR.sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[category][customedMesh[category]];
                }
            }
        }
            
    }


}
