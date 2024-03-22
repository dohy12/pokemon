using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private CursorUI parent;
    
    private Vector2 startPos;
    private float posY = 0;
    private float yDist;

    private RectTransform cursor;

    private float inputStun = 0;
    private GlobalInput input;

    public int cursorNum = 0;
    public int cursorMaxNum = 0;

    private bool isCursorJump = false;

    private float comboTime = 0;
    private int comboCh = 0;
    public float comboOn = 1;

    // Start is called before the first frame update
    void Start()
    {
        cursor = (RectTransform)transform;
        startPos = cursor.anchoredPosition;

        parent = transform.parent.GetComponent<CursorUI>();
        parent.CursorInit(this);
        
        input = GlobalInput.globalInput;
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
        CursorUpdate();
    }

    void InputUpdate()
    {
        if (parent.GetActive())
        {
            inputStun -= Time.deltaTime;
            comboTime -= Time.deltaTime;

            if (input.verticalRaw !=0 && inputStun < 0)
            {
                ComboUpdate();
                inputStun = 0.2f / comboOn;
                cursorNum -= (int)input.verticalRaw;

                if (cursorNum < 0)
                {
                    if (!isCursorJump)
                    {
                        cursorNum = 0;
                        parent.CursorChange(-1);
                    }
                    else
                    {
                        cursorNum = cursorMaxNum;
                    }                    
                }
                
                if (cursorNum > cursorMaxNum)
                {
                    if (!isCursorJump)
                    {                        
                        cursorNum = cursorMaxNum;
                        parent.CursorChange(1);
                    }
                    else
                    {
                        cursorNum = 0;
                    }
                }

                if(cursorNum >= 0 && cursorNum <= cursorMaxNum)
                {
                    parent.CursorChange(0);
                }
            }

            if (input.aButtonDown && inputStun< 0.1f)
            {
                input.InputStun();
                parent.CursorChoose(cursorNum);                
            }
        }
    }

    void ComboUpdate()
    {
        comboOn = 1f; 
        if (comboTime > 0)
        {
            comboCh++;
            if (comboCh > 2)
            {
                comboOn = 2f;
            }
        }
        else
        {
            comboCh = 1;
        }

        comboTime = 0.21f;
    }

    void CursorUpdate()
    {
        var tmpCursorY = yDist * cursorNum;
        if (posY != tmpCursorY)
        {
            posY = tmpCursorY / 10f + posY * 9 / 10f;
            if (Mathf.Abs(posY - tmpCursorY) < 1f)
            {
                posY = tmpCursorY;
            }
        }

        cursor.anchoredPosition = startPos - new Vector2(0, posY);
    }

    public void Init(int cursorMaxNum, float yDist, bool isCursorJump)
    {
        this.cursorMaxNum = cursorMaxNum;
        this.yDist = yDist;
        this.isCursorJump = isCursorJump;
    }

    public void Active()
    {
        cursorNum = 0;
        posY = 0;
        cursor.anchoredPosition = startPos;
        inputStun = 0.2f;
    }

    public void SetCursor(int cursorNum)
    {
        this.cursorNum = cursorNum;
        posY = yDist * cursorNum;
        cursor.anchoredPosition = startPos - new Vector2(0, posY);
    }
}
