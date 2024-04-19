using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static Unit;

public class DialogManager : SlideUI, SelectUIRedirec
{
    public static DialogManager instance;
    Dictionary<int, string[]> msgDictionary;
    TMP_Text textMesh;

    private int dialogMsgId = 0;
    private int dialogMsgPage = 0;
    private int dialogMsgMaxPage = 0;
    private string dialogMsg = "";
    private float dialogMsgCheck = 0f;
    private int dialogPreMsgCheck = 0;
    private float dialogMsgSpeed = 25f;
    private bool dialogMsgDone = false;
    private float dialogStun = 0f;
    private GameObject dialogCursor;

    private bool isQuest = false;
    private bool isCount = false;
    public bool isEvent = true;
    private int[] dialogArgs;

    private GlobalInput input;
    private UIManager uiManager;
    private GameObject uiID;

    private SelectUIRedirec redirec;

    private int replaceIndex = 0;

    private void Awake()
    {
        instance = this;
        textMesh = transform.GetChild(0).GetComponent<TMP_Text>();
        dialogCursor = transform.Find("Cursor").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {        
        Init();
        SlideUiInit();
    }

    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
        DialogUpdate();

        if (isActive && (input.aButtonDown || input.bButtonDown) && GetActive())
        {
            PushAButton();
        }

    }


    private void PushAButton()
    {
        if (dialogStun < 0)
        {
            dialogStun = 0.1f;
            if (dialogMsgDone)
            {                
                if (!GoNextPage())
                {                    
                    if (!(isQuest || isCount))
                    {
                        UnActive();
                    }


                }               
                
            }
        }
    }

    private bool GoNextPage()
    {
        if (dialogMsgPage < dialogMsgMaxPage)
        {
            dialogMsgPage++;
            SetMsg();
            return true;
        }
        else
        {
            return false;
        }
    }


    private void DialogUpdate()
    {
        if (isActive)
        {
            if (dialogMsg.Length > dialogMsgCheck)
            {
                dialogMsgCheck += Time.deltaTime * dialogMsgSpeed;
                var tmpDialogMsgCheck = Mathf.FloorToInt(dialogMsgCheck);
                if (tmpDialogMsgCheck> dialogPreMsgCheck)
                {
                    dialogPreMsgCheck = tmpDialogMsgCheck;
                    ShowMsg(dialogMsg.Substring(0, dialogPreMsgCheck));
                }
            }
            else
            {
                if (!dialogMsgDone)
                {
                    dialogStun = 0.1f;
                    dialogMsgDone = true;

                    if (dialogMsgPage < dialogMsgMaxPage)
                    {
                        dialogCursor.SetActive(true);
                    }
                    else
                    {
                        if (isQuest)
                        {
                            SelectUIActive();
                        }
                        if (isCount)
                        {
                            CountUIActive();
                        }
                    }                    
                }
                
            }

            dialogStun -= Time.deltaTime;
        }
    }

    public void Active(int msgId, SelectUIRedirec redirec, Type type, params int[] args)
    {
        SlideUiActive();
        SetMsgById(msgId);

        uiManager.ActiveUI(uiID);
        this.redirec = redirec;

        if (type != Type.NORMAL)
        {
            if (type == Type.QUEST)
            {
                QuestInit(args);
            }
            if (type == Type.COUNT)
            {
                CountInit(args);
            }
        }
        else
        {
            isQuest = false;
            isCount = false;
        }
        isEvent = true;
        replaceIndex = 0;
    }


    public void Active(int msgId, params int[] args)
    {
        this.dialogArgs = args;
        replaceIndex = 0;

        SlideUiActive();
        SetMsgById(msgId);

        uiManager.ActiveUI(uiID);

        isQuest = false;
        isCount = false;
        isEvent = false;     
        
        
    }



    public void UnActive()
    {
        SlideUiUnActive();

        uiManager.UnActiveUI(uiID);

        input.InputStun();

        if(!(isQuest || isCount) && isEvent)
            EventManager.instance.ActiveNextEvent(0.3f);
    }

    private void SetMsgById(int msgId)
    {
        dialogMsgPage = 0;
        dialogMsgId = msgId;
        dialogMsgMaxPage = msgDictionary[dialogMsgId].Length - 1;
        SetMsg();        
    }

    private void SetMsg()
    {
        ShowMsg("");
        dialogMsg = ReplaceMsg(msgDictionary[dialogMsgId][dialogMsgPage]);
        dialogMsgCheck = 0f;
        dialogPreMsgCheck = 0;
        dialogMsgDone = false;
        dialogCursor.SetActive(false);
        dialogStun = 0.1f;
    }

    private string ReplaceMsg(string msg)
    {
        while (true)
        {
        
            var tmpIndex = msg.IndexOf("{");
            if (tmpIndex == -1) break;

            var replaceArg = dialogArgs[replaceIndex++];

            string replaceStr = null;
            switch (msg.Substring(tmpIndex + 1, 4))
            {
                case "poke":
                    replaceStr = PokemonInfo.Instance.pokemons[replaceArg].name;
                    
                    msg = msg.Replace("{poke}", replaceStr);
                    break;

                case "item":
                    break;

                case "move":
                    replaceStr = PokemonSkillInfo.Instance.skills[replaceArg].name;

                    msg = msg.Replace("{move}", replaceStr);
                    break;
            }            
        }
        return msg;
    }


    private void ShowMsg(string msg)
    {      
        textMesh.text = msg;
    }

    private void Init()
    {
        uiID = gameObject;

        msgDictionary = new Dictionary<int, string[]>();
        SetDialog();

        input = GlobalInput.globalInput;
        uiManager = UIManager.instance;
    }

    private void QuestInit(params int[] args)
    {
        isQuest = true;
        isCount = false;
        this.dialogArgs = args;
    }

    private void CountInit(params int[] args)
    {
        isCount = true;
        isQuest = false;
        this.dialogArgs = args;
    }


    private void SetDialog()//npcID__대화ID
    {
        msgDictionary.Add(0, new string[] { "안녕하세요2", "메세지창 테스트", "이곳은 포켓몬 세계\n많은 포켓몬들이 살고 있다." });

        //NPC1_엄마
        msgDictionary.Add(1001, new string[] { "아 도희!\n옆집의 오박사님이 찾아왔었단다.", "뭔지 너에게\n부탁할 것이 있다고 하셔서", "연구소는 집 왼쪽에 있으니\n어서 찾아가 보렴"});
        msgDictionary.Add(1002, new string[] { "연구소는 집 왼쪽에 있단다." });
        msgDictionary.Add(1003, new string[] { "어머 귀여운 포켓몬이네\n여행 중 몸 조심 하렴" });

        //NPC2_오박사
        msgDictionary.Add(2001, new string[] { "연구소에 잘 왔다!\n나는 오박사 포켓몬 박사란다.", "오늘부터 모험을 시작하게 된것을 축하한단다."});
        msgDictionary.Add(2002, new string[] { "여기 너에게 줄 포켓몬이 있단다 신중하게 고르렴" });
        msgDictionary.Add(2003, new string[] { "여행을 떠나려면 포켓몬이 있어야 한단다." });
        msgDictionary.Add(2004, new string[] { "너의 첫 포켓몬이니 신중하게 고르거라\n천천히 골라도 괜찮단다" });
        msgDictionary.Add(2005, new string[] { "풀 포켓몬 이상해씨란다. 그 포켓몬으로 결정하겠니?" });
        msgDictionary.Add(2006, new string[] { "불 포켓몬 파이리란다. 그 포켓몬으로 결정하겠니?" });
        msgDictionary.Add(2007, new string[] { "물 포켓몬 꼬부기란다. 그 포켓몬으로 결정하겠니?" });
        msgDictionary.Add(2008, new string[] { "여행 응원한단다.\n멋진 포켓몬 트레이너가 되거라" });

        //NPC3_크리스
        msgDictionary.Add(3001, new string[] { "안녕 난 오박사님의 조수 크리스야","앞으로 잘 부탁해!"});
        msgDictionary.Add(3002, new string[] { "난 그럼 이 포켓몬으로 할게!" });
        msgDictionary.Add(3003, new string[] { "서로 포켓몬을 얻었으니 바로 배틀이지!" });
        msgDictionary.Add(3004, new string[] { "즐거운 배틀이었어!\n다음에 또 보자!" });

        //NPC4_연구원
        msgDictionary.Add(4001, new string[] { "아- 바뻐요" });

        //NPC5_연구원
        msgDictionary.Add(5001, new string[] { "오늘도 야근이야" });
        msgDictionary.Add(5002, new string[] { "도희군!","여기 몬스터볼이야\n포켓몬을 잡으려면 몬스터볼이 필요해" });

        //NPC6_연구원
        msgDictionary.Add(6001, new string[] { "여기선 포켓몬을 치료할 수 있어" });

        //NPC7_연구원
        msgDictionary.Add(7001, new string[] { "비상은 주로 비행 타입의 포켓몬을 사용해","약한 상대가 아니니 조심하렴!" });

        //npc8
        msgDictionary.Add(8001, new string[] { "체육관 관장 비상은 너무 강해","내 포켓몬들이 아무것도 못하고 모두 당해버렸어" });

        //npc9
        msgDictionary.Add(9001, new string[] { "체육관 트레이너2" });
        msgDictionary.Add(9002, new string[] { "체육관 트레이너2 전투종료" });

        //npc10
        msgDictionary.Add(10001, new string[] { "체육관 트레이너1 전투" });
        msgDictionary.Add(10002, new string[] { "체육관 트레이너1 전투종료" });

        //npc11
        msgDictionary.Add(11001, new string[] { "풀숲에는 포켓몬이 숨어있다\n언제 튀어나올지 몰라" });

        //npc12
        msgDictionary.Add(12001, new string[] { "포켓몬 트레이너로써의 실력을 확인하려는거?","그렇다면 각지에 있는 포켓몬 체육관에서 배지를 모으는 것이 좋아" });

        //npc13
        msgDictionary.Add(13001, new string[] { "길거리 트레이너" });
        msgDictionary.Add(13002, new string[] { "길거리 트레이너 전투종료" });

        //npc14
        msgDictionary.Add(14001, new string[] { "내 포켓몬이 모두 쓰려져서 치료하러 왔어" });

        //npc15
        msgDictionary.Add(15001, new string[] { "여긴 신기한 아이템이 정말 많아!" });

        //npc16
        msgDictionary.Add(16001, new string[] { "내 피카츄 귀엽지?","난 세상에서 피카츄가 제일 좋아!" });

        //npc17
        msgDictionary.Add(17001, new string[] { "거기에 언덕이 있지?","뛰어 내리는 건 무서워도 풀숲을 걸어가지 않고 마을로 갈 수 있지!" });

        //NPC18_마을여자
        msgDictionary.Add(18001, new string[] { "도희야 좋은 아침", "포켓몬 없이 풀숲에 들어가는 것은 위험하단다."});
        msgDictionary.Add(18002, new string[] { "어! 도희야", "혼자서 어디 가니?","이런! 포켓몬도 지니지 않고 도로에 나가다니 위험해!","근처의 마을까지는 야생의 포켓몬이 튀어 나오는 풀숲이 있으니까"});

        //NPC19_뚱보
        msgDictionary.Add(19001, new string[] { "어이! 도희", "오박사님이 새로운 포켓몬을 발견하셨대" });

        //NPC20
        msgDictionary.Add(20001, new string[] { "여기 맛있는건 혹시 없을까?" });

        //NPC21
        msgDictionary.Add(21001, new string[] { "여기서 파는 약은 사람에게는 효과가 없구나" });

        //NPC22
        msgDictionary.Add(22001, new string[] { "나도 젊었을 때는 힘찬 트레이너였단다!" ,"그런 나로부터 어드바이스!\n포켓몬을 많이 잡아라!","그리고 온화함을 지니고 포켓몬을 대해주는 것이다!"});

        //npc23 간호순
        msgDictionary.Add(23001, new string[] { "안녕하세요!\n포켓몬 센터입니다!", "이곳에서는 포켓몬의 체력을 회복합니다!", "당신의 포켓몬을 쉬게 하겠습니까?" });
        msgDictionary.Add(23002, new string[] { "다음 번에도 방문하시길 기다리겠습니다!" });
        msgDictionary.Add(23003, new string[] { "오래 기다리셨습니다!","맡아 놓으신 포켓몬은 모두 건강해졌습니다!"});
        msgDictionary.Add(23004, new string[] { "안녕하세요!\n포켓몬 센터입니다!", "이곳에서는 포켓몬의 체력을 회복합니다!", "아쉽지만 포켓몬이 없으면 시설을 이용하실 수 없습니다." });
        
        //npc24 간호순
        msgDictionary.Add(24001, new string[] { "간호순 테스트" });

        //npc25 상점주인
        msgDictionary.Add(25001, new string[] { "안녕하세요 프랜들리 숍입니다\n무엇을 도와드릴까요?" }); 
        msgDictionary.Add(25002, new string[] { "감사합니다\n다음에도 또 와주세요!" });

        //npc27
        msgDictionary.Add(27001, new string[] { "풀숲을 지나가다가 벌레포켓몬에게 독을 쏘였었다!","그대로 걸어가다가 포켓몬이 쓰러졌었다!","해독제는 가지고 가는편이 좋아" });

        //npc28
        msgDictionary.Add(28001, new string[] { "내 옆에 있는 컴퓨터를 통해 포켓몬을 맡기고 데려올 수 있단다!" });

        //npc29
        msgDictionary.Add(29001, new string[] { "피카피카!" });

        //npc30~31
        msgDictionary.Add(30001, new string[] { "체육관 뱃지가 없으면 이곳을 통과하실 수 없습니다" });
        msgDictionary.Add(31001, new string[] { "체육관 뱃지가 없으면 이곳을 통과하실 수 없습니다" });

        //npc32 관장
        msgDictionary.Add(32001, new string[] { "관장 테스트" });

        //npc33
        msgDictionary.Add(33001, new string[] { "피카피카!" });

        //npc33
        msgDictionary.Add(35001, new string[] { "npc 전투 테스트1" });
        msgDictionary.Add(35002, new string[] { "npc 전투 테스트 종료" });

        //오브젝트들
        msgDictionary.Add(99001, new string[] { "피카츄인형이다!" });
        msgDictionary.Add(99002, new string[] { "푸린인형이다!" });
        msgDictionary.Add(99003, new string[] { "라디오다!" });
        msgDictionary.Add(99004, new string[] { "TV에서 영화를 하고 있다. 남자아이 둘이서 여행을 하고 있다","기차의 창너머로 별이 보인다.....","....나도 빨리 가봐야지!" });
        msgDictionary.Add(99005, new string[] { "포켓몬 그림책이 모여있군!" });
        msgDictionary.Add(99006, new string[] { "내 방에 있는 나무다!\n어머니가 매일 물을 주신다." });
        msgDictionary.Add(99007, new string[] { "성도지방의 지도다" });
        msgDictionary.Add(99008, new string[] { "냉장고 안에는.....","맛있는 물이 가득\n그리고 달콤한 후르츠 밀크!" });
        msgDictionary.Add(99009, new string[] { "빤짝빤짝거리는 싱크대!" });
        msgDictionary.Add(99010, new string[] { "엄마가 맛있는 밥을 하고 있다!" });

        msgDictionary.Add(99011, new string[] { "오박사님의 컴퓨터다!","........","화면을 이해할 수가 없다...." });
        msgDictionary.Add(99012, new string[] { "책장에는 포켓몬 관련 전공 서적들이 꽂혀있다." });
        msgDictionary.Add(99013, new string[] { "비어 있는 쓰레기통이다." });

        msgDictionary.Add(99014, new string[] { "우리집이다!" });
        msgDictionary.Add(99015, new string[] { "오박사님 연구소다!" });
        msgDictionary.Add(99016, new string[] { "포켓몬 센터다!","여기서 포켓몬을 치료할 수 있다."});
        msgDictionary.Add(99017, new string[] { "포켓몬 숍이다!", "여기서 포켓몬관련 도구들을 구매할 수 있다." });
        msgDictionary.Add(99018, new string[] { "연두마을" });
        msgDictionary.Add(99019, new string[] { "옆마을" });
        msgDictionary.Add(99020, new string[] { "포켓몬 체육관이다!","관장을 쓰러뜨리면 뱃지를 얻을 수 있다." });

        msgDictionary.Add(99021, new string[] { "각종 물건들이 진열되어 있다." });
        msgDictionary.Add(99022, new string[] { "1번 도로" });

        msgDictionary.Add(99023, new string[] { "{poke}(을)를 얻었다." });
        msgDictionary.Add(99024, new string[] { "{item}(을)를 얻었다." });

        msgDictionary.Add(99025, new string[] { "남은 몬스터볼이다.\n오박사님이 잘 키워 주시겠지?" });

        msgDictionary.Add(99026, new string[] { "포켓몬을 회복시키겠습니까?" });

        msgDictionary.Add(99027, new string[] { "해당 아이템은 지금 사용하실 수 없습니다." });
        msgDictionary.Add(99028, new string[] { "중요 아이템은 버릴 수 없습니다." });
        msgDictionary.Add(99029, new string[] { "몇 개 버리시겠습니까?" });
        msgDictionary.Add(99030, new string[] { "몇 개 판매 하시겠습니까?" });
        msgDictionary.Add(99031, new string[] { "중요 아이템은 판매 하실 수 없습니다." });
        msgDictionary.Add(99032, new string[] { "정말 아이템을 버리시겠습니까?" });
        msgDictionary.Add(99033, new string[] { "정말 판매하시겠습니까?" });
        msgDictionary.Add(99034, new string[] { "몇 개 구매 하시겠습니까?" });
        msgDictionary.Add(99035, new string[] { "정말 구매하시겠습니까?" });
        msgDictionary.Add(99036, new string[] { "죄송합니다. 돈이 부족합니다" });
        msgDictionary.Add(99037, new string[] { "감사합니다!" });
        msgDictionary.Add(99038, new string[] { "현재 가지고 있는 포켓몬이 없습니다." });
        msgDictionary.Add(99039, new string[] { "트레이너와 배틀 중에는 도망칠 수 없어!" });

        msgDictionary.Add(99100, new string[] { "{poke}의 정보가 도감에 등록 되었습니다." });

        msgDictionary.Add(99101, new string[] { "{poke}의 {move}!!" });

        msgDictionary.Add(99102, new string[] { "효과가 굉장했다!!" });
        msgDictionary.Add(99103, new string[] { "효과가 별로인듯 하다...." }); 
        msgDictionary.Add(99104, new string[] { "그러나 효과가 없었다...." });
        msgDictionary.Add(99105, new string[] { "{poke}가 쓰러졌다." });
        msgDictionary.Add(99106, new string[] { "{poke}는 {INT.}의 경험치를 획득했다." });
        
        msgDictionary.Add(99107, new string[] { "전투에서 승리하였습니다." });
        msgDictionary.Add(99108, new string[] { "전투에서 패배하였습니다." });

        msgDictionary.Add(99109, new string[] { "야생의 {poke}(이)가 나타났다!" });
        msgDictionary.Add(99110, new string[] { "~~(이)가 승부를 걸어왔다" });
        msgDictionary.Add(99111, new string[] { "~~(은)는 {poke}(을)를 내보냈다!" });
        msgDictionary.Add(99112, new string[] { "가랏 {poke}!!!" });
        msgDictionary.Add(99113, new string[] { "{poke} 너로 정했다!!!" });
        msgDictionary.Add(99114, new string[] { "{poke} 힘내!!!!" });

        msgDictionary.Add(99115, new string[] { "{poke}(은)는 전투할 기력이 없는듯 하다...." });
        msgDictionary.Add(99116, new string[] { "{poke}(은)는 이미 나와 있다!" });

        msgDictionary.Add(99999, new string[] { "테스트용 메세지 입니다." });


    }

    public void OnSelectRedirec(int num, params int[] args)
    {
        UnActive();
        redirec.OnSelectRedirec(num, args);        
    }

    private void SelectUIActive()
    {
        SelectUI select = SelectUI.instance;
        var cursorMaxNum = 1;
        Vector2 pos = new Vector2(-14f, 232f);
        select.Active(cursorMaxNum, "예\n아니오", this, 200f, pos, dialogArgs);
    }

    private void CountUIActive()
    {
        CountUI ui;
        if (dialogArgs[1] == 0)
        {
            ui = CountUI.instance;
        }            
        else
        {
            ui = CountUI.monCountInstance;            
        }
            
        var cursorMaxNum = dialogArgs[0];
        var itmID = dialogArgs[2];
        var redirectID = dialogArgs[3];
        Vector2 pos = new Vector2(-17.3f, 218.8f);
        ui.Active(cursorMaxNum, this, pos, itmID, redirectID);
    }

    public enum Type
    {
        NORMAL,
        QUEST,
        COUNT
    }

    public enum Replace
    {
        POKE,
        MOVE,
        ITEM,
        INT
    }

    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID); }
}
