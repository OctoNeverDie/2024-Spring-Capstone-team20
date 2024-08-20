using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDefine.MoveState currentState;
    public NPCDefine.Talkable currentTalkable;
    public NPCDefine.LookState currentLook;

    public Transform destination;

    private GameObject myCanvas;
    private NPCMove npcMove;

    void Awake()
    {
        npcMove = transform.GetComponent<NPCMove>();
        myCanvas = transform.Find("Canvas").gameObject;
    }

    void Start()
    {
        AssignRandomLooks();
        AssignRandomState();
    }

    public void AssignRandomState()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            currentState = NPCDefine.MoveState.Stand;
            StartCoroutine(npcMove.StandForAwhile());
            npcMove.PlayRandomNPCAnim(NPCDefine.AnimType.Standing);
        }
        else
        {
            currentState = NPCDefine.MoveState.Walk;
            npcMove.ChooseNextDestination();
            npcMove.PlayRandomNPCAnim(NPCDefine.AnimType.Moving);
        }
    }

    void AssignRandomLooks()
    {
        NPCLooks looks = transform.GetComponent<NPCLooks>();

        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            looks.AssignCustomMesh(category, currentLook);
        }
    }

    public void SetTalkable()
    {
        GameObject GO = transform.Find("Canvas").gameObject;
        if(currentTalkable == NPCDefine.Talkable.Able)
        {
            GO.SetActive(true);
        }
    }



}
