using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PokeDexInfo;

public class ShopDetail : SlideUI, CursorUI, SelectUIRedirec
{
    public static ShopDetail instance;
    private UIManager uiManager;
    private Cursor cursor;
    private GameObject uiID;
    private GlobalInput input;
    private ItemInfo info;
    private Bag bag;
    private Money money;
    private int page = 0;

    public int[] itmList;

    private TMP_Text[] stringObj_Txt;
    private TMP_Text[] stringObj_Num;

    private GameDataManager data;

    private void Awake()
    {
        instance = this;
    }

    public void CursorChange(int pageTmp)
    {
        if (pageTmp != 0)
        {
            page += pageTmp;
            if (page < 0)
            {
                page = 0;
            }

            if (page > itmList.Length - 2)
            {
                page = itmList.Length - 2;
            }

            SetString((pageTmp == 1));
        }
        
    }

    public void CursorChoose(int selectNum)
    {
        if (itmList.Length <= selectNum + page)
        {
            UnActive();
        }
        else
        {
            var itmID = itmList[selectNum + page];
            var money = Money.instance.GetMoney();
            var itmNum = data.items.GetValueOrDefault(itmID, 0);
            var maxNum = Mathf.Min(money / info.info[itmID].price, 99 - itmNum);
            
            if (money < info.info[itmID].price)
            {
                DialogManager.instance.Active(99036, null, DialogManager.Type.NORMAL); //돈 부족
            }
            else
            {
                DialogManager.instance.Active(99034, this, DialogManager.Type.COUNT, maxNum, 1, itmID, 0);
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

    public bool GetActive()
    {
        return uiManager.CheckUITYPE(uiID);
    }

    public void OnSelectRedirec(int selectNum, params int[] args)
    {
        Debug.Log(selectNum + " " + string.Join(" ", args));
        if (args[0] == 0)
        {
            var itmID = args[2];

            if (selectNum > 0)
            {
                DialogManager.instance.Active(99035, this, DialogManager.Type.QUEST, 1, 1, itmID, selectNum);
            }
        }
        else if (args[0] == 1)
        {
            if (selectNum == 0)
            {                
                var itmID = args[2];
                var itmNum = args[3];
                var itmPrice = info.info[itmID].price;

                data.items[itmID] = data.items.GetValueOrDefault(itmID, 0) + itmNum;
                money.SetMoney(money.GetMoney() - itmNum * itmPrice);

                DialogManager.instance.Active(99037, null, DialogManager.Type.NORMAL); 
            }
        }
        
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
        uiManager = UIManager.instance;
        input = GlobalInput.globalInput;
        SetList();

        stringObj_Txt = new TMP_Text[5];
        stringObj_Num = new TMP_Text[5];
        for (var i = 0; i < 5; i++)
        {
            stringObj_Txt[i] = transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            stringObj_Num[i] = transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>();
        }

        info = ItemInfo.instance;
        bag = Bag.instance;
        money = Money.instance;

        data = GameDataManager.instance;
    }

    public void Active()
    {
        SlideUiActive();
        uiManager.ActiveUI(uiID);
        cursor.Active();
        page = 0;

        SetString();
    }

    public void UnActive()
    {
        SlideUiUnActive();
        uiManager.UnActiveUI(uiID);
        input.InputStun();
    }

    private void InputCheck()
    {
        if (GetActive() && input.bButtonDown)
        {
            UnActive();
        }
    }

    private void SetList()
    {
        itmList = new int[] { 11, 12, 13, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    private void SetString(params bool[] args)
    {
        var isChecked = false;

        cursor.cursorMaxNum = 4;
        for (var i = 0; i < 5; i++)
        {
            if (page + i < itmList.Length)
            {
                var itmID = itmList[page + i];
                var itm = info.info[itmID];
                stringObj_Txt[i].text = itm.name;
                stringObj_Num[i].text = itm.price + " 원";
            }
            else
            {
                if (!isChecked)
                {
                    cursor.cursorMaxNum = i;
                    if (args.Length > 0 && args[0])
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
}

