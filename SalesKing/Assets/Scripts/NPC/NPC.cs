using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    
    public NPCDefine.State currentState;
    public NPCDefine.Talkable currentTalkable;
    public Transform destination;
    public float minStandTime = 10f;
    public float maxStandTime = 30f;

    GameObject myCanvas;

    private NPCMove npcMove;

    void Start()
    {
        npcMove = transform.GetComponent<NPCMove>();
        myCanvas = transform.Find("Canvas").gameObject;
        AssignRandomLooks();
        AssignRandomState();
    }

    #region NPC 동작 관련

    void AssignRandomState()
    {
        if (Random.Range(0, 2) == 0)
        {
            currentState = NPCDefine.State.Stand;
            StartCoroutine(StandCoroutine());
        }
        else
        {
            currentState = NPCDefine.State.Walk;
            AssignRandomDestination();
        }
    }

    IEnumerator StandCoroutine()
    {
        float standTime = Random.Range(minStandTime, maxStandTime);
        yield return new WaitForSeconds(standTime);
        AssignRandomState();
    }

    void AssignRandomDestination()
    {
        
        NPCSpawner spawner = FindObjectOfType<NPCSpawner>();
        destination = spawner.GetRandomSpawnPoint();
        npcMove.SetDestination(destination);
    }

    public void OnDestinationReached()
    {
        AssignRandomState();
    }

    #endregion

    void ShowCurTalkable()
    {
        myCanvas.SetActive(true);
    }

    void AssignRandomLooks()
    {
        NPCLooks looks = transform.GetComponent<NPCLooks>();

        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            int options = Managers.NPC.Mesh.NPCMeshDictionary[category].Count;
            looks.AssignCustomMesh(category, Random.Range(0, options));
        }
    }

}
