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

        // 더미 데이터를 로드
        InitDummyData();
    }


    #region
    private void TestInit()
    {
        _npdIDStart = DataGetter.Instance.NpcList.Count;

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
        PlayerManager.Instance.player.FreezeAndUnFreezePlayer(false);

        npcsStr = npcsStr.Trim();
        npcsStr = npcsStr.Replace("json", "").Replace("`", "");

        Debug.Log($"잘왔어요 원본, {npcsStr}");

        // JSON 객체에서 Npcs 배열 추출
        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(npcsStr);
        var npcsArray = jsonObject["Npcs"].ToString();

        // Npcs 배열을 List<MuhanInfo>로 변환
        List<MuhanInfo> npcList = JsonConvert.DeserializeObject<List<MuhanInfo>>(npcsArray);

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

    public void InitDummyData()
    {
        string dummyData = @"
        {
        ""Npcs"": [
            {
                ""NpcName"": ""귀공자 미치광이"",
                ""NpcSex"": ""male"",
                ""NpcAge"": 35,
                ""KeyWord"": ""#권위적 #미치광이 #신사"",
                ""Concern"": ""자신이 이전 생에 귀족이었다고 굳게 믿음. 주변 사람들이 이 사실을 인정하지 않아 스트레스가 많음."",
                ""WantItem"": ""왕관"",
                ""Mbtis"": [1, -1, 0, 1],
                ""Personality"": ""자신을 귀족이라고 생각하며, 그에 걸맞은 대우를 받길 원함. 권위적이며 자존심이 강하고, 미치광이처럼 언행이 비이성적. 무례하게 대우받으면 화를 냄. 그와 반대로, 자신을 이해해주거나 신사처럼 대하는 사람에게는 호의적."",
                ""DialogueStyle"": ""기본: 고풍스러운 언어 사용, 자신을 귀족으로 생각하며 자주 '폐하'라 칭함. 상대가 자신을 못 알아볼 때: '흥! 감히 미천한 자가 내 정체를 모른다고?' 같은 비웃는 말투 사용."",
                ""Example"": ""persuasion=0: '이 물건이 내 귀족적 품격을 더해줄 수 있는지 의문이 드는군. 하지만 그대의 설득이 흥미롭기도 하니 좀 더 듣도록 하지.', persuasion=1: '말솜씨가 그야말로 황제 급이군. 이 물건을 통해 나의 귀공 자적 위치를 더욱 견고히 할 수 있을 것 같군.', persuasion=-1: '참으로 실망스럽군! 그대의 설득은 내 위대함을 이해하지 못한 채 허공에 메아리만 칠 뿐!', 화가 나거나 기분이 나쁠때: '이 무례한 자들이! 내 명예를 가볍게 여기다니 감히 이르지 마라!', 그 외: '하, 이거 생각보다 재미있는 복식이로군. 귀족다운 쓰임새가 있을지도 모르겠군.', '당신이 진정으로 내 지위와 권위를 인정한다면, 나도 그대에게 기꺼이 호의를 표하겠소.'"",
                ""Npclooks"": {
                    ""EyebrowType"": ""Aggressive"",
                    ""SexType"": ""Male"",
                    ""AgeType"": ""Common"",
                    ""BackpackType"": ""None"",
                    ""FullbodyType"": ""Party"",
                    ""GlassesType"": ""None"",
                    ""GloveType"": ""LadyGloves"",
                    ""HairType"": ""None"",
                    ""HatType"": ""Gentleman"",
                    ""MustacheType"": ""Beard"",
                    ""OuterwearType"": ""Suit"",
                    ""PantsType"": ""Pants"",
                    ""ShoeType"": ""Boots""
                }
            },
            {
                ""NpcName"": ""소심한 괴짜 펭귄"",
                ""NpcSex"": ""unknown"",
                ""NpcAge"": 28,
                ""KeyWord"": ""#소심 #괴짜 #펭귄팬"",
                ""Concern"": ""자신을 펭귄으로 여기고, 주위 사람들 대신 펭귄들과 소통할 방법을 찾고자 함."",
                ""WantItem"": ""펭귄 인형"",
                ""Mbtis"": [-1, 1, 0, 1],
                ""Personality"": ""정말 소심하고 겁이 많으며, 자신을 펭귄이라고 생각함. 다른 사람들과 어울리는 것을 무서워하지만, 펭귄과 관련된 일이라면 대담해지는 괴짜. 종종 혼자 펭귄 소리를 내며 놀람."",
                ""DialogueStyle"": ""기본: 조용하고 수줍은 말투, 자신이 곤란할 때 '꾸욱꾸욱' 같은 펭귄 소리를 내며 당황함. 자신이 펭귄일 때: '펭귄 동지님들, 오늘은 또 어떤 날인가요?' 같은 말을 사용."",
                ""Example"": ""persuasion=0: '음... 정말 이 인형이 저와 잘 어울릴까요? 아니면 그냥 사람들에게 이상하게 보일까요...?', persuasion=1: '그렇게 애써 설명해 주시니 이해했어요. 이건 분명 제가 원하는 딱 맞는 물건이에요!', persuasion=-1: '아... 저는 그런 과감한 선택을 할 수 없는 사람 같아요. 이번에는 패스할게요...', 화가 나거나 기분이 나쁠때: '그... 그렇게 말씀하시면 펭귄인 제가 너무 슬퍼지잖아요...', 그외: '펭귄인 제가 사는 건 조금 복잡하지만, 당신에게는 감사해요. 우리 친구가 될 수 있을까요?', '펭귄 친구들과 놀 수 있다면 정말 행복할 텐데요.'"",
                ""Npclooks"": {
                    ""EyebrowType"": ""Timid"",
                    ""SexType"": ""Unknown"",
                    ""AgeType"": ""Common"",
                    ""BackpackType"": ""None"",
                    ""FullbodyType"": ""Halloween"",
                    ""GlassesType"": ""Sunglasses"",
                    ""GloveType"": ""None"",
                    ""HairType"": ""None"",
                    ""HatType"": ""CuteCostume"",
                    ""MustacheType"": ""None"",
                    ""OuterwearType"": ""TShirt"",
                    ""PantsType"": ""Shorts"",
                    ""ShoeType"": ""Sneakers""
                }
            },
            {
                ""NpcName"": ""느끼한 장미기사"",
                ""NpcSex"": ""male"",
                ""NpcAge"": 30,
                ""KeyWord"": ""#느끼한 #매력주의자 #장미기사"",
                ""Concern"": ""최근 주위 사람들이 자신의 매력을 알아주지 않아 고민. 다시 자신의 존재감을 드러낼 방법을 찾고 있음."",
                ""WantItem"": ""고급 향수"",
                ""Mbtis"": [-1, 1, 1, 0],
                ""Personality"": ""스스로를 매력적이라 믿으며, 언제 어디서나 매력 발산. 느끼한 말투와 화려한 제스처로 대화를 이어나가며, 자신이 주인공이라는 생각에 빠져 있음. 칭찬이나 주목을 받으면 기분이 좋아지고 더욱 열정적으로 행동함."",
                ""DialogueStyle"": ""기본: 끼를 부리는 듯한 느릿한 말투, 자신을 '장미기사'라 칭하며 상대를 지나치게 칭찬하는 마법 같은 말투 사용. 감미로운 멜로디처럼 들릴 정도로 말한다."",
                ""Example"": ""persuasion=0: '이 향이 나의 매력에 날개를 달아줄 수 있을까요? 그래도 지금의 저는 이미 환상적인 향기를 뿜고 있는데 말이죠~', persuasion=1: '아, 당신의 설명은 저의 마음을 사로잡았어요. 이 향기가 저와 하나가 되게 할 수 있겠군요!', persuasion=-1: '아쉽게도 이번의 유혹은 저의 뜨거운 심장을 흔들지 못했군요.', 화가 나거나 기분이 나쁠때: '어머나, 이토록 무례한 대우는 처음이군요. 제 반짝이는 매력을 무시하다니, 감히!', 그외: '장미가 만개한 봄날처럼, 나의 매력은 한결같이 빛나고 있답니다. 그러니 함께 즐겨보지 않겠어요?', '이 장미기사의 매력을 당신도 느껴보세요. 어때요, 함께 모험을 떠나지 않겠습니까?'"",
                ""Npclooks"": {
                    ""EyebrowType"": ""Aggressive"",
                    ""SexType"": ""Male"",
                    ""AgeType"": ""Common"",
                    ""BackpackType"": ""None"",
                    ""FullbodyType"": ""OuterSpace"",
                    ""GlassesType"": ""None"",
                    ""GloveType"": ""LadyGloves"",
                    ""HairType"": ""Oriental"",
                    ""HatType"": ""Gentleman"",
                    ""MustacheType"": ""Thin"",
                    ""OuterwearType"": ""Suit"",
                    ""PantsType"": ""Pants"",
                    ""ShoeType"": ""Boots""
                }
            }
        ]
    }";

        Debug.Log("더미 데이터를 로드합니다.");
        NpcsReceive(dummyData);
    }

}
