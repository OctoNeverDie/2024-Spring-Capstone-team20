using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform destination;

    public GameObject myCanvas;

    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
        myCanvas = transform.Find("Canvas").gameObject;
    }

    void Start()
    {
        agent.speed = speed;
        AssignRandomLooks();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }




    void AssignRandomLooks()
    {
        NPCLooks looks = transform.GetComponent<NPCLooks>();

        foreach (NPCDefine.MeshType category in System.Enum.GetValues(typeof(NPCDefine.MeshType)))
        {
            looks.AssignCustomMesh(category);
        }
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
        //int randAnimIndex = Random.Range(0, NPCManager.Anim.NPCAnimDictionary[type].Count);
        //Debug.Log("애니메이터 상태는: "+animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        animator.Rebind();
        animator.Play(NPCManager.Anim.NPCAnimDictionary[type][0].name);
        Debug.Log("애니메이션 출력: "+NPCManager.Anim.NPCAnimDictionary[type][0].name);
    }

    public void NPCEnterConvo(GameObject player)
    {
        transform.DOLookAt(player.transform.position, 1f, AxisConstraint.None, null).SetUpdate(true);
        NPCManager.Instance.curTalkingNPC = transform.gameObject.GetComponent<NPC>();
       // animator.Play(NPCManager.Anim.NPCAnimDictionary[NPCDefine.AnimType.Moving][0].name);
    }

}
