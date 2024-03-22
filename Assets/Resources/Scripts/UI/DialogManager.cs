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
    private int uiID = 9001;

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
                if (!GoNextPage())
                {
                    UnActive();
                    if ((questIsQuest))
                    {//����

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
                
            }
            else
            {
                /* ���� ��ŵ
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

                    if (dialogMsgPage < dialogMsgMaxPage)
                    {
                        dialogCursor.SetActive(true);
                    }
                    else
                    {
                        if (questIsQuest)
                        {
                            questObject.SetActive(true);
                        }
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

        uiManager.ActiveUI(uiID);
        

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

        uiManager.UnActiveUI();
        questObject.SetActive(false);

        if (!(questIsQuest))
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
                if (input.verticalRaw == -1 && questCursorNum == 0 && questCursorPos == 0)
                {
                    questCursorNum = 1;
                    questCursorInputStun = questCursorSpeed + 0.2f;
                }

                if (input.verticalRaw == 1 && questCursorNum == 1 && questCursorPos == questCursorSpeed)
                {
                    questCursorNum = 0;
                    questCursorInputStun = questCursorSpeed + 0.2f;
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

    private void SetDialog()//npcID__��ȭID
    {
        msgDictionary.Add(0, new string[] { "�ȳ��ϼ���2", "�޼���â �׽�Ʈ", "�̰��� ���ϸ� ����\n���� ���ϸ���� ��� �ִ�." });

        //NPC1_����
        msgDictionary.Add(1001, new string[] { "�� ����!\n������ ���ڻ���� ã�ƿԾ��ܴ�.", "���� �ʿ���\n��Ź�� ���� �ִٰ� �ϼż�", "�����Ҵ� �� ���ʿ� ������ � ã�ư� ����"});
        msgDictionary.Add(1002, new string[] { "�����Ҵ� �� ���ʿ� �ִܴ�." });
        msgDictionary.Add(1003, new string[] { "��� �Ϳ��� ���ϸ��̳�\n���� �� �� ���� �Ϸ�" });

        //NPC2_���ڻ�
        msgDictionary.Add(2001, new string[] { "�����ҿ� �� �Դ�!\n���� ���ڻ� ���ϸ� �ڻ����.", "���ú��� ������ �����ϰ� �Ȱ��� �����Ѵܴ�."});
        msgDictionary.Add(2002, new string[] { "���� �ʿ��� �� ���ϸ��� �ִܴ� �����ϰ� ������" });
        msgDictionary.Add(2003, new string[] { "������ �������� ���ϸ��� �־�� �Ѵܴ�." });
        msgDictionary.Add(2004, new string[] { "���� ù ���ϸ��̴� �����ϰ� ������", "õõ�� ��� �����ܴ�." });
        msgDictionary.Add(2005, new string[] { "Ǯ ���ϸ� �̻��ؾ�����. �� ���ϸ����� �����ϰڴ�?" });
        msgDictionary.Add(2006, new string[] { "�� ���ϸ� ���̸�����. �� ���ϸ����� �����ϰڴ�?" });
        msgDictionary.Add(2007, new string[] { "�� ���ϸ� ���α����. �� ���ϸ����� �����ϰڴ�?" });
        msgDictionary.Add(2008, new string[] { "���� �����Ѵܴ�.\n���� ���ϸ� Ʈ���̳ʰ� �ǰŶ�" });

        //NPC3_ũ����
        msgDictionary.Add(3001, new string[] { "�ȳ� �� ���ڻ���� ���� ũ������","������ �� ��Ź��!"});
        msgDictionary.Add(3002, new string[] { "�� �׷� �� ���ϸ����� �Ұ�!" });
        msgDictionary.Add(3003, new string[] { "���� ���ϸ��� ������� �ٷ� ��Ʋ����!" });
        msgDictionary.Add(3004, new string[] { "��ſ� ��Ʋ�̾���!\n������ �� ����!" });

        //NPC4_������
        msgDictionary.Add(4001, new string[] { "��- �ٻ���" });

        //NPC5_������
        msgDictionary.Add(5001, new string[] { "���õ� �߱��̾�" });
        msgDictionary.Add(5002, new string[] { "����!","���� ���ͺ��̾�\n���ϸ��� �������� ���ͺ��� �ʿ���" });

        //NPC6_������
        msgDictionary.Add(6001, new string[] { "���⼱ ���ϸ��� ġ���� �� �־�" });

        //NPC7_������
        msgDictionary.Add(7001, new string[] { "����� �ַ� ���� Ÿ���� ���ϸ��� �����","���� ��밡 �ƴϴ� �����Ϸ�!" });

        //npc8
        msgDictionary.Add(8001, new string[] { "ü���� ���� ����� �ʹ� ����","�� ���ϸ���� �ƹ��͵� ���ϰ� ��� ���ع��Ⱦ�" });

        //npc9
        msgDictionary.Add(9001, new string[] { "ü���� Ʈ���̳�2" });
        msgDictionary.Add(9002, new string[] { "ü���� Ʈ���̳�2 ��������" });

        //npc10
        msgDictionary.Add(10001, new string[] { "ü���� Ʈ���̳�1 ����" });
        msgDictionary.Add(10002, new string[] { "ü���� Ʈ���̳�1 ��������" });

        //npc11
        msgDictionary.Add(11001, new string[] { "Ǯ������ ���ϸ��� �����ִ�\n���� Ƣ����� ����" });

        //npc12
        msgDictionary.Add(12001, new string[] { "���ϸ� Ʈ���̳ʷν��� �Ƿ��� Ȯ���Ϸ��°�?","�׷��ٸ� ������ �ִ� ���ϸ� ü�������� ������ ������ ���� ����" });

        //npc13
        msgDictionary.Add(13001, new string[] { "��Ÿ� Ʈ���̳�" });
        msgDictionary.Add(13002, new string[] { "��Ÿ� Ʈ���̳� ��������" });

        //npc14
        msgDictionary.Add(14001, new string[] { "�� ���ϸ��� ��� �������� ġ���Ϸ� �Ծ�" });

        //npc15
        msgDictionary.Add(15001, new string[] { "���� �ű��� �������� ���� ����!" });

        //npc16
        msgDictionary.Add(16001, new string[] { "�� ��ī�� �Ϳ���?","�� ���󿡼� ��ī�� ���� ����!" });

        //npc17
        msgDictionary.Add(17001, new string[] { "�ű⿡ ����� ����?","�پ� ������ �� �������� Ǯ���� �ɾ�� �ʰ� ������ �� �� ����!" });

        //NPC18_��������
        msgDictionary.Add(18001, new string[] { "����� ���� ��ħ", "���ϸ� ���� Ǯ���� ���� ���� �����ϴܴ�."});
        msgDictionary.Add(18002, new string[] { "��! �����", "ȥ�ڼ� ��� ����?","�̷�! ���ϸ� ������ �ʰ� ���ο� �����ٴ� ������!","��ó�� ���������� �߻��� ���ϸ��� Ƣ�� ������ Ǯ���� �����ϱ�"});

        //NPC19_�׺�
        msgDictionary.Add(19001, new string[] { "����! ����", "���ڻ���� ���ο� ���ϸ��� �߰��ϼ̴�" });

        //NPC20
        msgDictionary.Add(20001, new string[] { "���� ���ִ°� Ȥ�� ������?" });

        //NPC21
        msgDictionary.Add(21001, new string[] { "���⼭ �Ĵ� ���� ������Դ� ȿ���� ������" });

        //NPC22
        msgDictionary.Add(22001, new string[] { "���� ������ ���� ���� Ʈ���̳ʿ��ܴ�!" ,"�׷� ���κ��� �����̽�!\n���ϸ��� ���� ��ƶ�!","�׸��� ��ȭ���� ���ϰ� ���ϸ��� �����ִ� ���̴�!"});

        //npc23 ��ȣ��
        msgDictionary.Add(23001, new string[] { "�ȳ��ϼ���!\n���ϸ� �����Դϴ�!", "�̰������� ���ϸ��� ü���� ȸ���մϴ�!", "����� ���ϸ��� ���� �ϰڽ��ϱ�?" });
        msgDictionary.Add(23002, new string[] { "���� ������ �湮�Ͻñ� ��ٸ��ڽ��ϴ�!" });
        msgDictionary.Add(23003, new string[] { "���� ��ٸ��̽��ϴ�!","�þ� ������ ���ϸ��� ��� �ǰ��������ϴ�!"});
        msgDictionary.Add(23004, new string[] { "�ȳ��ϼ���!\n���ϸ� �����Դϴ�!", "�̰������� ���ϸ��� ü���� ȸ���մϴ�!", "�ƽ����� ���ϸ��� ������ �ü��� �̿��Ͻ� �� �����ϴ�." });
        
        //npc24 ��ȣ��
        msgDictionary.Add(24001, new string[] { "��ȣ�� �׽�Ʈ" });

        //npc25 
        msgDictionary.Add(25001, new string[] { "�������� �׽�Ʈ" });

        //npc26
        msgDictionary.Add(26001, new string[] { "�������� �׽�Ʈ" });

        //npc27
        msgDictionary.Add(27001, new string[] { "Ǯ���� �������ٰ� �������ϸ󿡰� ���� �����!","�״�� �ɾ�ٰ� ���ϸ��� ����������!","�ص����� ������ �������� ����" });

        //npc28
        msgDictionary.Add(28001, new string[] { "�� ���� �ִ� ��ǻ�͸� ���� ���ϸ��� �ñ�� ������ �� �ִܴ�!" });

        //npc29
        msgDictionary.Add(29001, new string[] { "��ī��ī!" });

        //npc30~31
        msgDictionary.Add(30001, new string[] { "ü���� ������ ������ �̰��� ����Ͻ� �� �����ϴ�" });
        msgDictionary.Add(31001, new string[] { "ü���� ������ ������ �̰��� ����Ͻ� �� �����ϴ�" });

        //npc32 ����
        msgDictionary.Add(32001, new string[] { "���� �׽�Ʈ" });

        //npc33
        msgDictionary.Add(33001, new string[] { "��ī��ī!" });

        //������Ʈ��
        msgDictionary.Add(99001, new string[] { "��ī�������̴�!" });
        msgDictionary.Add(99002, new string[] { "Ǫ�������̴�!" });
        msgDictionary.Add(99003, new string[] { "������!" });
        msgDictionary.Add(99004, new string[] { "TV���� ��ȭ�� �ϰ� �ִ�. ���ھ��� ���̼� ������ �ϰ� �ִ�","������ â�ʸӷ� ���� ���δ�.....","....���� ���� ��������!" });
        msgDictionary.Add(99005, new string[] { "���ϸ� �׸�å�� ���ֱ�!" });
        msgDictionary.Add(99006, new string[] { "�� �濡 �ִ� ������!\n��Ӵϰ� ���� ���� �ֽŴ�." });
        msgDictionary.Add(99007, new string[] { "���������� ������" });
        msgDictionary.Add(99008, new string[] { "����� �ȿ���.....","���ִ� ���� ����\n�׸��� ������ �ĸ��� ��ũ!" });
        msgDictionary.Add(99009, new string[] { "��¦��¦�Ÿ��� ��ũ��!" });
        msgDictionary.Add(99010, new string[] { "������ ���ִ� ���� �ϰ� �ִ�!" });

        msgDictionary.Add(99011, new string[] { "���ڻ���� ��ǻ�ʹ�!","........","ȭ���� ������ ���� ����...." });
        msgDictionary.Add(99012, new string[] { "å�忡�� ���ϸ� ���� ���� �������� �����ִ�." });
        msgDictionary.Add(99013, new string[] { "��� �ִ� ���������̴�." });

        msgDictionary.Add(99014, new string[] { "�츮���̴�!" });
        msgDictionary.Add(99015, new string[] { "���ڻ�� �����Ҵ�!" });
        msgDictionary.Add(99016, new string[] { "���ϸ� ���ʹ�!","���⼭ ���ϸ��� ġ���� �� �ִ�."});
        msgDictionary.Add(99017, new string[] { "���ϸ� ���̴�!", "���⼭ ���ϸ���� �������� ������ �� �ִ�." });
        msgDictionary.Add(99018, new string[] { "���θ���" });
        msgDictionary.Add(99019, new string[] { "������" });
        msgDictionary.Add(99020, new string[] { "���ϸ� ü�����̴�!","������ �����߸��� ������ ���� �� �ִ�." });

        msgDictionary.Add(99021, new string[] { "���� ���ǵ��� �����Ǿ� �ִ�." });
        msgDictionary.Add(99022, new string[] { "1�� ����" });

        msgDictionary.Add(99023, new string[] { "{poke}(��)�� �����." });
        msgDictionary.Add(99024, new string[] { "{item}(��)�� �����." });

        msgDictionary.Add(99025, new string[] { "���� ���ͺ��̴�.\n���ڻ���� �� Ű�� �ֽð���?" });

        msgDictionary.Add(99026, new string[] { "���ϸ��� ȸ����Ű�ڽ��ϱ�?" });

        msgDictionary.Add(99027, new string[] { "�ش� �������� ���� ����Ͻ� �� �����ϴ�." });
    }
}