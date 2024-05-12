using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Bag : SlideUI , CursorUI, SelectUIRedirec
{
    public static Bag instance;
    private GlobalInput input;
    private GameObject uiID;
    private Cursor cursor;
    public int page = 0;

    private ItemInfo info;
    private Money money;

    private int[] showItmList;
    private TMP_Text[] stringObj_Txt;
    private TMP_Text[] stringObj_Num;

    private bool isShop = false;

    private GameDataManager data;

    private int uiType = 0; //[0] 필드에서, [1] 샵, [2] 전투 중

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SlideUiInit();  
        Init();
    }


    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
        InputCheck();
    }


    private void Init()
    {
        uiID = gameObject;
        input = GlobalInput.globalInput;
        info = ItemInfo.instance;

        showItmList = new int[5] {-1,-1,-1,-1,-1};
        stringObj_Txt = new TMP_Text[5];
        stringObj_Num = new TMP_Text[5];
        for (var i = 0; i < 5; i++)
        {
            stringObj_Txt[i] = transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            stringObj_Num[i] = transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>();
        }        

        money = transform.parent.Find("Money").GetComponent<Money>();
        data = GameDataManager.instance;
    }

    private void ShowAllKeys()
    {
        var itmList = data.items.Keys.ToList();
        var str = "모든 키("+itmList.Count+") : ";

        foreach(var itm in itmList)
        {
            str = str + info.info[itm].name + " ";
        }
        Debug.Log(str);
    }


    private void SetString(params bool[] args)
    {
        var num = page;
        var itmsList = data.items.Keys.ToList();
        var isChecked = false;

        cursor.cursorMaxNum = 4;
        for (var i=0; i<5; i++)
        {
            if (num + i < itmsList.Count)
            {
                var itm = info.info[itmsList[num + i]];
                stringObj_Txt[i].text = itm.name;
                if (itm.type != ItemInfo.Type.IMPOTANT)
                {
                    stringObj_Num[i].text = "X "+(data.items[itm.id]).ToString("D2");
                }
                else
                {
                    stringObj_Num[i].text = "";
                }                
            }
            else
            {
                if(!isChecked)
                {
                    cursor.cursorMaxNum = i;
                    if (args.Length>0 && args[0])
                        cursor.cursorNum = i;
                    isChecked = true;
                    stringObj_Txt[i].text = "그만두다";
                }
                else
                {
                    stringObj_Txt[i].text = "";
                }
                stringObj_Num[i].text = "";
            }
        }
    }


    public void Active(int uiType)
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();

        page = 0;
        SetString();

        ActiveMoney();
        isShop = false;


        this.uiType = uiType;

        if (uiType == 1)
        {
            isShop = true;
        }
    }

    public void UnActive()
    {
        input.InputStun();
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);

        UnActiveMoney();
    }

    private void ActiveMoney()
    {
        money.Active();
    }

    private void UnActiveMoney()
    {
        if (!isShop)
        {
            money.UnActive();
        }
    }

    private void InputCheck()
    {
        if (GetActive())
        {
            if (input.bButtonDown)
            {
                UnActive();
            }
        }
    }

    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID); }

    public void CursorChange(int pageTmp) 
    {        
        if (pageTmp != 0)
        {
            page += pageTmp;

            if (page < 0)
            {
                page = 0;
            }

            if (page > data.items.Keys.Count - 2)
            {
                page = data.items.Keys.Count - 2;
            }

            SetString((pageTmp == 1));
        }
    }

    public void CursorChoose(int num)
    {
        if (data.items.Keys.Count <= num + page)
        {
            UnActive();
        }
        else
        {
            if (!isShop)
            {
                SelectUIActive(num + page);
            }
            else
            {
                int itmID = data.items.Keys.ToList()[num + page];
                if (info.info[itmID].type != ItemInfo.Type.IMPOTANT)
                {
                    ShopUIActive(itmID);
                }
                else
                {
                    DialogManager.instance.Active(99031, null, DialogManager.Type.NORMAL);
                }
                
            }
        }
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 5 - 1;
        float yDist = 60.2f;
        cursor.Init(cursorMaxNum, yDist, false);
    }

    public void OnSelectRedirec(int selectNum, params int[] args)
    {
        if (args[0] == 0)
        {
            var itmsList = data.items.Keys.ToList();
            var itmID = args[1];
            var itm = info.info[itmID];

            if (selectNum == 0)//사용한다.
            {
                if (itm.type == ItemInfo.Type.BALL)
                {
                    DialogManager.instance.Active(99027, null, DialogManager.Type.NORMAL);
                }
            }
            if (selectNum == 1)//버린다
            {
                if (itm.type == ItemInfo.Type.IMPOTANT)
                {
                    DialogManager.instance.Active(99028, null, DialogManager.Type.NORMAL);
                }
                else
                {
                    DialogManager.instance.Active(99029, this, DialogManager.Type.COUNT, data.items[itmID], 0, itmID, 1);
                }
            }
        }
        else if (args[0] == 1)
        {
            if (selectNum == 0)
            {
                return;
            }

            if (args[1] == 0)//버리기
            {
                var itmID = args[2];
                Debug.Log("버리기 : " + info.info[itmID].name + " [" + selectNum + "] 개를 버리시겠습니까?");
                SelectUIActive(0, itmID, selectNum);
            }
            else
            {
                var itmID = args[2];
                Debug.Log("판매하기 : " + info.info[itmID].name + "[" + selectNum + "] 개를 판매하시겠습니까?");
                SelectUIActive(1, itmID, selectNum);
            }
            ShowAllKeys();
        }
        else if (args[0] == 2)
        {
            if (selectNum == 0)
            {
                var itmID = args[2];
                var itmNum = args[3];
                data.items[itmID] -= itmNum;
                if (data.items[itmID] == 0)
                {
                    data.items.Remove(itmID);
                }
                ShowAllKeys();
                SetString();

                if (args[1] == 0)//버리기
                {
                    Debug.Log("버리기 : " + info.info[itmID].name + " [" + itmNum + "] 개를 버렸습니다");
                }
                else
                {
                    Debug.Log("판매하기 : " + info.info[itmID].name + " [" + itmNum + "] 개를 판매했습니다");
                    
                    Money m = Money.instance;                    
                    int money = m.GetMoney();
                    money = money + info.info[itmID].price * itmNum;
                    m.SetMoney(money);
                }

                
            }
        }

    }

    private void SelectUIActive(int num)
    {
        SelectUI select = SelectUI.instance;
        var cursorMaxNum = 2;
        Vector2 pos = new Vector2(-14f, 54.5f + (cursorMaxNum) * 17.5f);

        int itmID = data.items.Keys.ToList()[num];
        select.Active(cursorMaxNum, "사용한다\n버린다\n그만둔다",this, 240f, pos, 0, itmID);
    }

    private void SelectUIActive(int isShop, int itmID, int num)
    {
        if (isShop == 0)
            DialogManager.instance.Active(99032, this, DialogManager.Type.QUEST, 2, isShop, itmID, num);
        else
            DialogManager.instance.Active(99033, this, DialogManager.Type.QUEST, 2, isShop, itmID, num);
    }

    private void ShopUIActive(int itemID)
    {
        Debug.Log("아이템 판매 : " + info.info[itemID].name);
        DialogManager.instance.Active(99030, this, DialogManager.Type.COUNT, data.items[itemID], 1, itemID, 1);
    }

}
