using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    public static NPCMeshManager Mesh;
    public static NPCSpawner Spawner;
    public static NPCAnimationManager Anim;
    public static NPCMovementManager Move;

    public List<GameObject> NPCGroup = new List<GameObject>();
    public NPC curTalkingNPC;

    public StoryNpcSO storyNpcSO;
    public NpcLookSO npcLookSO;

    public GameObject NPCPrefab;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        Mesh = transform.AddComponent<NPCMeshManager>();
        Spawner = transform.AddComponent<NPCSpawner>();
        Anim = transform.AddComponent<NPCAnimationManager>();
        Move = transform.AddComponent<NPCMovementManager>();
    }

}
