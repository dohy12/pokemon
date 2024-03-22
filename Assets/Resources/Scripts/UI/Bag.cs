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

    private Dictionary<int, int> items;
    private int[] showItmList;
    private TMP_Text[] stringObj_Txt;
    private TMP_Text[] stringObj_Num;

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
        items = new Dictionary<int, int>();

        showItmList = new int[5] {-1,-1,-1,-1,-1};
        stringObj_Txt = new TMP_Text[5];
        stringObj_Num = new TMP_Text[5];
        for (var i = 0; i < 5; i++)
        {
            stringObj_Txt[i] = transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            stringObj_Num[i] = transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>();
        }

        items[0] = 1;
        items[1] = 1;
        for (var i = 0; i < 10; i++)
        {
            var n = Random.Range(2, 13);
            items[n] = items.GetValueOrDefault(n, 0) + 1;
        }

    }


    private void SetString(params bool[] args)
    {
        var num = page;
        var itmsList = items.Keys.ToList();
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
                    stringObj_Num[i].text = "X "+(items[itm.id]).ToString("D2");
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
                    if (args[0])
                        cursor.cursorNum = i;
                    isChecked = true;
                    stringObj_Txt[i].text = "�׸��δ�";
                }
                else
                {
                    stringObj_Txt[i].text = "";
                }
                stringObj_Num[i].text = "";
            }
        }
    }


    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();

        page = 0;
        SetString();
    }

    public void UnActive()
    {
        input.InputStun();
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);
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

            if (page > items.Keys.Count - 2)
            {
                page = items.Keys.Count - 2;
            }

            SetString((pageTmp == 1));
        }
    }

    public void CursorChoose(int num)
    {
        if (items.Keys.Count <= num + page)
        {
            UnActive();
        }
        else
        {
            SelectUIActive(num + page);
        }
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 5 - 1;
        float yDist = 60.2f;
        cursor.Init(cursorMaxNum, yDist, false);
    }

    public void OnSelectRedirec(int num, params int[] args)
    {
        if ( num != -1)
        {
            var itmsList = items.Keys.ToList();
            var itm = info.info[itmsList[args[0]]];

            if (num == 0)//����Ѵ�.
            {
                if (itm.type == ItemInfo.Type.BALL)
                {
                    DialogManager.instance.Active(99027, null, DialogManager.Type.NORMAL);
                }
            }
            if (num == 1)//������
            {
                if (itm.type == ItemInfo.Type.IMPOTANT)
                {
                    DialogManager.instance.Active(99028, null, DialogManager.Type.NORMAL);
                }
                else
                {
                    DialogManager.instance.Active(99029, this, DialogManager.Type.COUNT, items[itmsList[args[0]]]);
                }
            }
        }
        else
        {
            Debug.Log("������ [" + args[0] + "] ��");
        }
    }

    private void SelectUIActive(int num)
    {
        SelectUI select = SelectUI.instance;
        var cursorMaxNum = 2;
        Vector2 pos = new Vector2(-14f, 54.5f + (cursorMaxNum) * 17.5f);
        select.Active(cursorMaxNum, "����Ѵ�\n������\n�׸��д�",this, 240f, pos, num);
    }

}
