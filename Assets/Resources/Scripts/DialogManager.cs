using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    Dictionary<int, string[]> msgDictionary;
    RectTransform dialogTransform;
    TMP_Text textMesh;
    bool isActive = false;
    float posY = 0;
    float posTime = 0f;

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

    private bool questIsQuest = false;
    private int questYesEventID = 0;
    private int questNoEventID = 0;
    private float questCursorPos = 0f;
    private GameObject questObject;
    private GameObject questCursor;
    private int questCursorNum = 0;
    private float questCursorSpeed = 0.1f;
    private float questCursorInputStun = 0f;

    private GlobalInput input;
    private UIManager uiManager;

    private void Awake()
    {
        instance = this;
        dialogTransform = GetComponent<RectTransform>();
        textMesh = transform.GetChild(0).GetComponent<TMP_Text>();
        dialogCursor = transform.Find("Cursor").gameObject;
        questObject = transform.Find("Quest").gameObject;
        questCursor = questObject.transform.Find("QuestCursor").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {        
        Init();        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUi();
        DialogUpdate();

        if (isActive && input.aButtonDown)
        {
            PushAButton();
        }

        if (questIsQuest)
        {
            QuestInputUpdate();
            QuestCursorUpdate();
        }
    }



    private void PushAButton()
    {
            
        if (dialogStun < 0)
        {
            dialogStun = 0.1f;
            if (dialogMsgDone)
            {
                if (!(questIsQuest))
                {
                    if (!GoNextPage())
                    {
                        UnActive();
                    }
                }
                else
                {
                    //선택
                    UnActive();

                    if (questCursorNum == 0)
                    {
                        EventManager.instance.StartEvent(questYesEventID);
                    }
                    else
                    {
                        EventManager.instance.StartEvent(questNoEventID);
                    }                    
                }
                
            }
            else
            {
                /* 빠른 스킵
                dialogMsgCheck = dialogMsg.Length;
                dialogMsgDone = true;
                dialogCursor.SetActive(true);
                ShowMsg(dialogMsg);
                */
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

                    if (!questIsQuest)
                    {
                        dialogCursor.SetActive(true);
                    }
                    else
                    {
                        questObject.SetActive(true);
                    }
                }
                
            }

            dialogStun -= Time.deltaTime;
        }
    }

    public void Active(int msgId, params int[] args)
    {
        posTime = 0f;
        isActive = true;
        SetMsgById(msgId);

        uiManager.SetUIActive(true);
        

        if (args.Length > 0)
        {
            if (args[0] == 0)
            {
                QuestCursorInit(args[1], args[2]);
            }
            if (args[0] == 1)
            {

            }
        }
        else
        {
            questIsQuest = false;
        }
    }

    private void UnActive()
    {
        posTime = 0.5f;
        isActive = false;

        uiManager.SetUIActive(false);
        questObject.SetActive(false);

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
        dialogMsg = msgDictionary[dialogMsgId][dialogMsgPage];
        dialogMsgCheck = 0f;
        dialogPreMsgCheck = 0;
        dialogMsgDone = false;
        dialogCursor.SetActive(false);
        dialogStun = 0.1f;
    }

    private void ShowMsg(string msg)
    {
        textMesh.text = msg;
    }

    private void Init()
    {
        msgDictionary = new Dictionary<int, string[]>();
        SetDialog();

        input = GlobalInput.globalInput;
        uiManager = UIManager.instance;

        questObject.SetActive(false);
    }

    private void UpdateUi()
    {
        if (isActive)
        {
            if (posY < 200)
            {
                posTime += Time.deltaTime*2;
                posY = -800 * posTime * posTime + 800 * posTime;
                if (posY >199f) 
                {
                    posY = 200f;
                }
                SetUIPos();
            }
        }
        else
        {
            if (posY > 0)
            {
                posTime += Time.deltaTime * 2;
                posY = -800 * posTime * posTime + 800 * posTime;
                if (posY < 2)
                {
                    posY = 0f;
                }
                SetUIPos();
            }
        }
    }

    private void QuestInputUpdate()
    {
        if (dialogMsgDone)
        {
            questCursorInputStun -= Time.deltaTime;
            if (questCursorInputStun < 0f)
            {
                if (input.verticalRaw != 0)
                {
                    if (questCursorNum == 0 && questCursorPos == 0)
                    {
                        questCursorNum = 1;
                        questCursorInputStun = questCursorSpeed + 0.2f;
                    }

                    if (questCursorNum == 1 && questCursorPos == questCursorSpeed)
                    {
                        questCursorNum = 0;
                        questCursorInputStun = questCursorSpeed + 0.2f;
                    }
                }
            }
            
        }
    }

    private void QuestCursorInit(int yesID, int noID)
    {
        questIsQuest = true;
        questYesEventID = yesID;
        questNoEventID = noID;
        questCursorPos = 0f;
        questCursorNum = 0;

        var cursorYPos = (float)Math.Cos(questCursorPos * (Math.PI) / questCursorSpeed);
        var rTrans = (RectTransform)questCursor.transform;
        rTrans.anchoredPosition = new Vector2(-59f, -2.5f + 17.5f * cursorYPos);
    }

    private void QuestCursorUpdate()
    {
        if (dialogMsgDone)
        {
            if (questCursorNum == 0 && questCursorPos > 0)
            {
                questCursorPos = Math.Max(0, questCursorPos - Time.deltaTime);
                var cursorYPos = (float)Math.Cos(questCursorPos * (Math.PI) / questCursorSpeed);
                var rTrans = (RectTransform)questCursor.transform;
                rTrans.anchoredPosition = new Vector2(-59f, -2.5f + 17.5f*cursorYPos);
            }
            if (questCursorNum == 1 && questCursorPos < questCursorSpeed)
            {
                questCursorPos = Math.Min(questCursorSpeed, questCursorPos + Time.deltaTime);
                var cursorYPos = (float)Math.Cos(questCursorPos * (Math.PI) / questCursorSpeed);
                var rTrans = (RectTransform)questCursor.transform;
                rTrans.anchoredPosition = new Vector2(-59f, -2.5f + 17.5f * cursorYPos);
            }
        }
    }

    private void SetUIPos()
    {
        dialogTransform.anchoredPosition = new Vector3(0, posY - 180f, 0);
    }

    private void SetDialog()//npcID__대화ID
    {
        msgDictionary.Add(0, new string[] { "안녕하세요2", "메세지창 테스트", "이곳은 포켓몬 세계\n많은 포켓몬들이 살고 있다." });

        //NPC1_엄마
        msgDictionary.Add(1001, new string[] { "아 도희!\n옆집의 오박사님이 찾아왔었단다.", "뭔지 너에게\n부탁할 것이 있다고 하셔서", "연구소는 집 왼쪽에 있으니 어서 찾아가 보렴"});
        msgDictionary.Add(1002, new string[] { "연구소는 집 왼쪽에 있단다." });

        //NPC2_오박사
        msgDictionary.Add(2001, new string[] { "연구소에 잘 왔다!\n나는 오박사 포켓몬 박사란다.", "오늘부터 모험을 시작하게 된것을 축하한단다."});
        msgDictionary.Add(2002, new string[] { "여기 너에게 줄 포켓몬이 있단다 신중하게 고르렴" });
        msgDictionary.Add(2003, new string[] { "여행을 떠나려면 포켓몬이 있어야 한단다." });
        msgDictionary.Add(2004, new string[] { "너의 첫 포켓몬이니 신중하게 고르렴", "천천히 골라도 괜찮단다." });
        msgDictionary.Add(2005, new string[] { "풀 포켓몬 이상해씨란다. 그 포켓몬으로 결정하겠니?" });
        msgDictionary.Add(2006, new string[] { "불 포켓몬 파이리란다. 그 포켓몬으로 결정하겠니?" });
        msgDictionary.Add(2007, new string[] { "물 포켓몬 꼬부기란다. 그 포켓몬으로 결정하겠니?" });
        msgDictionary.Add(2008, new string[] { "여행 응원한단다.\n멋진 포켓몬 트레이너가 되거라" });

        //NPC3_크리스
        msgDictionary.Add(3001, new string[] { "안녕 난 오박사님의 조수 크리스야","앞으로 잘 부탁해!"});
        msgDictionary.Add(3002, new string[] { "난 그럼 이 포켓몬으로 할게!" });
        msgDictionary.Add(3003, new string[] { "서로 포켓몬을 얻었으니 바로 배틀이지!" });
        msgDictionary.Add(3004, new string[] { "즐거운 배틀이었어!\n다음에 또 보자!" });

        //NPC18_마을여자
        msgDictionary.Add(18001, new string[] { "도희야 좋은 아침", "포켓몬 없이 풀숲에 들어가는 것은 위험하단다."});
        msgDictionary.Add(18002, new string[] { "어! 도희야", "혼자서 어디 가니?","이런! 포켓몬도 지니지 않고 도로에 나가다니 위험해!","근처의 마을까지는 야생의 포켓몬이 튀어 나오는 풀숲이 있으니까"});

        //NPC19_뚱보
        msgDictionary.Add(19001, new string[] { "어이! 도희", "오박사님이 새로운 포켓몬을 발견하셨대" });


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
    }
}
