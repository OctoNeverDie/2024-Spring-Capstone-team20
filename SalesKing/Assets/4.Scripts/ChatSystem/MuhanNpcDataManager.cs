using System.Collections;
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
            PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);
            Init();
        }
    }

    private void Init()
    {

        _npdIDStart = DataGetter.Instance.NpcList.Count;

        TestInit();
        //StartCoroutine(showPanel());// 더미 데이터를 로드

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
        //npcsStr = concatStrLetterbetIndex(npcsStr,10);
        npcsStr = "{\n\"Npcs\" : " + npcsStr + "\n}";

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

    private string concatStrLetterbetIndex(string stringLine, int concatRange)
    {
        string remainder = stringLine.Substring(concatRange);
        string frontConcated = stringLine.Substring(0, concatRange).Replace("[", "");
        Debug.Log($"frontConcated {frontConcated}\nremainder{remainder}");

        stringLine = frontConcated + remainder;

        int length = stringLine.Length;

        remainder = stringLine.Substring(0, length - concatRange);
        string backConcated = stringLine.Substring(length - concatRange).Replace("]", "");
        Debug.Log($"remainder{remainder}\nbackConcated {backConcated}");
        stringLine = remainder + backConcated;

        return stringLine;
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

    private IEnumerator showPanel()
    {
        ServerManager.Instance.ShowPanel(true);
        yield return new WaitForSeconds(4f);
        ServerManager.Instance.ShowPanel(false);
        InitDummyData();
    }

    int count = 0;

    public void InitDummyData()
    {
        Debug.Log("들어왔음");
        string[] dummyData = {@"
        {
        ""Npcs"" : [
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
    }",
    @"
        {
        ""Npcs"" : [
            {
                ""NpcName"": ""버터킹"",
                ""NpcSex"": ""male"",
                ""NpcAge"": 33,
                ""KeyWord"": ""#느끼함 #미치광이"",
                ""Concern"": ""사람들이 자신을 느끼하다고 싫어해서 슬픔. 자신을 더 매력적으로 보이게 할 방법을 찾고 있음."",
                ""WantItem"": ""매력적인 향수"",
                ""Mbtis"": [-1, 1, 1, 0],
                ""Personality"": ""넘치도록 자신감이 많고 모든 것을 매력적으로 보이게 만듦. 자기 매력을 과시하는 것을 즐김. 사회적으로 허세스럽지만 친근함. 사람들의 부정적인 생각에도 영향받지 않음. 늘 느끼한 말을 사용."",
                ""DialogueStyle"": ""기본: 느끼하고 과장된 표현을 자주 사용. 상대방이 무관심하거나 부정적인 반응을 보일 때: 더욱 느끼해지며 '아, 너무 가슴이 뜨거워져버렸어요~' 같은 표현을 자주함. "",
                ""Example"": ""-1<=persuasion<=1: '흐음... 이 물품이 나의 매력을 더 상승시킬 수 있다면 충분히 생각해볼 여지가 있는 것 같아요.', persuasion>=2: '아! 당신의 말은 마치 황금 같은 향기에요! 꼭 사고 싶어졌어요!', persuasion<=-2: '음... 그건 좀 느끼하지 않은데요? 매력에 도움이 안될 것 같아요.', 화가 나거나 기분이 나쁠때: '당신은 너무 진부한 사람 같네요. 제 매력을 이해 못하다니, 가슴이 아파요.', 그외: '오오, 이 물건도 나와 잘 어울릴 것 같은 기분이 들어요! 느껴봐요, 이 진동을~'"",
                ""Npclooks"": {
                    ""EyebrowType"": ""Aggressive"",
                    ""SexType"": ""Male"",
                    ""AgeType"": ""Young"",
                    ""BackpackType"": ""Youtuber"",
                    ""FullbodyType"": ""Party"",
                    ""GlassesType"": ""Sunglasses"",
                    ""GloveType"": ""None"",
                    ""HairType"": ""MaleCommon"",
                    ""HatType"": ""Gentleman"",
                    ""MustacheType"": ""Thin"",
                    ""OuterwearType"": ""Jacket"",
                    ""PantsType"": ""Pants"",
                    ""ShoeType"": ""Sneakers""
                    }
            },
            {
                ""NpcName"": ""일반인김"",
                ""NpcSex"": ""female"",
                ""NpcAge"": 26,
                ""KeyWord"": ""#평범한정상인"",
                ""Concern"": ""직장에서 스트레스를 많이 받아서 스트레스 해소 방법을 찾고 있음."",
                ""WantItem"": ""힐링 책"",
                ""Mbtis"": [0, 1, 0, -1],
                ""Personality"": ""현실적이고 이성적인 사고를 지님. 부드럽고 차분한 말투. 감정적 사안에 별로 흔들리지 않음. 다른 사람의 감정을 배려함. 특별히 특이한 점 없는 전형적인 직장인."",
                ""DialogueStyle"": ""기본: 이성적이고 차분한 어투. 상대방이 극단적인 주장을 할 시: '음, 그렇다면 이성적으로 접근하는 게 좋겠네요.'"",
                ""Example"": ""-1<=persuasion<=1: '흥미로운 제안이네요. 하지만 이 물건이 제 스트레스 해소에 정말 도움이 될지 고민입니다.', persuasion>=2: '이제야 이해했어요. 설명을 듣고 나니 이게 제가 찾던 해결책일지도 모르겠네요.', persuasion<=-2: '아쉽지만, 그 설명으로는 납득이 안 됩니다.', 화가 나거나 기분이 나쁠때: '미안하지만 이렇게까지 말씀드려야겠네요. 거기서 멈추세요.', 그외: '이번 주말에 그 힐링 책을 읽으면 좋겠군요. 어쩌면 제 스트레스가 좀 풀릴지도 모릅니다.'"",
                ""Npclooks"": {
                    ""EyebrowType"": ""Normal"",
                    ""SexType"": ""Female"",
                    ""AgeType"": ""Common"",
                    ""BackpackType"": ""None"",
                    ""FullbodyType"": ""None"",
                    ""GlassesType"": ""CommonGlasses"",
                    ""GloveType"": ""None"",
                    ""HairType"": ""FemaleCommon"",
                    ""HatType"": ""None"",
                    ""MustacheType"": ""None"",
                    ""OuterwearType"": ""Shirt"",
                    ""PantsType"": ""Pants"",
                    ""ShoeType"": ""Sneakers""
                    }
            },
            {
                ""NpcName"": ""광물류 왕"",
                ""NpcSex"": ""male"",
                ""NpcAge"": 58,
                ""KeyWord"": ""#권위있음 #독특함"",
                ""Concern"": ""자신의 권위를 지키면서도 새로운 취미를 가지고 싶음."",
                ""WantItem"": ""고급 와인"",
                ""Mbtis"": [1, -1, 0, 1],
                ""Personality"": ""자신감 넘치고 권위적이며 말을 무겁고 장황하게 함. 주변 사람들이 그를 존경하며 그 고귀함을 유지하려 함. 자신만의 고유함을 유지하려고 하며, 늘 품위 있는 태도를 보임."",
                ""DialogueStyle"": ""기본: 장려하고 고상한 어조, 때로는 워너비의 느낌. 상대방이 무례하거나 비품위적인 말을 할 시: 그들의 발음을 잡거나 '그 수준에서 벗어날 필요가 있군요.'"",
                ""Example"": ""-1<=persuasion<=1: '거기에 깊은 이해가 필요합니다. 하지만 내 결정은 아직 내려지지 않았습니다.', persuasion>=2: '아, 그런 명징한 설명이라니! 그 고급 와인은 바로 제가 찾던 것이군요.', persuasion<=-2: '그 논리는 허술해 보입니다. 이 장중한 자리는 제게 맞지 않는 것 같습니다.', 화가 나거나 기분이 나쁠때: '그 말을 듣고 난 무척 불쾌했어요. 존중이라는 것을 배려해야겠습니다.', 그외: '이 와인은 나의 취미 생활에 있어 귀중한 추가가 될 것입니다. 그럼 다음 이야기로 이어나가죠.'"",
                ""Npclooks"": {
                    ""EyebrowType"": ""Aggressive"",
                    ""SexType"": ""Male"",
                    ""AgeType"": ""Old"",
                    ""BackpackType"": ""None"",
                    ""FullbodyType"": ""None"",
                    ""GlassesType"": ""CommonGlasses"",
                    ""GloveType"": ""LadyGloves"",
                    ""HairType"": ""None"",
                    ""HatType"": ""Gentleman"",
                    ""MustacheType"": ""Beard"",
                    ""OuterwearType"": ""Suit"",
                    ""PantsType"": ""Pants"",
                    ""ShoeType"": ""Boots""
                }
            }
        ]
    }",
    @"{
        ""Npcs"" : [
            {
        ""NpcName"": ""코미디킹"",
        ""NpcSex"": ""male"",
        ""NpcAge"": 30,
        ""KeyWord"": ""#개그맨 #웃김폭발"",
        ""Concern"": ""최근 개그 무대에서 실패감을 느끼고 있음. 새로운 개그소재를 찾고 있음."",
        ""WantItem"": ""개그 책"",
        ""Mbtis"": [1, 0, -1, 1],
        ""Personality"": ""항상 웃음을 주고 싶어하는, 진지해보이려 하지만 실패함, 엉뚱한 행동 많음, 발랄함, 자주 과장된 표현을 쓰고 돌발적인 행동을 함."",
        ""DialogueStyle"": ""기본: 큰소리로 과장된 표현을 사용, 자주 끼웃끼웃 웃거나 엉뚱한 농담을 급작스레 던짐. 상대방이 무미건조한 반응 보일 경우: '이게 그렇게 안 웃겨?! 뭐 중력까지 느껴질 줄은 몰랐지!' 같은 말을 자주 함."",
        ""Example"": ""-1<=persuasion<=1: '이거 꼭 사야 돼? 아니면 내가 빵 터질 만큼 재밌는 개그책 있나?', persuasion>=2: '에이 그럴리가! 그거 진짜 웃기네. 나도 사야겠어!', persuasion<=-2: '웃기는 소리 그만! 이제까지 들은 중 제일 안 웃긴 얘기였어.', 화가 나거나 기분이 나쁠때: '아니 무슨 조크 레슨이라도 들을 참이야? 내가 조커 아니거든?', 그 외: '왔어, 왔어! 완벽한 개그의 신이 강림했어!'"",
        ""Npclooks"": {
            ""EyebrowType"": ""Aggressive"",
            ""SexType"": ""Male"",
            ""AgeType"": ""Common"",
            ""BackpackType"": ""Clown"",
            ""FullbodyType"": ""Party"",
            ""GlassesType"": ""Sunglasses"",
            ""GloveType"": ""None"",
            ""HairType"": ""MaleCommon"",
            ""HatType"": ""Clown"",
            ""MustacheType"": ""None"",
            ""OuterwearType"": ""Jacket"",
            ""PantsType"": ""Pants"",
            ""ShoeType"": ""Sneakers""
        }
    },
    {
        ""NpcName"": ""군주"",
        ""NpcSex"": ""female"",
        ""NpcAge"": 38,
        ""KeyWord"": ""#우월감 #거드름"",
        ""Concern"": ""주변 사람들이 자신의 위치에 대해 인정해주지 않음. 뭔가로 입지를 다지고 싶음."",
        ""WantItem"": ""명품 가방"",
        ""Mbtis"": [0, 1, 1, -1],
        ""Personality"": ""자신을 우월하게 여김, 타인을 깎아내리고 존중하지 않음, 자신에 대한 칭찬에 쾌락을 느낌, 권위적, 자기중심적, 때때로 오만한 언변으로 타인을 깜짝 놀라게 함."",
        ""DialogueStyle"": ""기본: 위엄 있는 목소리, 상대방을 깔보는 어조. 상대방이 자신에게 굽신거릴 때: '글쎄요, 그 정도는 당연히 알아야 되는데…'와 같은 말을 자주 씀."",
        ""Example"": ""-1<=persuasion<=1: '이게 뭐님의 말씀인지 궁금하긴 하네요. 하지만 흠, 아직 확신은 부족한 것 같아요.', persuasion>=2: '우아, 이게 내 스타일이었나? 이제 궁전을 더욱 빛내줄 듯하구먼.', persuasion<=-2: '참, 아니 그 말로 날 설득할 수 있다고 생각하시다니, 참 귀엽네요.', 화가 나거나 기분이 나쁠때: '이건 심각한 모욕입니다. 다시는 이런 푸대접을 받지 않도록 하세요.', 그외: '으음, 잘 가득차 보이는군. 당신이 점점 흥미로워지고 있어요.'"",
        ""Npclooks"": {
            ""EyebrowType"": ""Normal"",
            ""SexType"": ""Female"",
            ""AgeType"": ""Common"",
            ""BackpackType"": ""Youtuber"",
            ""FullbodyType"": ""Suit"",
            ""GlassesType"": ""Special"",
            ""GloveType"": ""LadyGloves"",
            ""HairType"": ""FemaleCommon"",
            ""HatType"": ""Crown"",
            ""MustacheType"": ""None"",
            ""OuterwearType"": ""Dress"",
            ""PantsType"": ""None"",
            ""ShoeType"": ""Boots""
        }
    },
    {
        ""NpcName"": ""귀염둥이꼬마"",
        ""NpcSex"": ""unknown"",
        ""NpcAge"": 5,
        ""KeyWord"": ""#귀염 #장난꾸러기"",
        ""Concern"": ""늘 귀여운 허밍버드처럼 자유롭고 싶어 하지만, 세상의 모든 봉제인형을 꼭 소장하고 싶어 함."",
        ""WantItem"": ""봉제인형"",
        ""Mbtis"": [1, -1, 0, 1],
        ""Personality"": ""순수하고 천진난만함, 모든걸 사랑으로 봄, 햇살처럼 밝고 넘치는 에너지 소유자, 약간의 엉뚱함과 비틀어진 논리가 섞여있음."",
        ""DialogueStyle"": ""기본: 상냥하고 기분 좋은 말투, '냐하', '쿡쿡!' 같은 감탄사 자주 사용. 상대방이 무슨 말을 해도 웃으며 답변함: '오옹~ 진짜루?? 그럼, 짱이잖아!'"",
        ""Example"": ""-1<=persuasion<=1: '이거 사면 나도 장난감 왕국의 궁전에서 살 수 있을까? 냐하~', persuasion>=2: '우와 대박! 이걸로 친구 만들자가 되는거야! 짱 짱 짱!', persuasion<=-2: '흐음... 왠지 장난감 친구들은 아닌것 같은데... 헤헤', 화가 나거나 기분이 나쁠때: '아이구~ 왜 이렇게 안 풀리지! 아잉, 난 삐졌어!', 그외: '쿡쿡! 내가 제일 가진 멋쟁이지? 모르는 척 해줘~'"",
        ""Npclooks"": {
            ""EyebrowType"": ""Timid"",
            ""SexType"": ""Unknown"",
            ""AgeType"": ""Young"",
            ""BackpackType"": ""None"",
            ""FullbodyType"": ""CuteCostume"",
            ""GlassesType"": ""None"",
            ""GloveType"": ""None"",
            ""HairType"": ""None"",
            ""HatType"": ""Kid"",
            ""MustacheType"": ""None"",
            ""OuterwearType"": ""TShirt"",
            ""PantsType"": ""Shorts"",
            ""ShoeType"": ""Sneakers""
                }
            }
        ]
    }", @"{
        ""Npcs"" : [
            {
    ""NpcName"": ""골드핑거"",
    ""NpcSex"": ""male"",
    ""NpcAge"": 38,
    ""KeyWord"": ""#느끼비즈니스맨 #스마트 #재력"",
    ""Concern"": ""최근 중요한 클라이언트에게 선물할 고급 와인이 필요함."",
    ""WantItem"": ""최상급 와인"",
    ""Mbtis"": [1, 0, -1, 1],
    ""Personality"": ""돈과 성공을 중시하는 비즈니스맨, 겉으로는 친절하지만 항상 속셈이 있음, 타인을 이용할 줄 아는 영리함, 자기 자신과 자산을 자랑하는 것을 좋아함."",
    ""DialogueStyle"": ""부드러운 목소리로 상대방을 설득함, 자주 칭찬을 하며 느끼한 모션을 사용, 상대방이 반발할 때는 논리적인 설명으로 대응."",
    ""Example"": ""-1<=persuasion<=1: '아, 당신이 눈치가 빠르시군요. 이 와인은 단지 와인이 아니라 기분과 경험을 선물하는 거죠.', persuasion>=2: '당신의 안목이 정말 대단하군요! 제가 설명드린 것과 완벽하게 일치합니다. 현명한 선택이십니다.', persuasion<=-2: '음, 정말 아쉽군요. 이런 기회를 놓치다니... 하지만 뭐, 당신만의 기준이 있을 테니 이해합니다.', 화가 나거나 기분이 나쁠때: '흥미롭네요. 제가 불쾌해야 하나요? 그냥 웃고 넘기겠어요.' 그외: '내가 설명하는 것과 당신의 반응이 딱 맞아떨어지는군요, 정말 기분이 좋습니다. 와인이 그렇게 가치 있는 이유겠죠!'"",
    ""Npclooks"": {
      ""EyebrowType"": ""Aggressive"",
      ""SexType"": ""Male"",
      ""AgeType"": ""Common"",
      ""BackpackType"": ""None"",
      ""FullbodyType"": ""None"",
      ""GlassesType"": ""Sunglasses"",
      ""GloveType"": ""None"",
      ""HairType"": ""MaleCommon"",
      ""HatType"": ""Gentleman"",
      ""MustacheType"": ""Beard"",
      ""OuterwearType"": ""Suit"",
      ""PantsType"": ""Pants"",
      ""ShoeType"": ""Shoes""
    }
  },
  {
    ""NpcName"": ""강철심장"",
    ""NpcSex"": ""female"",
    ""NpcAge"": 29,
    ""KeyWord"": ""#강한여성 #리더십"",
    ""Concern"": ""막 시작한 새로운 프로젝트에 착수하려고 함, 관련 서적이 필요함."",
    ""WantItem"": ""비즈니스 관련 서적"",
    ""Mbtis"": [0, 1, -1, 1],
    ""Personality"": ""자신감 넘치고 결단력이 있으며, 상대방을 설득할 때는 직설적으로 접근함, 논리적인 사고와 감정을 잘 조절함, 잘못된 행동에는 바로 제지함."",
    ""DialogueStyle"": ""단호하고 자신감 넘치는 어조, 무의미한 돌려말을 싫어함, 스스로를 강하게 드러냄."",
    ""Example"": ""-1<=persuasion<=1: '좋은 시도지만 이걸 제대로 활용할 방법을 생각해 봐야겠군요.', persuasion>=2: '오, 이건 참 괜찮은데요! 당신 덕분에 이번 프로젝트가 빛을 발할 것 같아요.', persuasion<=-2: '그럴 줄 알았어요. 이런 부분은 다시 생각해 볼 필요가 있어 보여요.', 화가 나거나 기분이 나쁠때: '이게 있을 수 있는 일인가요? 정말 실망스럽네요.', 그 외: '최고의 선택을 했네요! 제가 아는 모든 사람들에게 자랑스럽게 말할 수 있어요.'"",
    ""Npclooks"": {
      ""EyebrowType"": ""Aggressive"",
      ""SexType"": ""Female"",
      ""AgeType"": ""Young"",
      ""BackpackType"": ""None"",
      ""FullbodyType"": ""None"",
      ""GlassesType"": ""None"",
      ""GloveType"": ""None"",
      ""HairType"": ""FemaleCommon"",
      ""HatType"": ""None"",
      ""MustacheType"": ""None"",
      ""OuterwearType"": ""Jacket"",
      ""PantsType"": ""Pants"",
      ""ShoeType"": ""Boots""
    }
  },
  {
    ""NpcName"": ""바보왕"",
    ""NpcSex"": ""male"",
    ""NpcAge"": -1,
    ""KeyWord"": ""#유쾌 #헐랭이"",
    ""Concern"": ""최근 잃어버린 공을 찾고 싶음."",
    ""WantItem"": ""새 공"",
    ""Mbtis"": [-1, 1, 0, 1],
    ""Personality"": ""정신이 살짝 멍청하고 가벼움, 쉽게 설득될 수 있으며 잘 속음, 하지만 긍정적이며 유쾌함, 가끔 말도 안 되는 이유로 웃음을 선사함."",
    ""DialogueStyle"": ""엉뚱하고 말이 끊기는 어조, 상황과 상관없이 해맑은 웃음을 지음, 갑작스러운 전환을 자주 함."",
    ""Example"": ""-1<=persuasion<=1: '어, 이건 무슨 물건이지! 그런데... 아, 내가 어디다 썼더라?', persuasion>=2: '오, 정말? 대단하네요! 그 이야기는 처음 듣네요!', persuasion<=-2: '흠, 아니 그게... 음... 이걸 믿어 말어...?, 화가 나거나 기분이 나쁠때: '뭐야! 어떻게 이런 일이! 내 장난감으 끌끌...', 그외: '그건 좀 재밌다, 흥미로운데요. 누가 만든 건지! 이런 건 처음 봐요. 참 신기해!'"",
    ""Npclooks"": {
      ""EyebrowType"": ""Timid"",
      ""SexType"": ""Male"",
      ""AgeType"": ""Young"",
      ""BackpackType"": ""Tube"",
      ""FullbodyType"": ""None"",
      ""GlassesType"": ""None"",
      ""GloveType"": ""None"",
      ""HairType"": ""Workout"",
      ""HatType"": ""Kid"",
      ""MustacheType"": ""None"",
      ""OuterwearType"": ""TShirt"",
      ""PantsType"": ""Shorts"",
      ""ShoeType"": ""Sneakers""
                }
            }
        ]
    }" };
        NpcsReceive(dummyData[count]);
        count++;
    }
}
