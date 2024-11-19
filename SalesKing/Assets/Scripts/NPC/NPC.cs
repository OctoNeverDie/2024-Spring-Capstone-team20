using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NPCDefine.MoveState currentState;
    public NPCDefine.Talkable currentTalkable;
    public NPCDefine.LookState currentLook;

    public Transform destination;

    public GameObject myCanvas;

    private Animator animator;
    private NavMeshAgent agent;

    public float speed = 0.5f;
    public float rotationSpeed = 5.0f; // 서서히 회전하기 위한 속도

    public Transform curDestination;

    public float minStandTime = 10f;
    public float maxStandTime = 30f;

    public int NpcID = 0;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myCanvas = transform.Find("Canvas").gameObject;
    }

    void Start()
    {
        agent.speed = speed;
        AssignRandomLooks();
        AssignRandomState();
    }

    void Update()
    {
        /*
        if (currentState != NPCDefine.MoveState.Talk && curDestination != null)
        {
            agent.SetDestination(curDestination.position);

            if (Vector3.Distance(transform.position, curDestination.position) < 1f)
            {
                curDestination = null;
                AssignRandomState();
            }
        }
        */
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentTalkable == NPCDefine.Talkable.Able)
        {
            NPCEnterConvo(other.gameObject);
        }
    }
    */

    public void AssignRandomState()
    {
        /*
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            currentState = NPCDefine.MoveState.Stand;
            StartCoroutine(StandForAwhile());
            PlayRandomNPCAnim(NPCDefine.AnimType.Standing);
        }
        else
        {
            currentState = NPCDefine.MoveState.Walk;
            ChooseNextDestination();
            PlayRandomNPCAnim(NPCDefine.AnimType.Moving);
        }
        */
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

    public void PlayNPCAnimByEmotion(Define.Emotion emotion)
    {
        switch (emotion)
        {
            case Define.Emotion.best:
                animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[NPCDefine.AnimType.VeryPositive][0].name);
                break;
            case Define.Emotion.good:
                animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[NPCDefine.AnimType.Positive][0].name);
                break;
            case Define.Emotion.normal:
                animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[NPCDefine.AnimType.SlightlyPositive][0].name);
                break;
            case Define.Emotion.bad:
                animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[NPCDefine.AnimType.Negative][0].name);
                break;
            case Define.Emotion.worst:
                animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[NPCDefine.AnimType.VeryNegative][0].name);
                break;
            default: break;
        }
        
    }

    public void PlayRandomNPCAnim(NPCDefine.AnimType type)
    {
        int randAnimIndex = Random.Range(0, NPCManager.Instance.Anim.NPCAnimDictionary[type].Count);
        animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[type][randAnimIndex].name);
    }

    public void ChooseNextDestination()
    {
        Transform thisTransform = NPCManager.Instance.Move.GetUniqueSpawnPoint();
        if (thisTransform != null) curDestination = thisTransform;
        else curDestination = this.transform;
    }

    public IEnumerator StandForAwhile()
    {
        float standTime = Random.Range(minStandTime, maxStandTime);
        yield return new WaitForSeconds(standTime);
        AssignRandomState();
    }

    public void NPCEnterConvo(GameObject player)
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        transform.DOLookAt(player.transform.position, 1f, AxisConstraint.None, null).SetUpdate(true);
        NPCManager.Instance.curTalkingNPC = transform.gameObject;
        currentState = NPCDefine.MoveState.Talk;
        agent.isStopped = true;
        animator.Play(NPCManager.Instance.Anim.NPCAnimDictionary[NPCDefine.AnimType.Standing][0].name);
    }

    public void NPCExitConvo()
    {
        animator.updateMode = AnimatorUpdateMode.Normal;
    }
}
