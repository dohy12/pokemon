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


    private void SetDialog()//npcID__��ȭID
    {
        msgDictionary.Add(0, new string[] { "�ȳ��ϼ���2", "�޼���â �׽�Ʈ", "�̰��� ���ϸ� ����\n���� ���ϸ���� ��� �ִ�." });

        //NPC1_����
        msgDictionary.Add(1001, new string[] { "�� ����!\n������ ���ڻ���� ã�ƿԾ��ܴ�.", "���� �ʿ���\n��Ź�� ���� �ִٰ� �ϼż�", "�����Ҵ� �� ���ʿ� ������\n� ã�ư� ����"});
        msgDictionary.Add(1002, new string[] { "�����Ҵ� �� ���ʿ� �ִܴ�." });
        msgDictionary.Add(1003, new string[] { "��� �Ϳ��� ���ϸ��̳�\n���� �� �� ���� �Ϸ�" });

        //NPC2_���ڻ�
        msgDictionary.Add(2001, new string[] { "�����ҿ� �� �Դ�!\n���� ���ڻ� ���ϸ� �ڻ����.", "���ú��� ������ �����ϰ� �Ȱ��� �����Ѵܴ�."});
        msgDictionary.Add(2002, new string[] { "���� �ʿ��� �� ���ϸ��� �ִܴ� �����ϰ� ����" });
        msgDictionary.Add(2003, new string[] { "������ �������� ���ϸ��� �־�� �Ѵܴ�." });
        msgDictionary.Add(2004, new string[] { "���� ù ���ϸ��̴� �����ϰ� ���Ŷ�\nõõ�� ��� �����ܴ�" });
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

        //npc25 ��������
        msgDictionary.Add(25001, new string[] { "�ȳ��ϼ��� �����鸮 ���Դϴ�\n������ ���͵帱���?" }); 
        msgDictionary.Add(25002, new string[] { "�����մϴ�\n�������� �� ���ּ���!" });

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

        //npc33
        msgDictionary.Add(35001, new string[] { "npc ���� �׽�Ʈ1" });
        msgDictionary.Add(35002, new string[] { "npc ���� �׽�Ʈ ����" });

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
        msgDictionary.Add(99028, new string[] { "�߿� �������� ���� �� �����ϴ�." });
        msgDictionary.Add(99029, new string[] { "�� �� �����ðڽ��ϱ�?" });
        msgDictionary.Add(99030, new string[] { "�� �� �Ǹ� �Ͻðڽ��ϱ�?" });
        msgDictionary.Add(99031, new string[] { "�߿� �������� �Ǹ� �Ͻ� �� �����ϴ�." });
        msgDictionary.Add(99032, new string[] { "���� �������� �����ðڽ��ϱ�?" });
        msgDictionary.Add(99033, new string[] { "���� �Ǹ��Ͻðڽ��ϱ�?" });
        msgDictionary.Add(99034, new string[] { "�� �� ���� �Ͻðڽ��ϱ�?" });
        msgDictionary.Add(99035, new string[] { "���� �����Ͻðڽ��ϱ�?" });
        msgDictionary.Add(99036, new string[] { "�˼��մϴ�. ���� �����մϴ�" });
        msgDictionary.Add(99037, new string[] { "�����մϴ�!" });
        msgDictionary.Add(99038, new string[] { "���� ������ �ִ� ���ϸ��� �����ϴ�." });
        msgDictionary.Add(99039, new string[] { "Ʈ���̳ʿ� ��Ʋ �߿��� ����ĥ �� ����!" });

        msgDictionary.Add(99100, new string[] { "{poke}�� ������ ������ ��� �Ǿ����ϴ�." });

        msgDictionary.Add(99101, new string[] { "{poke}�� {move}!!" });

        msgDictionary.Add(99102, new string[] { "ȿ���� �����ߴ�!!" });
        msgDictionary.Add(99103, new string[] { "ȿ���� �����ε� �ϴ�...." }); 
        msgDictionary.Add(99104, new string[] { "�׷��� ȿ���� ������...." });
        msgDictionary.Add(99105, new string[] { "{poke}�� ��������." });
        msgDictionary.Add(99106, new string[] { "{poke}�� {INT.}�� ����ġ�� ȹ���ߴ�." });
        
        msgDictionary.Add(99107, new string[] { "�������� �¸��Ͽ����ϴ�." });
        msgDictionary.Add(99108, new string[] { "�������� �й��Ͽ����ϴ�." });

        msgDictionary.Add(99109, new string[] { "�߻��� {poke}(��)�� ��Ÿ����!" });
        msgDictionary.Add(99110, new string[] { "~~(��)�� �ºθ� �ɾ�Դ�" });
        msgDictionary.Add(99111, new string[] { "~~(��)�� {poke}(��)�� �����´�!" });
        msgDictionary.Add(99112, new string[] { "���� {poke}!!!" });
        msgDictionary.Add(99113, new string[] { "{poke} �ʷ� ���ߴ�!!!" });
        msgDictionary.Add(99114, new string[] { "{poke} ����!!!!" });

        msgDictionary.Add(99115, new string[] { "{poke}(��)�� ������ ����� ���µ� �ϴ�...." });
        msgDictionary.Add(99116, new string[] { "{poke}(��)�� �̹� ���� �ִ�!" });

        msgDictionary.Add(99999, new string[] { "�׽�Ʈ�� �޼��� �Դϴ�." });


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
        select.Active(cursorMaxNum, "��\n�ƴϿ�", this, 200f, pos, dialogArgs);
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
