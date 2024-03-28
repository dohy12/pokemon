using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonList : SlideUI, CursorUI, SelectUIRedirec
{
    public static PokemonList instance; 
    private GlobalInput input;
    private GameObject uiID;
    public Cursor cursor;
    private RectTransform[] lists;
    private float[] listsXpos;

    private bool isChanging = false;
    private int changeNum = 0;
    private RectTransform changeCursor;
        
    public Sprite[] hpBarSpr;

    
    private void Awake()
    {
        uiID = gameObject;
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        Init();
        SlideUiInit();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        SlideUiUpdate();

        if (GetActive())
        {
            ListPosUpdate();
        }
        
    }

    void Init()
    {
        lists = new RectTransform[6];

        for (var i = 0; i < lists.Length; i++)
        {
            lists[i] = (RectTransform)transform.GetChild(i);
        }

        listsXpos = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f};

        changeCursor = (RectTransform)transform.Find("FixedCursor");
        changeCursor.gameObject.SetActive(false);
    }

    void ShowList()
    {
        var pokeList = GameDataManager.instance.pokeList;

        for (var i = 0; i < lists.Length; i++)
        {
            lists[i].gameObject.SetActive(false);
        }

        for (var i = 0; i < pokeList.Count; i++)
        {
            lists[i].gameObject.SetActive(true);
            ShowLine(lists[i].gameObject, pokeList[i]);
        }

        cursor.cursorMaxNum = pokeList.Count - 1;
    }

    void ShowLine(GameObject obj, Poke poke)
    {
        Image img = obj.GetComponent<Image>();
        TMP_Text name = obj.transform.Find("Name").GetComponent<TMP_Text>();
        TMP_Text level = obj.transform.Find("Level").GetComponent<TMP_Text> ();
        RectTransform hpBar = (RectTransform)obj.transform.Find("HPBar").GetChild(1);

        img.sprite = PokeSpr.instance.icons[poke.id];
        name.text = PokemonInfo.Instance.pokemons[poke.id].name;
        level.text = "Lv. " + poke.level;


        hpBar.sizeDelta = new Vector2(458.7765f / poke.stat[0] * poke.hp, 24f);

        if (poke.hp > poke.stat[0] * 2 / 3f)
            hpBar.GetComponent<Image>().sprite = hpBarSpr[0];
        else if (poke.hp > poke.stat[0] * 1 / 3f)
            hpBar.GetComponent<Image>().sprite = hpBarSpr[1];
        else
            hpBar.GetComponent<Image>().sprite = hpBarSpr[2];
    }

    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        cursor.Active();

        ShowList();
    }

    public void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);
        input.InputStun();
    }

    void InputCheck()
    {
        if (UIManager.instance.CheckUITYPE(uiID))
        {
            if (isActive)
            {
                if (!isChanging)
                {
                    if (input.bButtonDown)
                    {
                        UnActive();
                        return;
                    }
                }
                else
                {
                    if (input.bButtonDown)
                    {
                        isChanging = false;
                        return;
                    }
                }
                
            }
        }

    }

    void ListPosUpdate()
    {
        for (var i = 0;i < lists.Length;i++)
        {
            if (cursor.cursorNum == i)
                listsXpos[i] = 73f;
            else
                listsXpos[i] = 57f;

            if (isChanging && changeNum == i)
            {
                listsXpos[i] = 73f;
            }

            Vector2 pos = lists[i].anchoredPosition;
            if (pos.x != listsXpos[i])
            {
                float xPos = pos.x * 9 / 10 + listsXpos[i] /10;
                if (Mathf.Abs(xPos - listsXpos[i]) < 1f)
                {
                    xPos = listsXpos[i];
                }
                    
                lists[i].anchoredPosition = new Vector2(xPos, pos.y);                
            }
            
        }
    }


    public void CursorChange(int pageTmp){ }

    public void CursorChoose(int selectNum)
    {
        if (!isChanging)
        {
            SelectUIActive(selectNum);
        }            
        else
        {
            var pokelist = GameDataManager.instance.pokeList;
            var tmp_poke = pokelist[selectNum];
            pokelist[selectNum] = pokelist[changeNum];
            pokelist[changeNum] = tmp_poke;            

            lists[selectNum].gameObject.SetActive(false);
            lists[changeNum].gameObject.SetActive(false);
            input.InputStun(0.2f);
            Invoke("Change", 0.2f);            
        }
            
    }

    private void Change()
    {
        for (var i = 0; i < lists.Length; i++) { lists[i].gameObject.SetActive(true); }
        ShowList();
        StopChanging();
    }

    private void StartChanging(int selectedNum)
    {
        isChanging = true;
        changeNum = selectedNum;
        changeCursor.gameObject.SetActive(true);
        changeCursor.anchoredPosition = new Vector2(70, -82 - 70.88f * changeNum);
    }

    private void StopChanging()
    {
        isChanging = false;
        changeCursor.gameObject.SetActive(false);

    }

    private void SelectUIActive(int selectNum)
    {
        SelectUI select = SelectUI.instance;
        var cursorMaxNum = 2;
        Vector2 pos = new Vector2(-14f, 54.5f + (cursorMaxNum) * 17.5f);

        select.Active(cursorMaxNum, "상세보기\n순서바꾸기\n그만둔다", this, 280f, pos, selectNum);
    }


    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 6 - 1;
        float yDist = 70.88f;
        cursor.Init(cursorMaxNum, yDist, true);
    }

    public bool GetActive()
    {
        return UIManager.instance.CheckUITYPE(uiID);
    }

    public void OnSelectRedirec(int num, params int[] args)
    {
        switch (num)
        {
            case 0:
                PokeDetail.instance.Active();
                break;

            case 1://순서바꾸기
                StartChanging(args[0]);
                break;

        }
    }

    public int GetCursorNum()
    {
        return cursor.cursorNum;
    }
}
