using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    /*
    public Dictionary<NPCDefine.MeshType, GameObject> thisMesh = new Dictionary<NPCDefine.MeshType, GameObject>(); // 새롭게 적용할 메쉬

    void Awake()
    {
        SetNPCBody();
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

    public void AssignCustomMesh(NPCDefine.MeshType type)
    {
        
        int options = NPC.Mesh.NPCMeshDictionary[type].Count;
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
*/
}
