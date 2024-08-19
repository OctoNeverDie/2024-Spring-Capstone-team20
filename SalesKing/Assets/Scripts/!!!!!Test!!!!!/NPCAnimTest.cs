using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimTest : MonoBehaviour
{
    NPCMove move;
    NPCLooks looks;

    void Start()
    {
        move = GetComponent<NPCMove>();
        looks = GetComponent<NPCLooks>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.SlightlyPositive);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][10];
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Positive);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][10];
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.VeryPositive);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][10];
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.SlightlyNegative);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][6];
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Negative);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][6];
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.VeryNegative);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][6];
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Attack);
        }
    }

}
