using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SlideUI
{
    public static Menu Instance;
    private int menuNum;
    private RectTransform cursor;

    private GlobalInput input;

    private float cursorY = 0;
    private float cursorInputStun = 0f;

    public float cursorTmpY = 60f;

    private UIManager.TYPE uiType;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cursor = transform.Find("MenuCursor").GetComponent<RectTransform>();
        input = GlobalInput.globalInput;
        uiType = UIManager.TYPE.MENU;
        SlideUiInit();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        SlideUiUpdate();

        if (isActive )
        {
            CursorUpdate();
        }
        
    }

    void Active()
    {
        SlideUiActive();
        menuNum = 0;

        UIManager.instance.ActiveUI(uiType);
    }

    void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI();
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

        if (UIManager.instance.CheckUITYPE(uiType))
        {
            if (isActive)
            {
                if (input.bButtonDown || input.startButtonDown)
                {
                    UnActive();
                    return;
                }

                if (input.aButtonDown)
                {
                    StartMenu();
                    return;
                }

                cursorInputStun -= Time.deltaTime;
                if (cursorInputStun < 0 && input.verticalRaw != 0)
                {
                    cursorInputStun = 0.2f;

                    menuNum -= (int)input.verticalRaw;

                    if (menuNum < 0) { menuNum = 4; }
                    if (menuNum > 4) { menuNum = 0; }
                }
            }
        }
        
    }


    void CursorUpdate()
    {
        var tmpCursorY = cursorTmpY * menuNum;
        if (cursorY != tmpCursorY)
        {
            cursorY = tmpCursorY/10f + cursorY*9/10f;
            if (Mathf.Abs(cursorY - tmpCursorY) < 1f)
            {
                cursorY = tmpCursorY;
            }
        }

        cursor.anchoredPosition = new Vector2(-59, 125 - cursorY);
    }

    void StartMenu()
    {

        switch (menuNum)
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
}
