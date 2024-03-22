using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SlideUI, CursorUI
{
    public static Menu Instance;
    private GlobalInput input;
    private GameObject uiID;
    private Cursor cursor;

    private void Awake()
    {
        uiID = gameObject;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        SlideUiInit();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        SlideUiUpdate();
    }

    void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();
    }

    void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);
        input.InputStun();
    }

    void InputCheck()
    {
        if (input.startButtonDown)
        {
            if (!UIManager.instance.GetUIActive() && !EventManager.isEvent)
            {
                Active();
                return;
            }
        }

        if (UIManager.instance.CheckUITYPE(uiID))
        {
            if (isActive)
            {
                if (input.bButtonDown || input.startButtonDown)
                {
                    UnActive();
                    return;
                }
            }
        }
        
    }


    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID);}

    public void CursorChange(int pageTmp) { }

    public void CursorChoose(int num)
    {
        switch (num)
        {
            case 0://포켓몬 도감
                PokeDexManager.instance.ActivePokedex();
                break;

            case 2://가방
                Bag.instance.Active();
                break;


            case 4:
                UnActive();
                break;
        }
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 5 - 1;
        float yDist = 60f;
        cursor.Init(cursorMaxNum, yDist, true);
    }
}
