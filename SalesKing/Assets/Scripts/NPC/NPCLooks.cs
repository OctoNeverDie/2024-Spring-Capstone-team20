using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    //public Dictionary<NPCDefine.MeshType, Mesh> customedMesh = new Dictionary<NPCDefine.MeshType, Mesh>(); // ���Ӱ� ������ �޽�
    public Dictionary<NPCDefine.MeshType, GameObject> thisMesh = new Dictionary<NPCDefine.MeshType, GameObject>(); // ���Ӱ� ������ �޽�

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
            // Dictionary�� Ű�� ������ Add, ������ ���� ����
            if (customedMesh.ContainsKey(category))
            {
                // customedMesh[category] = Managers.NPC.Mesh.NPCMeshDictionary[category][0]; // Ű�� ���� �� ���� ����
                customedMesh[category] = null;
            }
            else
            {
                customedMesh.Add(category, null); // Ű�� ���� �� �߰�
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
