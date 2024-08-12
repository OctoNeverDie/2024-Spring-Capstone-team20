using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    public Dictionary<NPCDefine.MeshType, int> customedMesh = new Dictionary<NPCDefine.MeshType, int>(); // ���Ӱ� ������ �޽�

    void Start()
    {
        SetCustomedMeshDefault();
    }

    private void SetCustomedMeshDefault()
    {
        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            // Dictionary�� Ű�� ������ Add, ������ ���� ����
            if (customedMesh.ContainsKey(category))
            {
                customedMesh[category] = 0; // Ű�� ���� �� ���� ����
            }
            else
            {
                customedMesh.Add(category, 0); // Ű�� ���� �� �߰�
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

                // �ش� ī�װ��� ���� ������Ʈ�� ���ؼ�
                if(category.ToString() == GO.name)
                {
                    MR.sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[category][customedMesh[category]];
                }
            }
        }
            
    }


}
