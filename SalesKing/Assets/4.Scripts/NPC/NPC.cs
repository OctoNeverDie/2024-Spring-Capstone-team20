using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static NPCDefine;

public class NPC : MonoBehaviour
{
    public Transform destination;

    public GameObject myCanvas;

    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private NPCLooksSetter looks;

    public float speed = 0.5f;
    public float rotationSpeed = 5.0f; // 서서히 회전하기 위한 속도

    public Transform curDestination;

    public float minStandTime = 10f;
    public float maxStandTime = 30f;

    public int NpcID = 0;
    [HideInInspector]
    public bool Talkable;

    private bool isCheckDestination = false;


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Talkable = true;
        looks = GetComponent<NPCLooksSetter>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (isCheckDestination)
        {
            // 도착 여부 확인
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    isCheckDestination = false;
                    SetNPCToTalkingState();
                }
            }
            
        }
    }

    public void SetNPCDestination(Vector3 position, bool isWalkIn)
    {
        agent.SetDestination(position);
        PlayRandomNPCAnimByAnimType(AnimType.Moving);

        // 걸어들어오는 경우
        if (isWalkIn)
        {
            isCheckDestination = true;
        }
        // 걸어나가는 경우
        else
        {

        }
        
    }

    public void SetNPCToTalkingState()
    {
        // 플레이어 정의
        Player myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();

        // convo enter
        myPlayer.PlayerEnterConvo(this.gameObject);
        NPCEnterConvo(myPlayer.gameObject);
        ChatManager.Instance.Init(NpcID);

        // 회전
        this.transform.rotation = NPCManager.Instance.StandPoint.rotation;

        // 애니메이션 설정
        PlayRandomNPCAnimByAnimType(AnimType.Standing);
        Talkable = false;
    }

    public void PlayNPCAnimByEmotion(Define.Emotion emotion)
    {
        Debug.Log("여기서 감정 호출: "+emotion.ToString());
        switch (emotion)
        {
            case Define.Emotion.best:
                PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.VeryPositive);
                break;
            case Define.Emotion.good:
                PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Positive);
                break;
            case Define.Emotion.normal:
                PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.SlightlyPositive);
                break;
            case Define.Emotion.bad:
                PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Negative);
                break;
            case Define.Emotion.worst:
                PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.VeryNegative);
                break;
            default: break;
        }
        
    }

    public void PlayRandomNPCAnimByAnimType(NPCDefine.AnimType type)
    {
        /*
        if (animator == null)
        {
            Debug.LogError("Animator가 null입니다. Animator를 설정해주세요.");
            return;
        }

        if (NPCManager.Anim == null)
        {
            Debug.LogError("NPCManager.Anim이 null입니다. NPCManager를 확인하세요.");
            return;
        }

        if (!NPCManager.Anim.NPCAnimDictionary.ContainsKey(type))
        {
            //Debug.LogError($"NPCAnimDictionary에 {type} 키가 없습니다.");
            return;
        }

        if (animations.Count == 0)
        {
            //Debug.LogError($"NPCAnimDictionary[{type}]가 비어있습니다.");
            return;
        }
        */
        var animations = NPCManager.Anim.NPCAnimDictionary[type];

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        animator.Rebind();

        int rand_int = Random.Range(0, animations.Count);
        animator.Play(animations[rand_int].name);
    }


    public void NPCEnterConvo(GameObject player)
    {
        transform.DOLookAt(player.transform.position, 1f, AxisConstraint.None, null).SetUpdate(true);
        NPCManager.Instance.curTalkingNPC = transform.gameObject.GetComponent<NPC>();
        PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Idle);
    }

    public void NPCExitConvo()
    {
        SetNPCDestination(NPCManager.Instance.SpawnPoint.position, false);
        int npc_index = NPCManager.Spawner.cur_NPC_index;

        if (npc_index < 3)
        {
            UIManager.Instance.Main.NextNPCButton.SetActive(true);
        }
    }

}
