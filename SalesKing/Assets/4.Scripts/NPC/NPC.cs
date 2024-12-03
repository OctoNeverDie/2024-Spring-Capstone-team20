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


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Talkable = true;
        looks = GetComponent<NPCLooksSetter>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
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
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        animator.Rebind();

        int anim_count = NPCManager.Anim.NPCAnimDictionary[type].Count;
        int rand_int = Random.Range(0, anim_count);
        animator.Play(NPCManager.Anim.NPCAnimDictionary[type][rand_int].name);
    }

    public void NPCEnterConvo(GameObject player)
    {
        transform.DOLookAt(player.transform.position, 1f, AxisConstraint.None, null).SetUpdate(true);
        NPCManager.Instance.curTalkingNPC = transform.gameObject.GetComponent<NPC>();
        PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Idle);
    }

    public void NPCExitConvo()
    {
        PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Standing);
    }

}
