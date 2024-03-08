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
    public static DialogManager manger;
    Dictionary<int, string[]> msgDictionary;
    RectTransform dialogTransform;
    TMP_Text textMesh;
    bool isActive = false;
    float posY = 0;
    float posTime = 0f;
    
    int dialogMsgId = 0;
    int dialogMsgPage = 0;
    int dialogMsgMaxPage = 0;
    string dialogMsg = "";
    float dialogMsgCheck = 0f;
    int dialogPreMsgCheck = 0;
    float dialogMsgSpeed = 13f;
    bool dialogMsgDone = false;

    private GlobalInput input;

    private void Awake()
    {
        manger = GetComponent<DialogManager>();
        dialogTransform = GetComponent<RectTransform>();
        textMesh = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {        
        init();
        SetMsgById(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUi();
        DialogUpdate();

        if (input.aButtonDown)
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
                ShowMsg(dialogMsg);
            }
        }
        else
        {
            Active(0);
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
    }

    private void ShowMsg(string msg)
    {
        textMesh.text = msg;
    }

    private void init()
    {
        msgDictionary = new Dictionary<int, string[]>();
        msgDictionary.Add(0, new string[] { "안녕하세요2", "메세지창 테스트", "이곳은 포켓몬 세계\n많은 포켓몬들이 살고 있다." });

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
}
