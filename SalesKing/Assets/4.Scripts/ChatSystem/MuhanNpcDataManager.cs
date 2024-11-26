using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using static Define;
using static NPCDefine;
using static StoryNpcSO;

public class MuhanNpcDataManager : Singleton<MuhanNpcDataManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => false;

    [SerializeField] StoryNpcSO storyNpcSO;
    [HideInInspector]
    public NpcLooks[] todayNpcLooks = new NpcLooks[3];
    public List<int> npc_IDs = new List<int>();

    private MuhanInfo[] npcs = new MuhanInfo[3];
    private int _npdIDStart = 0;
    private string _npcLooksStr;
    private string[] npcOption = { "웃기고 개성있는", "독특한", "말이 잘 통하는" };

    public class NpcLooks
    {
        public EyebrowType EyebrowType { get; set; }
        public SexType SexType { get; set; }
        public AgeType AgeType { get; set; }
        public BackpackType BackpackType { get; set; }
        public FullbodyType FullbodyType { get; set; }
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
    private void Init()
    {
        _npdIDStart = DataGetter.Instance.NpcList.Count;
        int randIdx = UnityEngine.Random.Range(0, npcOption.Length);
        ServerManager.Instance.GetGPTReply(Define.GameMode.Infinity, $"{npcOption[randIdx]} Npc 하나 만들어줘.", SendChatType.MuhanInit);
    }

    public void NpcsReceive(String[] npcsStr)
    {
        for (int i = 0; i < 3; i++)
        {
            npc_IDs.Add(++_npdIDStart);
            ConcatInfo(npcsStr[i], i);
            todayNpcLooks[i] = npcs[i].NpcLooks;
        }

        InjectIDtoStorySO();
    }

    private void ConcatInfo(string npcStr, int idx)
    {
        Debug.Log($"잘 왔어요~ {npcStr}");
        npcs[idx] = JsonConvert.DeserializeObject<MuhanInfo>(npcStr);
        npcs[idx].NpcID = _npdIDStart;
        npcs[idx].ItemCategory = ItemCategory.Random;
        Debug.Log($"유후~ {npcs[idx].NpcLooks.HatType}");
    }

    private void InjectIDtoStorySO()
    {
        StoryNpcSet storyNpcSet = new StoryNpcSet();
        storyNpcSet.npc_IDs = npc_IDs;

        storyNpcSO.storyNpcs.Add(storyNpcSet);
        npc_IDs.Clear();
    }
}
