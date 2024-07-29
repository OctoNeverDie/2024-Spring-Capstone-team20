using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    NPCMeshManager _myMeshManager;

    void Start()
    {
        _myMeshManager = GameObject.Find("Spawner").transform.GetComponent<NPCMeshManager>();
        AssignRandomNPCLooks();
    }

    void AssignRandomNPCLooks()
    {
        //_myMeshManager
    }

}
