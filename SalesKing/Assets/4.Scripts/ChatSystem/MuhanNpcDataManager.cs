using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using static Define;
using static NPCDefine;

/// <summary>
/// MuhanInfo[] npcs 안에
/// 1. npcID, npclooks가 있어.
/// 2. 언니는 npclooks만 필요한가?
/// 
/// Server에서 만들어와서
/// 1. StoryNpcSO에 걔네 int npc ID 3개 1day로 넣어줘.
/// 2. DataGetter의 Npclist에 걔네 
/// </summary>
public class MuhanNpcDataManager : MonoBehaviour
{ 

    private static MuhanNpcDataManager instance;
    public static MuhanNpcDataManager Instance
    {
        get
        {
            if(instance != null)
                return instance;
            return null;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //----------------------------//----------------------------
    [Header("Just For Test")]
    [SerializeField] GameObject[] lookSetters;
    //[SerializeField] TextMeshProUGUI[] testtexts;
    List<NPCLooksSetter> npcLooksSetters = new List<NPCLooksSetter>();
    //----------------------------//----------------------------

    [SerializeField] StoryNpcSO storyNpcSO;
    
    [HideInInspector]
    public List<MuhanInfo> npcs = new List<MuhanInfo>(); //n. n+1. n+2 각각의 npc info, npc looks
    public List<int> npc_IDs = new List<int>(); //새로 더해질 3명 아이디 n, n+1, n+2

    private int _npdIDStart = 0;
    private int _replyTurn = 0;
    
    private string[] npcOptionA = { "권위적인", "소심한", "독특한", "쾌활한", "엉뚱한", "느끼한", "성격 나쁜", "야비한" };
    private string[] npcOptionB = { "미치광이", "괴짜", "개성있는" };

    public class NpcLooks
    {
        public EyebrowType EyebrowType { get; set; }
        public SexType SexType { get; set; }
        public AgeType AgeType { get; set; }
        public BackpackType BackpackType { get; set; }
        public FullBodyType FullbodyType { get; set; }
        public GlassesType GlassesType { get; set; }
        public GloveType GloveType { get; set; }
        public HairType HairType { get; set; }
        public HatType HatType { get; set; }
        public MustacheType MustacheType { get; set; }
        public OuterwearType OuterwearType { get; set; }
        public PantsType PantsType { get; set; }
        public ShoeType ShoeType { get; set; }
    }
    public class MuhanInfo : NpcInfo
    {
        //muhanmode
        public NpcLooks NpcLooks;
    }
    //-----------------------------------------------------------
    private void Start()
    {
        TestInit();
    }

    #region
    private void TestInit()
    {
        //extract npclookssetter from every gameobject
        foreach (var look in lookSetters)
        { 
            if(look.TryGetComponent<NPCLooksSetter>(out var lookSetterScript))
            {
                npcLooksSetters.Add(lookSetterScript);
            }
        }
    }
    private void NpcLookSetting(NpcLooks npcLooks, int idx)
    {
        if(idx < npcLooksSetters.Count)
            npcLooksSetters[idx].AssignMuhanMeshes(npcLooks);
    }
    #endregion

    public void OnClickInit()
    {
        if (_replyTurn % 2 == 0)
        {
            cntupTurn();
            Init();
        }
    }

    private void Init()
    {
        _npdIDStart = DataGetter.Instance.NpcList.Count;
        
        int randIdx;
        int randIdx2;
        string gameSend = "";

        for (int i = 0; i < 3; i++)
        {
            randIdx = UnityEngine.Random.Range(0, npcOptionA.Length);
            randIdx2 = UnityEngine.Random.Range(0, npcOptionB.Length);

            gameSend += $" {npcOptionA[randIdx]} {npcOptionB[randIdx2]} Npc 하나 만들어줘.\n";
        }
        gameSend += "총 npc 3개 프로필을 만들어줘. system prompt의 example 형식처럼 말이야.";
        ServerManager.Instance.GetGPTReply(gameSend, SendChatType.MuhanInit);
    }

    public void NpcsReceive(string npcsStr)
    {
        cntupTurn();
        PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);

        npcsStr = npcsStr.Trim();
        npcsStr = npcsStr.Replace("json", "").Replace("`", "");

        Debug.Log($"잘왔어요 원본, {npcsStr}");

        List<MuhanInfo> npcList = JsonConvert.DeserializeObject<List<MuhanInfo>>(npcsStr);
        
        int idx = 0;
        npc_IDs.Clear();
        // 각각 개별 처리
        foreach (var npc in npcList)
        {
            NpcLookEdit(npc);
            ConcatInfo(npc);
            AddDataToJsonNpcDict(npc);
            NpcLookSetting(npc.NpcLooks, idx++);
        }

        City_TabletDataManager.Instance.InitNpc(false);
        //InjectIDtoStorySO();
    }

    private void NpcLookEdit(MuhanInfo npc)
    {
        NpcLooks npcLooks = npc.NpcLooks;

        if(npcLooks.FullbodyType != FullBodyType.None)
        {
            npcLooks.OuterwearType = OuterwearType.None;
            npcLooks.PantsType = PantsType.None;
        }

        if(npcLooks.HatType != HatType.None)
        {
            npcLooks.HairType = HairType.None;
        }

    }

    private void ConcatInfo(MuhanInfo npc)
    {
        npc.NpcID = _npdIDStart++;
        npc.ItemCategory = ItemCategory.Random;

        npcs.Add(npc);
        npc_IDs.Add(npc.NpcID);
    }

    private void AddDataToJsonNpcDict(MuhanInfo muhanInfo)
    {
        NpcInfo npc = muhanInfo;
        DataGetter.Instance.NpcList.Add(npc);

        string logMessage = $"NpcID: {npc.NpcID}, " +
                                $"NpcAge: {npc.NpcAge}, " +
                                $"Mbtis: [{string.Join(", ", npc.Mbtis)}], " +
                                $"ItemCategory: {npc.ItemCategory}, " +
                                $"NpcName: {npc.NpcName}, " +
                                $"NpcSex: {npc.NpcSex}, " +
                                $"KeyWord: {npc.KeyWord}, " +
                                $"Concern: {npc.Concern}, " +
                                $"WantItem: {npc.WantItem}, " +
                                $"Personality: {npc.Personality}, " +
                                $"DialogueStyle: {npc.DialogueStyle}, " +
                                $"Example: {npc.Example}";

        Debug.Log(logMessage);
        //texts.text = $"닉네임 : {npc.NpcName}\n 키워드 : {npc.KeyWord} \n거래 물품 : {npc.WantItem} \n";
        Debug.Log($"DataGetter.Instance.NpcList[_npdIDStart].NpcName; {npc.NpcID}, {DataGetter.Instance.NpcList[_npdIDStart - 1].NpcName}");
    }

    private void cntupTurn()
    {
        _replyTurn++;
    }
}
