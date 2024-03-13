using System;
using System.Collections;
using System.Collections.Generic;
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
    private float dialogMsgSpeed = 20f;
    private bool dialogMsgDone = false;
    private float dialogStun = 0f;

    private GameObject dialogCursor;

    private GlobalInput input;
    private UIManager uiManager;

    private void Awake()
    {
        instance = this;
        dialogTransform = GetComponent<RectTransform>();
        textMesh = transform.GetChild(0).GetComponent<TMP_Text>();
        dialogCursor = transform.Find("Cursor").gameObject;
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
    }

    private void PushAButton()
    {
        
        if (isActive)
        {            
            if (dialogStun < 0)
            {
                dialogStun = 0.1f;
                if (dialogMsgDone)
                {
                    if (!GoNextPage())
                    {
                        UnActive();
                    }
                }
                else
                {
                    dialogMsgCheck = dialogMsg.Length;
                    dialogMsgDone = true;
                    dialogCursor.SetActive(true);
                    ShowMsg(dialogMsg);
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
                dialogMsgDone = true;
                dialogCursor.SetActive(true);
            }

            dialogStun -= Time.deltaTime;
        }
    }

    public void Active(int msgId)
    {
        posTime = 0f;
        isActive = true;

        SetMsgById(msgId);

        uiManager.SetUIActive(true);
    }

    private void UnActive()
    {
        posTime = 0.5f;
        isActive = false;

        uiManager.SetUIActive(false);

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

        //NPC3_크리스
        msgDictionary.Add(3001, new string[] { "안녕 난 오박사님의 조수 크리스야 잘 부탁해"});

        //NPC18_마을여자
        msgDictionary.Add(18001, new string[] { "도희야 좋은 아침", "포켓몬 없이 풀숲에 들어가는 것은 위험하단다."});
        msgDictionary.Add(18002, new string[] { "어! 도희야", "혼자서 어디 가니?","이런! 포켓몬도 지니지 않고 도로에 나가다니 위험해!","근처의 마을까지는 야생의 포켓몬이 튀어 나오는 풀숲이 있으니까"});

        //NPC19_뚱보
        msgDictionary.Add(19001, new string[] { "어이! 도희", "오박사님이 새로운 포켓몬을 발견하셨대" });


        //오브젝트들
        msgDictionary.Add(99001, new string[] { "피카츄인형이다" });
        msgDictionary.Add(99002, new string[] { "푸린인형이다" });
        msgDictionary.Add(99003, new string[] { "피카츄인형이다" });
    }
}
