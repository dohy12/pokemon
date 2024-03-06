using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class DialogManager : MonoBehaviour
{
    public static DialogManager manger;
    Dictionary<int, string[]> msgDictionary;
    RectTransform dialogTransform;
    TMP_Text textMesh;
    int page = 0;
    bool isActive = false;
    float posY = 0;
    float posTime = 0f;

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
        ShowMsg(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUi();

        if (input.aButtonDown)
        {
            if (!isActive)
            {
                Active();
            }
            else
            {
                UnActive();
            }
        }
    }

    private void Active()
    {
        posTime = 0f;
        isActive = true;
    }

    private void UnActive()
    {
        posTime = 0.5f;
        isActive = false;
    }

    public void ShowMsg(int msgId)
    {
        page = 0;
        SetMsg(msgId, page);
    }

    private void SetMsg(int msgId, int page)
    {
        textMesh.text = msgDictionary[msgId][page];
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
