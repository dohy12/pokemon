using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class BattleMenu2 : SlideUI, CursorUI
{
    public static BattleMenu2 instance;

    private GlobalInput input;
    private GameObject uiID;
    private Cursor cursor;

    private float selectedAlpha = 0f;
    private int selectedAlphaCh = 1;

    private Image[] elements;

    private FightManager fightManager;
    private FightQueueManager fightQManager;

    public void CursorChange(int pageTmp)
    {
        var poke = fightManager.pokes[0];

        for (int i = 0; i < elements.Length; i++)
        {
            if (poke.skills[i] != 0)
            {
                elements[i].color = Color.white;
                elements[i].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.black;
            }            
        }

        elements[cursor.cursorNum].color = Color.black;
        elements[cursor.cursorNum].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.white;
    }

    public void CursorChoose(int selectNum)
    {
        var eventType = FightQueueManager.BTEventType.USESKILL;
        var poke = fightManager.pokes[0];
        var move = poke.skills[selectNum];

        Debug.Log(poke.GetInfo().name + "ÀÇ " + PokemonSkillInfo.Instance.skills[move].name);
        fightQManager.Active(eventType, move);
        UnActive();
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
            InputCheck();
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
        fightQManager = FightQueueManager.instance;
    }

    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();
        CursorChange(0);

        SetMoves();
    }

    public void UnActive()
    {
        input.InputStun(0.2f);
        SlideUiUnActive();
    }

    public void ActiveMenu1()
    {
        UIManager.instance.UnActiveUI(uiID);
        BattleMenu1.instance.Active();
    }


    private void AlphaUpdate()
    {
        selectedAlpha += Time.deltaTime * selectedAlphaCh * 3;

        if (selectedAlpha > 1.0f) { selectedAlphaCh = -1; }
        if (selectedAlpha < -0.1f) { selectedAlphaCh = 1; }

        elements[cursor.cursorNum].color = new Color(selectedAlpha, selectedAlpha, selectedAlpha);
    }

    private void InputCheck()
    {
        if (input.bButtonDown)
        {
            UnActive();
            Invoke("ActiveMenu1", 0.2f);
        }
    }

    private void SetMoves()
    {
        var skillInfo = PokemonSkillInfo.Instance.skills;

        var poke = fightManager.pokes[0];
        var moves = poke.skills;
        var pp = poke.skillsPP;

        bool cursorCh = false;
        cursor.cursorMaxNum = 4 - 1;

        for (int i=0; i < 4; i++)
        {
            var poke_Move = moves[i];
            if (poke_Move == 0)
            {
                for (int j = 0; j < 3; j++) { elements[i].transform.GetChild(j).gameObject.SetActive(false); }
                elements[i].color = Color.gray;
                
                if (!cursorCh)
                {
                    cursorCh = true;
                    cursor.cursorMaxNum = i - 1;
                }
            }
            else
            {
                for (int j = 0; j < 3; j++) { elements[i].transform.GetChild(j).gameObject.SetActive(true); }
                elements[i].color = Color.white;

                var skill = skillInfo[poke_Move];

                elements[i].transform.Find("Type").GetComponent<Image>().color = PokemonInfo.GetColorFromType2(skill.type);
                elements[i].transform.Find("Type").GetChild(0).GetComponent<TMP_Text>().text = PokemonInfo.TypeToString(skill.type);
                elements[i].transform.Find("Name").GetComponent<TMP_Text>().text = skill.name;
                elements[i].transform.Find("PP").GetChild(0).GetComponent<TMP_Text>().text = pp[i] + "/" + skill.ppMax;
            }
        }

        
    }
}
