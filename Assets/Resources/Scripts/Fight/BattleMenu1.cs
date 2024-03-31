using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class BattleMenu1 : SlideUI, CursorUI
{
    public static BattleMenu1 instance;

    private GlobalInput input;
    private GameObject uiID;
    private Cursor cursor;

    private float selectedAlpha = 0f;
    private int selectedAlphaCh = 1;

    private Image[] elements;

    private FightManager fightManager;

    public void CursorChange(int pageTmp)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].color = Color.white;
            elements[i].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.black;
        }

        elements[cursor.cursorNum].color = Color.black;
        elements[cursor.cursorNum].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.white;
    }

    public void CursorChoose(int selectNum)
    {
        switch(selectNum)
        {
            case 0://싸운다
                BattleMenu2.instance.Active();
                UnActive();
                break;

            case 1://포켓몬
                PokemonList.instance.Active();
                break;

            case 2://가방
                Bag.instance.Active();
                break;

            case 3://도주
                if (fightManager.isTrainerBattle)
                {
                    DialogManager.instance.Active(99039);
                }
                break;
        }
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 4 - 1;
        float yDist = 57.2333333333f;
        cursor.Init(cursorMaxNum, yDist, false);
    }

    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID); }

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

        if (GetActive())
        {
            AlphaUpdate();
        }
        
    }

    void Init()
    {
        uiID = gameObject;
        input = GlobalInput.globalInput;

        elements = new Image[4];
        for(int i = 0; i < 4; i++) 
        {
            elements[i] = transform.GetChild(i).GetComponent<Image>();
        }

        fightManager = FightManager.instance;
    }

    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();
    }

    public void UnActive()
    {
        input.InputStun();
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);
    }

    private void AlphaUpdate()
    {
        selectedAlpha += Time.deltaTime * selectedAlphaCh * 3;

        if (selectedAlpha > 1.0f) { selectedAlphaCh = -1; }
        if (selectedAlpha < -0.1f) { selectedAlphaCh = 1; }

        elements[cursor.cursorNum].color = new Color(selectedAlpha, selectedAlpha, selectedAlpha);
    }
}
