using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu Instance;
    private int menuNum;
    private RectTransform cursor;

    private RectTransform menuRectTransform;
    private bool isActive = false;
    private float posX = 0;
    private float posTime = 0f;
    private GlobalInput input;

    private float cursorY = 0;
    private float cursorInputStun = 0f;

    public float cursorTmpY = 60f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cursor = transform.Find("MenuCursor").GetComponent<RectTransform>();
        input = GlobalInput.globalInput;
        menuRectTransform = (RectTransform)transform;
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        UpdateUi();

        if (isActive )
        {
            CursorUpdate();
        }
        
    }

    void Active()
    {
        posTime = 0f;
        isActive = true;
        menuNum = 0;
        UIManager.instance.SetUIActive(true);
    }

    void UnActive()
    {
        posTime = 0.5f;
        isActive = false;
        UIManager.instance.SetUIActive(false);
    }

    void InputCheck()
    {
        if (input.startButtonDown)
        {
            if (!UIManager.instance.GetUIActive())
            {
                Active();
                return;
            }
        }

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
            if (cursorInputStun < 0 && input.verticalRaw != 0 )
            {
                cursorInputStun = 0.2f;

                menuNum -= (int)input.verticalRaw;

                if (menuNum < 0) { menuNum = 4; }
                if (menuNum > 4) { menuNum = 0; }
            }
        }
    }

    void UpdateUi()
    {
        if (isActive)
        {
            if (posX < 227f)
            {
                posTime += Time.deltaTime * 2;
                posX = -908 * posTime * posTime + 908 * posTime;
                if (posX > 225f)
                {
                    posX = 227f;
                }
                SetUIPos();
            }
        }
        else
        {
            if (posX > 0)
            {
                posTime += Time.deltaTime * 2;
                posX = -908 * posTime * posTime + 908 * posTime;
                if (posX < 2)
                {
                    posX = 0f;
                }
                SetUIPos();
            }
        }
    }

    void SetUIPos()
    {
        menuRectTransform.anchoredPosition = new Vector2(527 - posX,190f);
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



            case 4:
                UnActive();
                break;
        }
    }
}
