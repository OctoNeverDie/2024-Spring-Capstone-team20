using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public NPCMeshManager Mesh;
    public NPCSpawner Spawner;

    public List<GameObject> NPCGroup = new List<GameObject>();
    public GameObject NPCHolder;

    private void Awake()
    {
        Init();
        NPCHolder = new GameObject("NPCHolder");
    }

    void Init()
    {
        Mesh = transform.AddComponent<NPCMeshManager>();
        Spawner = transform.AddComponent<NPCSpawner>();
    }

}
