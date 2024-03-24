using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Shop : SlideUI, CursorUI
{
    public static Shop instance;
    private GameObject uiID;
    private GlobalInput input;
    private Money money;
    private Cursor cursor;

    

    public void CursorChange(int pageTmp) { }

    public void CursorChoose(int num)
    {
        DialogManager.instance.UnActive();
        switch (num)
        {
            case 0:
                ShopDetail.instance.Active();
                break;

            case 1:
                Bag.instance.Active(true);
                break;

            case 2:
                UnActive();
                break;
        }
        
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 3 - 1;
        float yDist = 60.2f;
        cursor.Init(cursorMaxNum, yDist, false);
    }

    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID); }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SlideUiInit();
        Init();
    }

    void Update()
    {
        SlideUiUpdate();
        InputCheck();
    }

    void Init()
    {
        input = GlobalInput.globalInput;
        money = transform.parent.Find("Money").GetComponent<Money>();
        uiID = gameObject;
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

    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        money.Active();
        cursor.Active();
        input.InputStun();
    }


    public void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);
        money.UnActive();
        input.InputStun();

        EventManager.instance.StartEvent(999, 25002);
    }
}
