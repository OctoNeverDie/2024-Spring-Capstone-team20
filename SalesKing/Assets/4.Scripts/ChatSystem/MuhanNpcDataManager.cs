using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using static Define;
using static NPCDefine;
using static StoryNpcSO;

/// <summary>
/// MuhanInfo[] npcs 안에
/// 1. npcID, npclooks가 있어.
/// 2. 언니는 npclooks만 필요한가?
/// 
/// Server에서 만들어와서
/// 1. StoryNpcSO에 걔네 int npc ID 3개 1day로 넣어줘.
/// 2. DataGetter의 Npclist에 걔네 
/// </summary>
public class MuhanNpcDataManager : Singleton<MuhanNpcDataManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    //----------------------------//----------------------------
    [Header("Just For Test")]
    [SerializeField] GameObject[] lookSetters;
    List<NPCLooksSetter> npcLooksSetters = new List<NPCLooksSetter>();
    //----------------------------//----------------------------

    [SerializeField] StoryNpcSO storyNpcSO;
    
    [HideInInspector]
    public List<MuhanInfo> npcs = new List<MuhanInfo>(); //n. n+1. n+2 각각의 npc info, npc looks
    public List<int> npc_IDs = new List<int>(); //새로 더해질 3명 아이디 n, n+1, n+2

    private int _npdIDStart = 0;
    
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
        Init();
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

            gameSend += $" {npcOptionA[randIdx]} {npcOptionB[randIdx2]} Npc 하나 만들어줘. Npc 설정 전부 합해서 700 토큰을 넘기지 마.,";
        }
        
        ServerManager.Instance.GetGPTReply(Define.GameMode.Infinity, gameSend, SendChatType.MuhanInit);
    }

    public void NpcsReceive(String[] npcsStr)
    {
        for (int i = 0; i < 3; i++)
        {
            npc_IDs.Add(_npdIDStart++);
            ConcatInfo(npcsStr[i], i);
            AddDataToJsonNpcDict(npcs[npcs.Count - 1]);//막 추가한, 마지막 요소
            NpcLookSetting(npcs[npcs.Count - 1].NpcLooks, i);
        }

        InjectIDtoStorySO();
    }

    private void ConcatInfo(string npcStr, int idx)
    {
        Debug.Log($"잘 왔어요~ {npcStr}");
        npcStr = npcStr.Replace("json", "").Replace("`", "");
        MuhanInfo npcMuhanProfile = JsonConvert.DeserializeObject<MuhanInfo>(npcStr);
        npcMuhanProfile.NpcID = _npdIDStart;
        npcMuhanProfile.ItemCategory = ItemCategory.Random;
        npcs.Add(npcMuhanProfile);
    }

    private void InjectIDtoStorySO()
    {
        StoryNpcSet storyNpcSet = new StoryNpcSet();
        storyNpcSet.npc_IDs = npc_IDs;

        storyNpcSO.storyNpcs.Add(storyNpcSet);
        
        npc_IDs.Clear();
    }

    private void AddDataToJsonNpcDict(MuhanInfo muhanInfo)
    {
        NpcInfo npc = muhanInfo;
        DataGetter.Instance.NpcList.Add(npc);

        //string logMessage = $"NpcID: {npc.NpcID}, " +
        //                        $"NpcAge: {npc.NpcAge}, " +
        //                        $"Mbtis: [{string.Join(", ", npc.Mbtis)}], " +
        //                        $"ItemCategory: {npc.ItemCategory}, " +
        //                        $"NpcName: {npc.NpcName}, " +
        //                        $"NpcSex: {npc.NpcSex}, " +
        //                        $"KeyWord: {npc.KeyWord}, " +
        //                        $"Concern: {npc.Concern}, " +
        //                        $"WantItem: {npc.WantItem}, " +
        //                        $"Personality: {npc.Personality}, " +
        //                        $"DialogueStyle: {npc.DialogueStyle}, " +
        //                        $"Example: {npc.Example}";

        // Debug.Log(logMessage);
        // Debug.Log($"DataGetter.Instance.NpcList[_npdIDStart].NpcName; {DataGetter.Instance.NpcList[_npdIDStart -1].NpcName}");
    }
}
