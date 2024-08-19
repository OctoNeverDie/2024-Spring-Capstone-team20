using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimTest : MonoBehaviour
{
    NPCMove move;
    NPCLooks looks;
    private Rigidbody rb;

    Vector3 init_position;
    Quaternion init_rotation;

    void Start()
    {
        move = GetComponent<NPCMove>();
        looks = GetComponent<NPCLooks>();
        rb = GetComponent<Rigidbody>();

        init_position = transform.position;
        init_rotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.SlightlyPositive);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][10];
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Positive);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][10];
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.VeryPositive);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][10];
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.SlightlyNegative);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][6];
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Negative);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][6];
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.VeryNegative);
            looks.thisMesh[NPCDefine.MeshType.Eyebrow].GetComponent<SkinnedMeshRenderer>().sharedMesh = Managers.NPC.Mesh.NPCMeshDictionary[NPCDefine.MeshType.Eyebrow][6];
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Attack);
        }
    }

}
