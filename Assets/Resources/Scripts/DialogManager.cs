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

    private GameObject dialogCursor;

    private GlobalInput input;

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
        init();
        
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
            Player.player.inputStun = 0.2f;
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
        }
    }

    public void Active(int msgId)
    {
        posTime = 0f;
        isActive = true;

        SetMsgById(msgId);
    }

    private void UnActive()
    {
        posTime = 0.5f;
        isActive = false;
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
    }

    private void ShowMsg(string msg)
    {
        textMesh.text = msg;
    }

    private void init()
    {
        msgDictionary = new Dictionary<int, string[]>();
        SetDialog();

        input = GlobalInput.globalInput;
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

    private void SetDialog()
    {
        msgDictionary.Add(0, new string[] { "안녕하세요2", "메세지창 테스트", "이곳은 포켓몬 세계\n많은 포켓몬들이 살고 있다." });
        msgDictionary.Add(1, new string[] { "아 도희!\n옆집의 오박사님이 찾아왔었단다", "뭔지 너에게\n부탁할 것이 있다고 하셔서", "연구소는 집 왼쪽에 있으니 어서 찾아가 보렴" });
        
    }
}
