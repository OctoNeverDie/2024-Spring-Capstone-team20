using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    
    public NPCDefine.State currentState;
    public NPCDefine.Talkable currentTalkable;

    public Transform destination;

    private GameObject myCanvas;
    private NPCMove npcMove;
    private Animator animator;

    void Start()
    {
        npcMove = transform.GetComponent<NPCMove>();
        myCanvas = transform.Find("Canvas").gameObject;
        animator = GetComponent<Animator>();
        AssignRandomLooks();
        AssignRandomState();
    }

    public void AssignRandomState()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            currentState = NPCDefine.State.Stand;
            StartCoroutine(npcMove.StandForAwhile());

            int randAnimIndex = Random.Range(0, Managers.NPC.Anim.NPCAnimDictionary[NPCDefine.AnimType.Standing].Count);
            animator.Play(Managers.NPC.Anim.NPCAnimDictionary[NPCDefine.AnimType.Standing][randAnimIndex].name);
        }
        else
        {
            currentState = NPCDefine.State.Walk;
            npcMove.ChooseNextDestination();

            int randAnimIndex = Random.Range(0, Managers.NPC.Anim.NPCAnimDictionary[NPCDefine.AnimType.Moving].Count);
            animator.Play(Managers.NPC.Anim.NPCAnimDictionary[NPCDefine.AnimType.Moving][randAnimIndex].name);
        }
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

    public void SetTalkable()
    {
        GameObject GO = transform.Find("Canvas").gameObject;
        if(currentTalkable == NPCDefine.Talkable.Able)
        {
            GO.SetActive(true);
        }
    }

}
