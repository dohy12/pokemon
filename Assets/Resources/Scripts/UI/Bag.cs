using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : SlideUI , CursorUI
{
    public static Bag instance;
    private GlobalInput input;
    private int uiID = 3001;
    private Cursor cursor;
    private int page = 0;

    private ItemInfo info;

    private Dictionary<int, int> items;
    private int[] showItmList;
    private GameObject[] stringObj;    

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {        
        SlideUiInit();
        input = GlobalInput.globalInput;

        Init();

        info = ItemInfo.instance;
    }


    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
        InputCheck();
    }


    private void Init()
    {
        items = new Dictionary<int, int>();
        for (var i=0;i<14; i++)
        {
            items.Add(0, 0);
        }

        showItmList = new int[5] {-1,-1,-1,-1,-1};
        stringObj = new GameObject[5];
        for (var i = 0; i < 5; i++)
        {
            stringObj[i] = transform.GetChild(i).gameObject;
        }

        items[0] = 1;
        items[1] = 1;
        for (var i = 0; i < 10; i++)
        {
            var n = Random.Range(2, 13);
            items[n] += 1;
        }

        
    }

    private void SetString()
    {
        var num = cursor.cursorNum + page; 

        for (var i = 0;i < 5; i++)
        {

        }    
    }


    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();
    }

    public void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI();
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

    public void CursorChange(int pageTmp) { }

    public void CursorChoose(int num)
    {

    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 5 - 1;
        float yDist = 60.2f;
        cursor.Init(cursorMaxNum, yDist, false);
    }
}
