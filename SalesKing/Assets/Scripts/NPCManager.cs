using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public NPCMeshManager Mesh;
    public NPCSpawner Spawner;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        Mesh = transform.AddComponent<NPCMeshManager>();
        Spawner = transform.AddComponent<NPCSpawner>();
    }

}
