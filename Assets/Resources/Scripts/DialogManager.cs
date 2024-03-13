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

    private void SetDialog()//npcID__��ȭID
    {
        msgDictionary.Add(0, new string[] { "�ȳ��ϼ���2", "�޼���â �׽�Ʈ", "�̰��� ���ϸ� ����\n���� ���ϸ���� ��� �ִ�." });

        //NPC1_����
        msgDictionary.Add(1001, new string[] { "�� ����!\n������ ���ڻ���� ã�ƿԾ��ܴ�.", "���� �ʿ���\n��Ź�� ���� �ִٰ� �ϼż�", "�����Ҵ� �� ���ʿ� ������ � ã�ư� ����"});
        msgDictionary.Add(1002, new string[] { "�����Ҵ� �� ���ʿ� �ִܴ�." });

        //NPC2_���ڻ�
        msgDictionary.Add(2001, new string[] { "�����ҿ� �� �Դ�!\n���� ���ڻ� ���ϸ� �ڻ����.", "���ú��� ������ �����ϰ� �Ȱ��� �����Ѵܴ�."});
        msgDictionary.Add(2002, new string[] { "���� �ʿ��� �� ���ϸ��� �ִܴ� �����ϰ� ����" });
        msgDictionary.Add(2003, new string[] { "������ �������� ���ϸ��� �־�� �Ѵܴ�." });
        msgDictionary.Add(2004, new string[] { "���� ù ���ϸ��̴� �����ϰ� ����", "õõ�� ��� �����ܴ�." });

        //NPC3_ũ����
        msgDictionary.Add(3001, new string[] { "�ȳ� �� ���ڻ���� ���� ũ������ �� ��Ź��"});

        //NPC18_��������
        msgDictionary.Add(18001, new string[] { "����� ���� ��ħ", "���ϸ� ���� Ǯ���� ���� ���� �����ϴܴ�."});
        msgDictionary.Add(18002, new string[] { "��! �����", "ȥ�ڼ� ��� ����?","�̷�! ���ϸ� ������ �ʰ� ���ο� �����ٴ� ������!","��ó�� ���������� �߻��� ���ϸ��� Ƣ�� ������ Ǯ���� �����ϱ�"});

        //NPC19_�׺�
        msgDictionary.Add(19001, new string[] { "����! ����", "���ڻ���� ���ο� ���ϸ��� �߰��ϼ̴�" });


        //������Ʈ��
        msgDictionary.Add(99001, new string[] { "��ī�������̴�" });
        msgDictionary.Add(99002, new string[] { "Ǫ�������̴�" });
        msgDictionary.Add(99003, new string[] { "��ī�������̴�" });
    }
}
