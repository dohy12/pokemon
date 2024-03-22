using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SelectUI : MonoBehaviour, CursorUI
{
    public static SelectUI instance;
    private UIManager uiManager;
    private int uiID = -999;
    private Cursor cursor;
    private SelectUIRedirec redirec;
    private int cursorMaxNum;
    private float uiWidth;
    private Vector2 uiPos;
    private int[] args;

    private GlobalInput input;

    private void Awake()
    {
        instance = this;
    }

    public void Active(int uiID, int cursorMaxNum, string menus, SelectUIRedirec redirec, float width, Vector2 pos, params int[] args)
    {
        this.uiID = uiID;
        this.cursorMaxNum = cursorMaxNum;
        this.redirec = redirec;
        this.uiWidth = width;
        this.uiPos = pos;
        this.args = args;
        cursor.cursorMaxNum = this.cursorMaxNum; ;

        transform.Find("Text").GetComponent<TMP_Text>().text = menus;
        cursor.Active();
        uiManager.ActiveUI(uiID);

        SetSize();
    }

    private void SetSize()
    {
        RectTransform rect = (RectTransform)transform;
        rect.sizeDelta = new Vector2(uiWidth, 85f + cursorMaxNum * 35f);
        rect.anchoredPosition = uiPos;
    }

    private void UnActive()
    {
        uiManager.UnActiveUI(uiID);
        input.InputStun();

        RectTransform rect = (RectTransform)transform;
        rect.anchoredPosition = new Vector2(9999f, 9999f);
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        input = GlobalInput.globalInput;
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    public void InputCheck()
    {
        if(GetActive() && input.bButtonDown)
        {
            input.InputStun();
            UnActive();
            redirec.OnSelectRedirec(cursorMaxNum, args);
        }
    }

    public void CursorChange(int pageTmp)
    {
        
    }

    public void CursorChoose(int num)
    {
        UnActive();
        redirec.OnSelectRedirec(num, args);        
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 1;
        float yDist = 35f;
        cursor.Init(cursorMaxNum, yDist, false);
    }

    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID); }
}
