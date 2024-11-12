using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public NPCMeshManager Mesh;
    public NPCSpawner Spawner;
    public NPCAnimationManager Anim;
    public NPCMovementManager Move;

    public List<GameObject> NPCGroup = new List<GameObject>();
    public GameObject NPCHolder;
    public GameObject curTalkingNPC;

    protected override void Awake()
    {
        base.Awake();
        Init();
        NPCHolder = new GameObject("NPCHolder");
    }

    void Init()
    {
        Mesh = transform.AddComponent<NPCMeshManager>();
        Spawner = transform.AddComponent<NPCSpawner>();
        Anim = transform.AddComponent<NPCAnimationManager>();
        Move = transform.AddComponent<NPCMovementManager>();
    }

}
